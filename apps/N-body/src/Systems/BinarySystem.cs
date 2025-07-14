using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeBinarySystem()
	{
		float mass1 = PseudoRandom.GetRandomNumber(9e9f) + 1e9f;
		float mass2 = PseudoRandom.GetRandomNumber(9e9f) + 1e9f;
		float angle0 = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
		float distance0 = PseudoRandom.GetRandomNumber(1e5f) + 3e4f;
		float distance1 = distance0 / 2;
		float distance2 = distance0 / 2;
		Vector3 location1 = new Vector3((float)Math.Cos(angle0) * distance1, 0, (float)Math.Sin(angle0) * distance1);
		Vector3 location2 = new Vector3((float)-Math.Cos(angle0) * distance2, 0, (float)-Math.Sin(angle0) * distance2);
		float speed1 = (float)Math.Sqrt(mass2 * mass2 * G / ((mass1 + mass2) * distance0));
		float speed2 = (float)Math.Sqrt(mass1 * mass1 * G / ((mass1 + mass2) * distance0));
		Vector3 velocity1 = Vector3.Normalize(Vector3.Cross(location1, Vector3.UnitY)) * speed1;
		Vector3 velocity2 = Vector3.Normalize(Vector3.Cross(location2, Vector3.UnitY)) * speed2;
		_bodies[0] = new Body(location1, mass1, velocity1);
		_bodies[1] = new Body(location2, mass2, velocity2);

		for (int i = 2; i < _bodies.Length; i++)
		{
			float distance = PseudoRandom.GetRandomNumber(1e6f);
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			Vector3 location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-2e4f, 2e4f), (float)Math.Sin(angle) * distance);
			float mass = PseudoRandom.GetRandomNumber(1e6f) + 3e4f;
			float speed = (float)Math.Sqrt((mass1 + mass2) * (mass1 + mass2) * G / ((mass1 + mass2 + mass) * distance));
			speed /= distance >= distance0 / 2 ? 1 : distance0 / 2 / distance;
			Vector3 velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * speed;
			_bodies[i] = new Body(location, mass, velocity);
		}
	}
}