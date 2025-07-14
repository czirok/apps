using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeOrbitalSystem()
	{
		_bodies[0] = new Body(mass: 1e10f);

		for (var i = 1; i < _bodies.Length; i++)
		{
			var distance = PseudoRandom.GetRandomNumber(1e6f) + _bodies[0].Radius;
			var angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			var location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-2e4f, 2e4f), (float)Math.Sin(angle) * distance);
			var mass = PseudoRandom.GetRandomNumber(1e6f) + 3e4f;
			var speed = (float)Math.Sqrt(_bodies[0].Mass * _bodies[0].Mass * G / ((_bodies[0].Mass + mass) * distance));
			var velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * speed;
			_bodies[i] = new Body(location, mass, velocity);
		}
	}
}