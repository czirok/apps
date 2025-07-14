using System.Numerics;

namespace NBody;

public partial class World
{
	// Gemini improvised spectacular N-body system
	void SystemTypeCosmicBallet()
	{
		// Scaling factors – Values can be fine-tuned to achieve the desired visual effect.
		// Inspired by the "CollidingSystems" example, but with extreme dynamics.
		const float S = 5e5f;   // Position scale: Compressed into smaller space, increases interactions.
		const float V = 3e3f;   // Velocity scale: Higher initial velocities for more dynamic motion.
		const float M_STAR = 5e10f; // Central "star" mass
		const float M_PARTICLE = 1e7f; // "Dust" particle mass

		// Color palette: vibrant, contrasting colors
		Vector4[] vibrantColors = new Vector4[]
		{
			new Vector4(1.0f, 0.2f, 0.7f, 1.0f), // Magenta
            new Vector4(0.2f, 0.8f, 1.0f, 1.0f), // Cyan
            new Vector4(1.0f, 0.7f, 0.2f, 1.0f), // Orange
            new Vector4(0.7f, 1.0f, 0.2f, 1.0f), // Yellow-green
            new Vector4(0.5f, 0.2f, 1.0f, 1.0f)  // Purple
        };

		// Collision of two spiral galaxies, but with inward pulling force and chaotic dust

		// System 1: Spiral arms gathering towards the center
		Vector3 center1 = new Vector3(-S * 1.5f, 0, 0); // Left center point
		Vector3 initialVel1 = new Vector3(V * 0.8f, 0, 0); // Movement velocity to the right

		// Central "star" or mass concentration
		_bodies[0] = new Body(center1, M_STAR * 2, initialVel1, vibrantColors[0]); // Larger mass, vibrant magenta

		int particlesPerSystem = (_bodies.Length - 1) / 2; // Dynamically calculate the particles

		for (int i = 0; i < particlesPerSystem; i++)
		{
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float radius = PseudoRandom.GetRandomNumber(S * 0.5f, S * 1.2f); // Wider radius range
			float height = PseudoRandom.GetRandomNumber(-S * 0.1f, S * 0.1f); // Some Z-direction spread for 3D effect

			// Spiral arrangement (lighter spirals)
			Vector3 location = new Vector3(
				(float)Math.Cos(angle + radius * 0.00001f) * radius, // Radius-bound rotation
				height,
				(float)Math.Sin(angle + radius * 0.00001f) * radius
			) + center1;

			// Initial tangential velocity for spiral formation, plus collision system velocity
			float speed = (float)Math.Sqrt(G * M_STAR / radius) * 0.8f; // Slightly slower orbit, so it "collapses" better
			Vector3 velocityVector = Vector3.Normalize(Vector3.Cross(location - center1, Vector3.UnitY)) * speed + initialVel1;

			// Color encodes radius (or mass), with vibrant shades
			Vector4 particleColor = vibrantColors[1]; // Cyan
			if (i % 5 == 0) particleColor = vibrantColors[2]; // Sometimes orange
			if (i % 10 == 0) particleColor = vibrantColors[3]; // Sometimes yellow-green

			_bodies[i + 1] = new Body(location, M_PARTICLE, velocityVector, particleColor);
		}

		// System 2: Another spiral galaxy, also with inward pulling force
		Vector3 center2 = new Vector3(S * 1.5f, 0, 0); // Right center point
		Vector3 initialVel2 = new Vector3(-V * 0.8f, 0, 0); // Movement velocity to the left

		// Central "star" or mass concentration
		_bodies[particlesPerSystem + 1] = new Body(center2, M_STAR * 2, initialVel2, vibrantColors[4]); // Purple

		for (int i = 0; i < particlesPerSystem; i++)
		{
			float angle = PseudoRandom.GetRandomNumber((float)(Math.PI * 2));
			float radius = PseudoRandom.GetRandomNumber(S * 0.5f, S * 1.2f);
			float height = PseudoRandom.GetRandomNumber(-S * 0.1f, S * 0.1f);

			Vector3 location = new Vector3(
				(float)Math.Cos(angle + radius * -0.00001f) * radius, // Reversed spiral direction
				height,
				(float)Math.Sin(angle + radius * -0.00001f) * radius
			) + center2;

			float speed = (float)Math.Sqrt(G * M_STAR / radius) * 0.8f;
			Vector3 velocityVector = Vector3.Normalize(Vector3.Cross(location - center2, Vector3.UnitY)) * speed + initialVel2;

			Vector4 particleColor = vibrantColors[0]; // Magenta
			if (i % 5 == 0) particleColor = vibrantColors[1]; // Sometimes cyan
			if (i % 10 == 0) particleColor = vibrantColors[2]; // Sometimes orange

			_bodies[i + particlesPerSystem + 2] = new Body(location, M_PARTICLE, velocityVector, particleColor);
		}

		// Chaotic "interstellar dust" between the two systems (inspired by ThreeBody.cs)
		// These particles have smaller masses and are initially randomly positioned between the systems.
		// Fills the rest of the "Body" array, if there's space remaining.
		int dustParticlesStart = (particlesPerSystem * 2) + 1;
		for (int i = dustParticlesStart; i < _bodies.Length; i++)
		{
			// Placement in the band between the two galaxies
			float x = PseudoRandom.GetRandomNumber(-S * 1.0f, S * 1.0f);
			float y = PseudoRandom.GetRandomNumber(-S * 1.0f, S * 1.0f);
			float z = PseudoRandom.GetRandomNumber(-S * 1.0f, S * 1.0f);
			var loc = new Vector3(x, y, z);
			float m = PseudoRandom.GetRandomNumber(M_PARTICLE * 0.01f, M_PARTICLE * 0.5f); // Smaller mass dust

			// Initial velocity can be zero, or very small, so it "falls into" the systems
			Vector3 dustVel = new Vector3(
				PseudoRandom.GetRandomNumber(-V * 0.1f, V * 0.1f),
				PseudoRandom.GetRandomNumber(-V * 0.1f, V * 0.1f),
				PseudoRandom.GetRandomNumber(-V * 0.1f, V * 0.1f)
			);

			// Color adapted to velocity/mass, with transitional shades
			Vector4 color = new Vector4(
				MathF.Abs(MathF.Sin(loc.X * 0.000001f + m * 0.0000001f)), // Red component
				MathF.Abs(MathF.Cos(loc.Y * 0.000001f + m * 0.0000002f)), // Green component
				MathF.Abs(MathF.Sin(loc.Z * 0.000001f + m * 0.0000003f)), // Blue component
				0.6f // More transparent dust
			);

			_bodies[i] = new Body(loc, m, dustVel, color);
		}
	}
}