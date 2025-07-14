using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeSlowParticles()
	{
		for (var i = 0; i < _bodies.Length; i++)
		{
			var distance = PseudoRandom.GetRandomNumber(1e6f);
			var angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2f));
			var location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-2e5f, 2e5f), (float)Math.Sin(angle) * distance);
			var mass = PseudoRandom.GetRandomNumber(1e6f) + 3e4f;
			var velocity = PseudoRandom.Vector3(5);
			_bodies[i] = new Body(location, mass, velocity);
		}
	}
}