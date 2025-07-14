using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Claude.
/// </summary>
public partial class World
{
	void SystemTypeGalacticSpiralChaos()
	{
		// Central pulsating neutron star - gravitationally attractive
		float centralMass = 3e12f;
		Vector4 centralColor = new Vector4(0.9f, 0.4f, 1.0f, 1.0f); // vivid purple-pink
		_bodies[0] = new Body(Vector3.Zero, centralMass, Vector3.Zero, centralColor); // appears stationary, but attracts gravitationally

		// Number of spiral arms
		int spiralArms = 4;
		int starsPerArm = (_bodies.Length - 1) / spiralArms;

		for (int arm = 0; arm < spiralArms; arm++)
		{
			float armAngleOffset = arm * (2f * MathF.PI / spiralArms);

			for (int star = 0; star < starsPerArm && (arm * starsPerArm + star + 1) < _bodies.Length; star++)
			{
				int bodyIndex = arm * starsPerArm + star + 1;

				// Spiral arrangement
				float normalizedDistance = (float)star / starsPerArm; // between 0-1
				float distance = 2e5f + normalizedDistance * 8e5f; // 200k-1M distance
				float spiralTightness = 2f; // spiral tightness
				float angle = armAngleOffset + normalizedDistance * spiralTightness * 2f * MathF.PI;

				// Position calculation
				Vector3 position = new Vector3(
					MathF.Cos(angle) * distance,
					PseudoRandom.GetRandomNumber(-5e4f, 5e4f) * (1f - normalizedDistance * 0.7f), // thicker in the center
					MathF.Sin(angle) * distance
				);

				// Mass - decreases with distance
				float mass = 1e8f * (2f - normalizedDistance); // closer = higher mass

				// Color - based on spectral class (depends on distance and mass)
				Vector4 color;
				if (normalizedDistance < 0.3f)
				{
					// Inner region - hot blue giants
					color = new Vector4(0.3f + normalizedDistance, 0.5f + normalizedDistance * 0.5f, 1.0f, 1.0f);
				}
				else if (normalizedDistance < 0.7f)
				{
					// Middle region - yellow-orange stars
					color = new Vector4(1.0f, 0.8f - normalizedDistance * 0.3f, 0.2f + normalizedDistance * 0.3f, 1.0f);
				}
				else
				{
					// Outer region - red dwarfs
					color = new Vector4(1.0f, 0.3f - normalizedDistance * 0.2f, 0.1f, 1.0f);
				}

				// Keplerian speed calculation, but with small perturbation for chaos
				float keplerSpeed = MathF.Sqrt(G * centralMass / distance);
				float perturbation = 1.0f + PseudoRandom.GetRandomNumber(-0.15f, 0.15f); // ±15% disturbance
				keplerSpeed *= perturbation;

				// Tangential velocity in the direction of the spiral
				Vector3 tangentDirection = Vector3.Normalize(new Vector3(-MathF.Sin(angle), 0, MathF.Cos(angle)));

				// Radial velocity component - minimal perturbation
				Vector3 radialDirection = Vector3.Normalize(position);
				float radialSpeed = PseudoRandom.GetRandomNumber(-50f, 50f); // much smaller disturbance

				// Vertical oscillation + effect of central star's rotation
				float verticalSpeed = PseudoRandom.GetRandomNumber(-100f, 100f) * MathF.Sin(angle * 3f);
				float centralInfluence = 50f * MathF.Sin(angle * 2f) / (normalizedDistance + 0.1f); // greater effect on closer stars

				Vector3 velocity = tangentDirection * keplerSpeed +
								 radialDirection * radialSpeed +
								 Vector3.UnitY * (verticalSpeed + centralInfluence);

				_bodies[bodyIndex] = new Body(position, mass, velocity, color);
			}
		}
	}
}