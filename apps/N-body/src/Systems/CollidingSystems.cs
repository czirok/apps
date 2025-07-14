using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with ChatGPT.
/// </summary>
public partial class World
{
	void SystemTypeCollidingSystems()
	{
		float distance = 2e6f / 3f;
		float velocity = 2e3f;
		float centralMass = 1e10f;
		float planetMass = 1e6f;
		int planetsPerSystem = (_bodies.Length - 2) / 2;

		// Left system (moving right) – orange
		Vector4 leftColor = new Vector4(1.0f, 0.5f, 0.1f, 1.0f); // star color
		Vector4 leftDustColor = new Vector4(1.0f, 0.4f, 0.2f, 0.7f); // dust color

		_bodies[0] = new Body(new Vector3(-distance, 0, 0), centralMass, new Vector3(+velocity, 0, 0), leftColor);
		for (int i = 0; i < planetsPerSystem; i++)
		{
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float radius = PseudoRandom.GetRandomNumber(1e5f) + _bodies[0].Radius;
			Vector3 location = new Vector3((float)Math.Cos(angle) * radius, 0, (float)Math.Sin(angle) * radius) + _bodies[0].Location;
			float speed = (float)Math.Sqrt(centralMass * centralMass * G / ((centralMass + planetMass) * radius));
			Vector3 velocityVector = Vector3.Normalize(Vector3.Cross(location - _bodies[0].Location, Vector3.UnitY)) * speed + new Vector3(+velocity, 0, 0);

			_bodies[i + 1] = new Body(location, planetMass, velocityVector, leftDustColor);
		}

		// Right system (moving left) – light blue
		Vector4 rightColor = new Vector4(0.2f, 0.6f, 1.0f, 1.0f); // star color
		Vector4 rightDustColor = new Vector4(0.3f, 0.7f, 1.0f, 0.7f); // dust color

		_bodies[planetsPerSystem + 1] = new Body(new Vector3(+distance, 0, 0), centralMass, new Vector3(-velocity, 0, 0), rightColor);
		for (int i = 0; i < planetsPerSystem; i++)
		{
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float radius = PseudoRandom.GetRandomNumber(1e5f) + _bodies[planetsPerSystem + 1].Radius;
			Vector3 location = new Vector3((float)Math.Cos(angle) * radius, 0, (float)Math.Sin(angle) * radius) + _bodies[planetsPerSystem + 1].Location;
			float speed = (float)Math.Sqrt(centralMass * centralMass * G / ((centralMass + planetMass) * radius));
			Vector3 velocityVector = Vector3.Normalize(Vector3.Cross(location - _bodies[planetsPerSystem + 1].Location, Vector3.UnitY)) * speed + new Vector3(-velocity, 0, 0);

			_bodies[i + planetsPerSystem + 2] = new Body(location, planetMass, velocityVector, rightDustColor);
		}
	}
}
