using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Perplexity Sonar.
/// </summary>
public partial class World
{
	void SystemTypeAuroraHarmonia()
	{
		// Harmony attractor points (e.g. vertices of a hexagon + center)
		Vector3[] harmonyPoints =
		[
			new(1e5f, 0, 0),
			new(-1e5f, 0, 0),
			new(0, 1e5f, 0),
			new(0, -1e5f, 0),
			new(7e4f, 7e4f, 0),
			new(-7e4f, -7e4f, 0),
			new(0, 0, 0) // center
        ];

		// Invisible attractors
		for (int i = 0; i < harmonyPoints.Length && i < _bodies.Length; i++)
		{
			_bodies[i] = new Body(
				harmonyPoints[i],
				5e11f, // large mass
				Vector3.Zero,
				new Vector4(0, 0, 0, 0) // fully transparent
			);
		}

		// Additional bodies: symmetric, but randomly offset
		int start = harmonyPoints.Length;
		for (int i = start; i < _bodies.Length; i++)
		{
			float angle = (i - start) * (2 * MathF.PI) / (_bodies.Length - start) + PseudoRandom.GetRandomNumber(-0.2f, 0.2f);
			float radius = PseudoRandom.GetRandomNumber(8e4f, 1.5e5f);
			Vector3 pos = new Vector3(MathF.Cos(angle) * radius, MathF.Sin(angle) * radius, PseudoRandom.GetRandomNumber(-2e4f, 2e4f));
			float mass = PseudoRandom.GetRandomNumber(1e5f, 5e6f);
			Vector3 velocity = Vector3.Normalize(Vector3.Cross(pos, Vector3.UnitY)) * PseudoRandom.GetRandomNumber(150f, 500f);

			// Color: based on acceleration, mass, and orbital curvature
			float accelFactor = MathF.Min(1.0f, velocity.Length() / 500f);
			Vector4 color = new Vector4(
				0.2f + 0.8f * accelFactor,
				0.5f + 0.5f * MathF.Sin(angle * 3),
				1.0f - accelFactor,
				1.0f
			);

			_bodies[i] = new Body(pos, mass, velocity, color);
		}
	}
}