using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeFastParticles()
	{
		for (int i = 0; i < _bodies.Length; i++)
		{
			var distance = PseudoRandom.GetRandomNumber(1e6f);
			var angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			var location = new Vector3((float)Math.Cos(angle) * distance, PseudoRandom.GetRandomNumber(-2e5f, 2e5f), (float)Math.Sin(angle) * distance);
			var mass = PseudoRandom.GetRandomNumber(1e6f) + 3e4f;
			var velocity = PseudoRandom.Vector3(5e3f);
			_bodies[i] = new Body(location, mass, velocity);
		}
	}
}