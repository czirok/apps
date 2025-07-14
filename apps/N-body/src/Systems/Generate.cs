using System.Numerics;

namespace NBody;

public partial class World
{
	/// <summary>
	/// Generates the specified gravitational system. 
	/// </summary>
	/// <param name="type">The system type to generate.</param>
	public void Generate(SystemType? type = null, string? title = null)
	{
		if (title != null)
			SystemTitle = title;
		if (type != null)
			SystemType = type.Value;

		// Reset frames elapsed. 
		Frames = 0;

		lock (_bodyLock)
		{
			Array.Clear(_bodies, 0, _bodies.Length);

			switch (SystemType)
			{
				case SystemType.None:
					ResetCamera();
					SystemTypeNone();
					break;
				case SystemType.SlowParticles:
					ResetCamera();
					SystemTypeSlowParticles();
					break;
				case SystemType.FastParticles:
					ResetCamera();
					SystemTypeFastParticles();
					break;
				case SystemType.MassiveBody:
					ResetCamera();
					SystemTypeMassiveBody();
					break;
				case SystemType.OrbitalSystem:
					ResetCamera();
					SystemTypeOrbitalSystem();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(60.07f, -81.26f, 0.0f)), 0.5052f);
					MoveCamera(-150);
					break;
				case SystemType.BinarySystem:
					ResetCamera();
					SystemTypeBinarySystem();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(67.42f, 113.72f, 0.0f)), 0.6610f);
					MoveCamera(-300);
					break;
				case SystemType.PlanetarySystem:
					ResetCamera();
					SystemTypePlanetarySystem();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(58.13f, 25.31f, 0.0f)), 0.3170f);
					MoveCamera(-400);
					break;
				case SystemType.PlanetarySystemColor:
					ResetCamera();
					SystemTypePlanetarySystemColor();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(58.13f, 25.31f, 0.0f)), 0.3170f);
					MoveCamera(-400);
					break;
				case SystemType.DistributionTest:
					ResetCamera();
					SystemTypeDistributionTest();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(143.91f, -134.78f, 0.0f)), 0.9859f);
					break;
				case SystemType.ThreeBody:
					ResetCamera();
					SystemTypeThreeBody();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(11.29f, -209.95f, 0.0f)), 1.0513f);
					MoveCamera(-300);
					break;
				case SystemType.CollidingSystems:
					ResetCamera();
					SystemTypeCollidingSystems();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(368.77f, 202.9f, 0.0f)), 2.1045f);
					break;
				case SystemType.QuantumAurora:
					ResetCamera();
					SystemTypeQuantumAurora();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(0.7f, 1.2f, 0.4f)), 1.8f);
					MoveCamera(-250);
					break;
				case SystemType.GalacticDance:
					ResetCamera();
					SystemTypeGalacticDance();
					MoveCamera(-400);
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(61.45f, 203.23f, 0.0f)), 1.0616f);
					break;
				case SystemType.GalacticSpiralChaos:
					ResetCamera();
					SystemTypeGalacticSpiralChaos();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(43.66f, -48.9f, 0.0f)), 0.3278f);
					MoveCamera(-400);
					break;
				case SystemType.SpiralGalaxy:
					ResetCamera();
					SystemTypeSpiralGalaxy();
					MoveCamera(-300);
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(86.48f, 127.33f, 0.0f)), 0.7696f);
					break;
				case SystemType.FractalChaosAttractor:
					ResetCamera();
					SystemTypeFractalChaosAttractor();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(0.7f, 1.2f, 0.4f)), 1.8f);
					MoveCamera(-250);
					break;
				case SystemType.AuroraHarmonia:
					ResetCamera();
					SystemTypeAuroraHarmonia();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(0.7f, 1.2f, 0.4f)), 1.8f);
					MoveCamera(-250);
					break;
				case SystemType.CosmicBallet:
					ResetCamera();
					SystemTypeCosmicBallet();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(0.7f, 1.2f, 0.4f)), 1.8f);
					MoveCamera(-250);
					break;
				case SystemType.SupernovaRemnants:
					ResetCamera();
					SystemTypeSupernovaRemnants();
					Rotate(Vector3.Zero, Vector3.Normalize(new Vector3(0.7f, 1.2f, 0.4f)), 1.8f);
					MoveCamera(-250);
					break;
			}
		}
	}
}