using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Mistral.
/// </summary>
public partial class World
{
	void SystemTypeGalacticDance()
	{
		// Scaling factors – can be adjusted for camera needs
		const float S = 1e0f;   // position scale
		const float V = 1e3f;   // velocity scale

		// Central star
		_bodies[0] = new Body(Vector3.Zero, mass: 1e12f, color: new Vector4(1, 1, 0, 1)); // Yellow

		// Generate planets and stars
		for (int i = 1; i < _bodies.Length / 2; i++)
		{
			// Chaotic but beautiful placement
			float distance = PseudoRandom.GetRandomNumber(1e5f, 1e6f) * S;
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float height = PseudoRandom.GetRandomNumber(-1e5f, 1e5f) * S;
			Vector3 location = new Vector3(
				MathF.Cos(angle) * distance,
				height,
				MathF.Sin(angle) * distance
			);

			// Mass and color relationship
			float mass = PseudoRandom.GetRandomNumber(1e5f, 1e9f);
			Vector4 color = new Vector4(
				PseudoRandom.GetRandomNumber(0f, 1f),  // Red
				PseudoRandom.GetRandomNumber(0f, 1f),  // Green
				PseudoRandom.GetRandomNumber(0f, 1f),  // Blue
				1.0f
			);

			// Velocity: circular motion around the center
			Vector3 velocity = new Vector3(
				-location.Z * V / distance,
				0,
				location.X * V / distance
			);

			_bodies[i] = new Body(location, mass, velocity, color);
		}

		// Generate stardust
		for (int i = _bodies.Length / 2; i < _bodies.Length; i++)
		{
			float r = PseudoRandom.GetRandomNumber(2e6f) * S;
			float a = PseudoRandom.GetRandomNumber((float)(2 * Math.PI));
			var loc = new Vector3(MathF.Cos(a) * r, PseudoRandom.GetRandomNumber(-5e4f, 5e4f) * S, MathF.Sin(a) * r);
			float m = PseudoRandom.GetRandomNumber(5e3f) + 1e2f;

			// Bright colored stardust
			Vector4 color = new Vector4(
				PseudoRandom.GetRandomNumber(0.8f, 1f),  // Red
				PseudoRandom.GetRandomNumber(0.8f, 1f),  // Green
				PseudoRandom.GetRandomNumber(0.8f, 1f),  // Blue
				1.0f
			);

			_bodies[i] = new Body(loc, m, Vector3.Zero, color); // initial velocity 0 → falls into chaos
		}
	}
}