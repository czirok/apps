using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeMassiveBody()
	{
		_bodies[0] = new Body(Vector3.Zero, 1e10f);

		var location1 = PseudoRandom.Vector3(8e3f) + new Vector3(-3e5f, 1e5f + _bodies[0].Radius, 0);
		var mass1 = 1e6f;
		var velocity1 = new Vector3(2e3f, 0, 0);
		_bodies[1] = new Body(location1, mass1, velocity1);

		for (int i = 2; i < _bodies.Length; i++)
		{
			var distance = PseudoRandom.GetRandomNumber(2e5f) + _bodies[1].Radius;
			var angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			var vertical = Math.Min(2e8f / distance, 2e4f);
			var location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-vertical, vertical), (float)Math.Sin(angle) * distance) + _bodies[1].Location;
			var mass = PseudoRandom.GetRandomNumber(5e5f) + 1e5f;
			var speed = (float)Math.Sqrt(_bodies[1].Mass * _bodies[1].Mass * G / ((_bodies[1].Mass + mass) * distance));
			var velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * speed + velocity1;
			location = location.Rotate(Vector3.Zero, Vector3.One, (float)(Math.PI * 0.1));
			velocity = velocity.Rotate(Vector3.Zero, Vector3.One, (float)(Math.PI * 0.1));
			_bodies[i] = new Body(location, mass, velocity);
		}
	}
}