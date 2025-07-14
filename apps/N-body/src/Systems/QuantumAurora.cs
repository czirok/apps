using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Deepseek.
/// </summary>
public partial class World
{
	void SystemTypeQuantumAurora()
	{
		// Central "energy source" (white hole? quantum singularity?)
		_bodies[0] = new Body(Vector3.Zero, mass: 1e12f, color: new Vector4(1, 1, 1, 1));

		// Generating quantum particles
		for (int i = 1; i < _bodies.Length; i++)
		{
			// Chaotic, but fractal-like arrangement
			float distance = PseudoRandom.GetRandomNumber(1e6f) * (i % 3 == 0 ? 0.2f : 1f);
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float height = PseudoRandom.GetRandomNumber(-1e5f, 1e5f) * MathF.Sin(i * 0.7f);
			Vector3 location = new Vector3(
				MathF.Cos(angle) * distance,
				height,
				MathF.Sin(angle) * distance
			);

			// Mass and color relationship
			float mass = PseudoRandom.GetRandomNumber(1e5f, 1e7f);
			Vector4 color = new Vector4(
				MathF.Abs(MathF.Sin(mass * 0.00001f)),  // Red
				MathF.Abs(MathF.Cos(mass * 0.00002f)),  // Green
				MathF.Abs(MathF.Sin(mass * 0.00003f)),  // Blue
				1.0f
			);

			// Velocity: not a classical orbit, but a "chaotic impulse"
			Vector3 velocity = Vector3.Normalize(new Vector3(
				PseudoRandom.GetRandomNumber(-1f, 1f),
				PseudoRandom.GetRandomNumber(-0.3f, 0.3f),
				PseudoRandom.GetRandomNumber(-1f, 1f)
			) * (mass * 0.003f));

			_bodies[i] = new Body(location, mass, velocity, color);
		}

	}
}