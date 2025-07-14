using System.Numerics;

namespace NBody;

/// <summary>
/// Represents a massive body in the simulation. 
/// </summary>
partial class Body
{
	/// <summary>
	/// The spatial location of the body. 
	/// </summary>
	public Vector3 Location = Vector3.Zero;

	/// <summary>
	/// The velocity of the body. 
	/// </summary>
	public Vector3 Velocity = Vector3.Zero;

	/// <summary>
	/// The acceleration accumulated for the body during a single simulation 
	/// step. 
	/// </summary>
	public Vector3 Acceleration = Vector3.Zero;

	/// <summary>
	/// The mass of the body. 
	/// </summary>
	public float Mass;

	/// <summary>
	/// The previous locations of the body in a circular queue. 
	/// </summary>
	private readonly Vector3[] _locationHistory;

	/// <summary>
	/// The current index in the location history queue. 
	/// </summary>
	private int _locationHistoryIndex = 0;

	/// <summary>
	/// The radius of the body. 
	/// </summary>
	public float Radius => GetRadius(Mass);

	public Body(
		Vector3 location = default,
		float mass = 1e6f,
		Vector3 velocity = default,
		Vector4? color = null
	)
	{
		_locationHistory = new Vector3[20];

		Mass = mass;
		Location = location;
		Velocity = velocity;
		Color = color ?? Vector4.One;

		// Location history inicializálása
		for (int i = 0; i < _locationHistory.Length; i++)
		{
			_locationHistory[i] = location;
		}
	}

	/// <summary>
	/// Updates the properties of the body such as location, velocity, and 
	/// applied acceleration. This method should be invoked at each time step. 
	/// </summary>
	public void Update()
	{
		_locationHistory[_locationHistoryIndex] = Location;
		_locationHistoryIndex = ++_locationHistoryIndex % _locationHistory.Length;

		float speed = Velocity.Length();
		if (speed > World.Instance.C)
		{
			Velocity = World.Instance.C * Vector3.Normalize(Velocity);
			speed = World.Instance.C;
		}

		if (speed == 0)
			Velocity += Acceleration;
		else
		{
			// Apply relativistic velocity addition. 
			Vector3 parallelAcc = Acceleration.Projection(Velocity);
			Vector3 orthogonalAcc = Acceleration.Rejection(Velocity);
			float alpha = (float)Math.Sqrt(1 - Math.Pow(speed / World.Instance.C, 2));
			Velocity = (Velocity + parallelAcc + alpha * orthogonalAcc) / (1 + Vector3.Dot(Velocity, Acceleration) / (World.Instance.C * World.Instance.C));
		}

		Location += Velocity;
		Acceleration = Vector3.Zero;
	}

	/// <summary>
	/// Rotates the body along an arbitrary axis. 
	/// </summary>
	/// <param name="point">The starting point for the axis of rotation.</param>
	/// <param name="direction">The direction for the axis of rotation</param>
	/// <param name="angle">The angle to rotate by.</param>
	public void Rotate(Vector3 point, Vector3 direction, float angle)
	{
		Location = Location.Rotate(point, direction, angle);

		// To rotate velocity and acceleration we have to adjust for the starting 
		// point for the axis of rotation. This way the vectors are effectively 
		// rotated about their own starting points. 
		Velocity += point;
		Velocity = Velocity.Rotate(point, direction, angle);
		Velocity -= point;
		Acceleration += point;
		Acceleration = Acceleration.Rotate(point, direction, angle);
		Acceleration -= point;

		if (DrawTracers)
		{
			for (int i = 0; i < _locationHistory.Length; i++)
			{
				_locationHistory[i] = _locationHistory[i].Rotate(point, direction, angle);
			}
		}
	}

	/// <summary>
	/// Returns the radius defined for the given mass value. 
	/// </summary>
	/// <param name="mass">The mass to calculate a radius for.</param>
	/// <returns>The radius defined for the given mass value.</returns>
	public static float GetRadius(float mass)
	{
		// We assume all bodies have the same density so volume is directly 
		// proportion to mass. Then we use the inverse of the equation for the 
		// volume of a sphere to solve for the radius. The end result is arbitrarily 
		// scaled and added to a constant so the Body is generally visible. 
		return 10f * (float)Math.Pow(3 * mass / (4 * Math.PI), 1 / 3.0) + 10f;
	}

	Vector4 _color = Vector4.One;
	/// <summary>
	/// Default white color for the body.
	/// </summary>
	public Vector4 Color
	{
		get => _color;
		set
		{
			_color = value;
			if (_drawingBrush != null) // Csak akkor frissít, ha az ecset már létezik
				_drawingBrush.Color = _color.ToSKColor();
		}
	}

	/// <summary>
	/// Determines whether to draw tracers showing history of body locations. 
	/// </summary>
	public static bool DrawTracers { get; set; } = false;
}
