using System.Numerics;

namespace NBody;

public partial class World
{
	void SystemTypeDistributionTest()
	{
		float distance = 4e4f;
		float mass = 5e6f;

		int side = (int)Math.Pow(_bodies.Length, 1.0 / 3);
		int k = 0;
		for (int a = 0; a < side; a++)
			for (int b = 0; b < side; b++)
				for (int c = 0; c < side; c++)
					_bodies[k++] = new Body(distance * new Vector3(a - side / 2, b - side / 2, c - side / 2), mass);
	}
}