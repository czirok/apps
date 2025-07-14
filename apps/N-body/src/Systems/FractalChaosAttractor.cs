using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Claude.
/// </summary>
public partial class World
{
	void SystemTypeFractalChaosAttractor()
	{
		// "Gravitational chaos" based on the Lorenz Attractor - in 4 dimensions
		// But not mathematical, with physical bodies!

		// "Chaos Seeds" - invisible attractor points in phase space
		Vector3[] attractorPoints = new Vector3[]
		{
			new Vector3(-3e5f, -2e5f, 1e5f),   // Lorenz left wing
            new Vector3(+3e5f, +2e5f, 1e5f),   // Lorenz right wing  
            new Vector3(0f, 0f, -4e5f),        // Lower attractor
            new Vector3(0f, 4e5f, 2e5f),       // Upper attractor
            new Vector3(-2e5f, 0f, 0f),        // Left side
            new Vector3(+2e5f, 0f, 0f),        // Right side
        };

		// Invisible "virtual bodies" for the attractor points - these do not appear
		for (int i = 0; i < attractorPoints.Length && i < _bodies.Length; i++)
		{
			_bodies[i] = new Body(
				location: attractorPoints[i],
				mass: 8e11f, // Huge mass, but invisible
				velocity: Vector3.Zero,
				color: new Vector4(0, 0, 0, 0) // Transparent = invisible
			);
		}

		// "Chaos particles" - these are visible and move chaotically
		int particleStartIndex = attractorPoints.Length;
		for (int i = particleStartIndex; i < _bodies.Length; i++)
		{
			// Initial position: small sphere around the center
			float startRadius = PseudoRandom.GetRandomNumber(5e4f, 8e4f);
			float theta = PseudoRandom.GetRandomNumber((float)(2 * Math.PI));
			float phi = PseudoRandom.GetRandomNumber((float)(Math.PI));

			Vector3 location = new Vector3(
				startRadius * MathF.Sin(phi) * MathF.Cos(theta),
				startRadius * MathF.Sin(phi) * MathF.Sin(theta),
				startRadius * MathF.Cos(phi)
			);

			// Mass - affects color and behavior
			float mass = PseudoRandom.GetRandomNumber(1e5f, 5e6f);

			// Color gradient: low mass = cold colors, high mass = warm colors
			float normalizedMass = (mass - 1e5f) / (5e6f - 1e5f); // 0-1 between
			Vector4 color;

			if (normalizedMass < 0.33f)
			{
				// Blue-cyan spectrum (cold)
				float t = normalizedMass / 0.33f;
				color = new Vector4(0.1f + t * 0.3f, 0.3f + t * 0.7f, 1.0f, 1.0f);
			}
			else if (normalizedMass < 0.66f)
			{
				// Green-yellow spectrum (medium)
				float t = (normalizedMass - 0.33f) / 0.33f;
				color = new Vector4(0.4f + t * 0.6f, 1.0f, 1.0f - t * 0.8f, 1.0f);
			}
			else
			{
				// Orange-red spectrum (hot)
				float t = (normalizedMass - 0.66f) / 0.34f;
				color = new Vector4(1.0f, 1.0f - t * 0.5f, 0.2f - t * 0.2f, 1.0f);
			}

			// Initial velocity: small random kick + "attractor magnet" effect
			Vector3 randomKick = new Vector3(
				PseudoRandom.GetRandomNumber(-100f, 100f),
				PseudoRandom.GetRandomNumber(-100f, 100f),
				PseudoRandom.GetRandomNumber(-100f, 100f)
			);

			// Direction towards the nearest attractor (weak)
			Vector3 nearestAttractor = attractorPoints[0];
			float minDist = Vector3.Distance(location, nearestAttractor);
			foreach (var attractor in attractorPoints)
			{
				float dist = Vector3.Distance(location, attractor);
				if (dist < minDist)
				{
					minDist = dist;
					nearestAttractor = attractor;
				}
			}

			Vector3 attractorPull = Vector3.Normalize(nearestAttractor - location) * 50f;
			Vector3 initialVelocity = randomKick + attractorPull;

			_bodies[i] = new Body(location, mass, initialVelocity, color);
		}
	}
}