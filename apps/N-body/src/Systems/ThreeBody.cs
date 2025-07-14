using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with ChatGPT.
/// </summary>
public partial class World
{
	void SystemTypeThreeBody()
	{
		// Scaling factors – can be adjusted for camera needs
		const float S = 1e6f;   // position scale
		const float V = 1e3f;   // velocity scale
		const float M = 1e10f;  // mass of each star

		// Yellow-orange
		Vector4 glowingOrange = new Vector4(1.0f, 0.8f, 0.0f, 1.0f);

		// Figure-eight periodic orbit (Moore, 1993) dimensionless data, but closer together
		var p = new[]
		{
			new Vector3(-0.5f,  0.1f, 0) * S,
			new Vector3( 0.5f, -0.1f, 0) * S,
			new Vector3( 0f,    0f,   0) * S
		};

		var v = new[]
		{
			new Vector3( 0.466203685f,  0.43236573f, 0) * V,
			new Vector3( 0.466203685f,  0.43236573f, 0) * V,
			new Vector3(-0.93240737f,  -0.86473146f, 0) * V
		};

		for (int i = 0; i < 3 && i < _bodies.Length; i++)
		{
			_bodies[i] = new Body(p[i], M, v[i], glowingOrange);
		}

		// If there is still space in the array, fill it with colorful "dust" particles for enhanced visual effect
		for (int i = 3; i < _bodies.Length; i++)
		{
			float r = PseudoRandom.GetRandomNumber(2e6f);
			float a = PseudoRandom.GetRandomNumber((float)(2 * Math.PI));
			var loc = new Vector3(MathF.Cos(a) * r, PseudoRandom.GetRandomNumber(-5e4f, 5e4f), MathF.Sin(a) * r);
			float m = PseudoRandom.GetRandomNumber(5e5f) + 1e4f;

			// Color and mass relationship
			Vector4 color = new Vector4(
				MathF.Abs(MathF.Sin(m * 0.00001f)),  // Red
				MathF.Abs(MathF.Cos(m * 0.00002f)),  // Green
				MathF.Abs(MathF.Sin(m * 0.00003f)),  // Blue
				1.0f
			);

			_bodies[i] = new Body(loc, m, Vector3.Zero, color); // initial velocity 0 → falls into chaos
		}
	}
}