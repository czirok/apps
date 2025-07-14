using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypePlanetarySystem()
	{
		_bodies[0] = new Body(mass: 1e10f);
		int planets = PseudoRandom.Random.Next(10) + 5;
		int planetsWithRings = PseudoRandom.Random.Next(1) + 1;
		int k = 1;
		for (int i = 1; i < planets + 1 && k < _bodies.Length; i++)
		{
			int planetK = k;
			float distance = PseudoRandom.GetRandomNumber(2e6f) + 1e5f + _bodies[0].Radius;
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			Vector3 location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-2e4f, 2e4f), (float)Math.Sin(angle) * distance);
			float mass = PseudoRandom.GetRandomNumber(1e8f) + 1e7f;
			float speed = (float)Math.Sqrt(_bodies[0].Mass * _bodies[0].Mass * G / ((_bodies[0].Mass + mass) * distance));
			Vector3 velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * speed;
			_bodies[k++] = new Body(location, mass, velocity);

			// Generate rings.
			const int RingParticles = 100;
			if (--planetsWithRings >= 0 && k < _bodies.Length - RingParticles)
			{
				for (int j = 0; j < RingParticles; j++)
				{
					float ringDistance = PseudoRandom.GetRandomNumber(1e1f) + 1e4f + _bodies[planetK].Radius;
					float ringAngle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
					Vector3 ringLocation = location + new Vector3((float)Math.Cos(ringAngle) * ringDistance, 0, (float)Math.Sin(ringAngle) * ringDistance);
					float ringMass = PseudoRandom.GetRandomNumber(1e3f) + 1e3f;
					float ringSpeed = (float)Math.Sqrt(_bodies[planetK].Mass * _bodies[planetK].Mass * G / ((_bodies[planetK].Mass + ringMass) * ringDistance));
					Vector3 ringVelocity = Vector3.Normalize(Vector3.Cross(location - ringLocation, Vector3.UnitY)) * ringSpeed + velocity;
					_bodies[k++] = new Body(ringLocation, ringMass, ringVelocity);
				}
				continue;
			}

			// Generate moons. 
			int moons = PseudoRandom.Random.Next(4);
			while (moons-- > 0 && k < _bodies.Length)
			{
				float moonDistance = PseudoRandom.GetRandomNumber(1e4f) + 5e3f + _bodies[planetK].Radius;
				float moonAngle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
				Vector3 moonLocation = location + new Vector3((float)Math.Cos(moonAngle) * moonDistance, PseudoRandom.GetRandomNumber(-2e3f, 2e3f), (float)Math.Sin(moonAngle) * moonDistance);
				float moonMass = PseudoRandom.GetRandomNumber(1e6f) + 1e5f;
				float moonSpeed = (float)Math.Sqrt(_bodies[planetK].Mass * _bodies[planetK].Mass * G / ((_bodies[planetK].Mass + moonMass) * moonDistance));
				Vector3 moonVelocity = Vector3.Normalize(Vector3.Cross(moonLocation - location, Vector3.UnitY)) * moonSpeed + velocity;
				_bodies[k++] = new Body(moonLocation, moonMass, moonVelocity);
			}
		}

		// Generate asteroid belt.
		while (k < _bodies.Length)
		{
			float asteroidDistance = PseudoRandom.GetRandomNumber(4e5f) + 1e6f;
			float asteroidAngle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			Vector3 asteroidLocation = new Vector3((float)Math.Cos(asteroidAngle) * asteroidDistance, PseudoRandom.GetRandomNumber(-1e3f, 1e3f), (float)Math.Sin(asteroidAngle) * asteroidDistance);
			float asteroidMass = PseudoRandom.GetRandomNumber(1e6f) + 3e4f;
			float asteroidSpeed = (float)Math.Sqrt(_bodies[0].Mass * _bodies[0].Mass * G / ((_bodies[0].Mass + asteroidMass) * asteroidDistance));
			Vector3 asteroidVelocity = Vector3.Normalize(Vector3.Cross(asteroidLocation, Vector3.UnitY)) * asteroidSpeed;
			_bodies[k++] = new Body(asteroidLocation, asteroidMass, asteroidVelocity);
		}

	}
}