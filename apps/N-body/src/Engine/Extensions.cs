using SkiaSharp;
using System.Numerics;

namespace NBody;

public static class Extensions
{
	public static Vector3 Rotate(this Vector3 vector, Vector3 point, Vector3 direction, float angle)
	{
		var num1 = 1.0f / direction.Length();
		direction.X *= num1;
		direction.Y *= num1;
		direction.Z *= num1;
		var num2 = (float)Math.Cos(angle);
		var num3 = (float)Math.Sin(angle);
		return new Vector3(
			(point.X * (direction.Y * direction.Y) - direction.X * (point.Y * direction.Y - direction.X * vector.X - direction.Y * vector.Y)) * (1.0f - num2) + vector.X * num2 + (-point.Z * direction.Y + direction.Y * vector.Z) * num3,

			(point.Y * (direction.X * direction.X) - direction.Y * (point.X * direction.X - direction.X * vector.X - direction.Y * vector.Y)) * (1.0f - num2) + vector.Y * num2 + (point.Z * direction.X - direction.X * vector.Z) * num3,

			point.Z * (direction.X * direction.X + direction.Y * direction.Y) * (1.0f - num2) + vector.Z * num2 + (-point.Y * direction.X + point.X * direction.Y - direction.Y * vector.X + direction.X * vector.Y) * num3
		);
	}

	public static Vector3 Average(this ICollection<Vector3> vectors)
	{
		return vectors.Aggregate(new Vector3(), (current, vector2) => current + vector2) / vectors.Count;
	}

	public static double Angle(this Vector3 a, Vector3 b)
	{
		return Math.Acos(Vector3.Dot(a, b) / (a.Length() * b.Length()));
	}

	public static Vector3 Projection(this Vector3 a, Vector3 b)
	{
		return Vector3.Dot(a, b) / Vector3.Dot(b, b) * b;
	}

	public static Vector3 Rejection(this Vector3 a, Vector3 b)
	{
		return a - a.Projection(b);
	}

	public static double Magnitude(this Vector3 a)
	{
		return Math.Sqrt(
			a.X * a.X +
			a.Y * a.Y +
			a.Z * a.Z
		);
	}

	public static SKColor ToSKColor(this Vector4 color)
	{
		return new SKColor(
			(byte)(color.X * 255),
			(byte)(color.Y * 255),
			(byte)(color.Z * 255),
			(byte)(color.W * 255)
		);
	}
}