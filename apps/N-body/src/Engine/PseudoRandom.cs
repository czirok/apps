using System.Numerics;

namespace NBody;

internal static class PseudoRandom
{
	public static readonly Random Random = new();

	public static float GetRandomNumber(float a, float b = 0)
	{
		if (a > b)
			(a, b) = (b, a);

		return (float)(Random.NextDouble() * (b - a) + a);
	}

	public static Vector3 Vector3(float maximumMagnitude = 1)
	{
		return GetRandomNumber(maximumMagnitude) * DirectionVector();
	}

	public static Vector3 DirectionVector(float magnitude = 1.0f)
	{
		Vector3 vector;
		do
		{
			vector = new Vector3(
				GetRandomNumber(-1, 1),
				GetRandomNumber(-1, 1),
				GetRandomNumber(-1, 1)
				);
		} while (vector.Length().Equals(0.0f));

		return magnitude / vector.Length() * vector;
	}
}