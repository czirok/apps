using System.Numerics;

namespace NBody;

/// <summary>
/// Generated with Grok.
/// </summary>
public partial class World
{
	void SystemTypeSpiralGalaxy()
	{
		// Parameters
		const float S = 1.5e6f;   // Position scale
		const float V = 1.2e3f;   // Velocity scale
		const float centralMass = 1.5e10f; // Mass of central stars
		const float planetMass = 1e6f;    // Mass of planets
		const float dustMass = 1e4f;      // Mass of dust particles
		int centralStars = 3;              // Three central stars
		int planetsPerStar = (_bodies.Length - centralStars) / 3 / 2; // Number of planets per star
		int dustParticles = _bodies.Length - centralStars - planetsPerStar * centralStars; // Number of dust particles

		// Colors for stars (vivid, high contrast)
		Vector4[] starColors = new[]
		{
			new Vector4(1.0f, 0.3f, 0.3f, 1.0f), // Red star
            new Vector4(0.3f, 0.8f, 1.0f, 1.0f), // Blue star
            new Vector4(1.0f, 1.0f, 0.4f, 1.0f)  // Yellow star
        };

		// Colors for planets and dust particles (paler shades)
		Vector4[] planetColors = new[]
		{
			new Vector4(1.0f, 0.5f, 0.5f, 0.7f), // Pale red
            new Vector4(0.5f, 0.7f, 1.0f, 0.7f), // Pale blue
            new Vector4(1.0f, 1.0f, 0.6f, 0.7f)  // Pale yellow
        };

		// Place three stars at equal distances from the center, in a triangle formation
		for (int i = 0; i < centralStars && i < _bodies.Length; i++)
		{
			float angle = i * 2 * MathF.PI / centralStars;
			Vector3 location = new Vector3(MathF.Cos(angle) * S * 0.5f, 0, MathF.Sin(angle) * S * 0.5f);
			// The velocity of the stars is set so they slowly rotate around the center
			Vector3 velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * V * 0.5f;
			_bodies[i] = new Body(location, centralMass, velocity, starColors[i]);
		}

		// Place planets around each star
		int bodyIndex = centralStars;
		for (int star = 0; star < centralStars && bodyIndex < _bodies.Length; star++)
		{
			Vector3 starLocation = _bodies[star].Location;
			for (int i = 0; i < planetsPerStar && bodyIndex < _bodies.Length; i++)
			{
				float angle = PseudoRandom.GetRandomNumber(2 * MathF.PI);
				float radius = PseudoRandom.GetRandomNumber(1e5f, 3e5f) + _bodies[star].Radius;
				Vector3 location = new Vector3(MathF.Cos(angle) * radius, PseudoRandom.GetRandomNumber(-2e4f, 2e4f), MathF.Sin(angle) * radius) + starLocation;
				float speed = MathF.Sqrt(centralMass * G / radius);
				Vector3 velocity = Vector3.Normalize(Vector3.Cross(location - starLocation, Vector3.UnitY)) * speed + _bodies[star].Velocity;

				_bodies[bodyIndex] = new Body(location, planetMass, velocity, planetColors[star]);
				bodyIndex++;
			}
		}

		// Add dust particles for spiral effect
		for (; bodyIndex < _bodies.Length; bodyIndex++)
		{
			// Place dust particles in a spiral around the center of the galaxy
			float angle = PseudoRandom.GetRandomNumber(2 * MathF.PI);
			float radius = PseudoRandom.GetRandomNumber(2e5f, 1e6f);
			Vector3 location = new Vector3(MathF.Cos(angle) * radius, PseudoRandom.GetRandomNumber(-5e4f, 5e4f), MathF.Sin(angle) * radius);
			float speed = MathF.Sqrt(centralMass * G / (radius * 2)); // Slower spiral motion
			Vector3 velocity = Vector3.Normalize(Vector3.Cross(location, Vector3.UnitY)) * speed;

			// Gradient color for dust particles based on distance
			float colorFactor = radius / 1e6f;
			Vector4 dustColor = new Vector4(
				MathF.Abs(MathF.Sin(colorFactor)),
				MathF.Abs(MathF.Cos(colorFactor * 0.5f)),
				MathF.Abs(MathF.Sin(colorFactor * 0.3f)),
				0.6f
			);

			_bodies[bodyIndex] = new Body(location, dustMass, velocity, dustColor);
		}
	}
}