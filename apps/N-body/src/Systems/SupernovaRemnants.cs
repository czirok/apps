using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with ChatGPT.
/// </summary>
public partial class World
{
	void SystemTypeSupernovaRemnants()
	{
		const float coreMass = 1e12f;
		const float gasMass = 1e6f;
		const float shellRadius = 2e5f;
		const float explosionVelocity = 2e3f;
		const int gammaRays = 100;
		int gasParticles = _bodies.Length - gammaRays - 1;

		// Neutron star in the center, very small but extremely massive
		_bodies[0] = new Body(
			Vector3.Zero,
			coreMass,
			Vector3.Zero,
			new Vector4(0.8f, 0.9f, 1.0f, 1.0f)
		);

		// Gamma burst in two cones (±Y direction)
		for (int i = 0; i < gammaRays / 2; i++)
		{
			float theta = PseudoRandom.GetRandomNumber(0, MathF.PI / 10); // Narrow angle
			float phi = PseudoRandom.GetRandomNumber(0, 2 * MathF.PI);
			Vector3 dir = SphericalDirection(theta, phi);
			Vector3 vel = dir * 5e3f;

			Vector4 color = new Vector4(0.9f, 0.7f, 1.0f, 1.0f); // Purple gamma
			_bodies[i + 1] = new Body(Vector3.Zero, gasMass, vel, color);
		}

		for (int i = gammaRays / 2; i < gammaRays; i++)
		{
			float theta = PseudoRandom.GetRandomNumber(0, MathF.PI / 10);
			float phi = PseudoRandom.GetRandomNumber(0, 2 * MathF.PI);
			Vector3 dir = SphericalDirection(MathF.PI - theta, phi);
			Vector3 vel = dir * 5e3f;

			Vector4 color = new Vector4(0.8f, 0.6f, 1.0f, 1.0f);
			_bodies[i + 1] = new Body(Vector3.Zero, gasMass, vel, color);
		}

		// Gas cloud particles: on a circular shock front
		for (int i = 0; i < gasParticles; i++)
		{
			float angle = PseudoRandom.GetRandomNumber(0, 2 * MathF.PI);
			float z = PseudoRandom.GetRandomNumber(-shellRadius / 4, shellRadius / 4);
			float r = shellRadius + PseudoRandom.GetRandomNumber(-1e4f, 1e4f);
			Vector3 pos = new Vector3(MathF.Cos(angle) * r, z, MathF.Sin(angle) * r);

			Vector3 vel = Vector3.Normalize(pos) * explosionVelocity;

			// Color: from green to red based on distance
			float t = r / (shellRadius + 1e4f);
			Vector4 color = new Vector4(1.0f, 0.8f - t * 0.3f, 0.4f + t * 0.2f, 1.0f);

			float mass = PseudoRandom.GetRandomNumber(gasMass * 0.5f, gasMass * 1.5f);
			_bodies[i + gammaRays + 1] = new Body(pos, mass, vel, color);
		}
	}

	private Vector3 SphericalDirection(float theta, float phi)
	{
		return new Vector3(
			MathF.Sin(theta) * MathF.Cos(phi),
			MathF.Cos(theta),
			MathF.Sin(theta) * MathF.Sin(phi)
		);
	}
}