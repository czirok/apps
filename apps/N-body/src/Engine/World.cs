using System.Numerics;
using EasyUIBinding.GirCore.Binding;

namespace NBody;

/// <summary>
/// Represents the world of the simulation. 
/// </summary>
public partial class World : NotifyPropertyModel
{
	/// <summary>
	/// The gravitational constant. 
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private float g = 67;

	/// <summary>
	/// The maximum speed. 
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private float c = 1e4f;

	/// <summary>
	/// The world instance. 
	/// </summary>
	public static World Instance => _instance ??= new World();

	private static World _instance = default!;

	/// <summary>
	/// The number of bodies allocated in the simulation. 
	/// </summary>
	public int BodyAllocationCount
	{
		get => _bodies.Length;
		set
		{
			if (_bodies.Length != value)
			{
				lock (_bodyLock)
					_bodies = new Body[value];
				Generate();
			}
		}
	}

	/// <summary>
	/// The number of bodies that exist in the simulation. 
	/// </summary>
	public int BodyCount => _tree == null ? 0 : _tree.BodyCount;

	/// <summary>
	/// The total mass of the bodies that exist in the simulation. 
	/// </summary>
	public float TotalMass => _tree == null ? 0 : _tree.Mass;

	/// <summary>
	/// The number of frames elapsed in the simulation. 
	/// </summary>
	public long Frames { get; private set; }

	/// <summary>
	/// Determines whether the simulation is active or paused. 
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private bool active;

	/// <summary>
	/// Determines whether to draw the tree structure for calculating forces. 
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private bool drawTree;

	/// <summary>
	/// Determines whether to draw tracers showing history of body locations. 
	/// </summary>
	public bool DrawTracers
	{
		get => Body.DrawTracers;
		set => Body.DrawTracers = value;
	}

	/// <summary>
	/// The title of the simulation system.
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private string systemTitle = nameof(SystemType.None);

	/// <summary>
	/// The type of the simulation system.
	/// </summary>
	[GirCoreNotify(ValidateValue = true)]
	private SystemType systemType = SystemType.None;

	public bool ShowStats { get; set; } = true;

	/// <summary>
	/// The collection of bodies in the simulation. 
	/// </summary>
	private Body[] _bodies = new Body[1000];

	/// <summary>
	/// The lock for modifying the bodies collection. 
	/// </summary>
	private readonly object _bodyLock = new();

	/// <summary>
	/// The tree for calculating forces. 
	/// </summary>
	private Octree _tree = default!;

	/// <summary>
	/// The Renderer instance for drawing 3D graphics. 
	/// </summary>
	private Renderer _renderer = new();

	/// <summary>
	/// Constructs the world and starts the simulation. 
	/// </summary>
	internal World()
	{
		// Initialize default values.
		Active = true;
		Frames = 0;
		_renderer.Camera.Z = _cameraZ;
		_renderer.FieldOfView = CameraFOV;
	}

	internal void Simulate()
	{
		if (Active)
			lock (_bodyLock)
			{
				// Update the bodies and determine the required tree width. 
				float halfWidth = 0;
				foreach (Body body in _bodies)
					if (body != null)
					{
						body.Update();
						halfWidth = Math.Max(Math.Abs(body.Location.X), halfWidth);
						halfWidth = Math.Max(Math.Abs(body.Location.Y), halfWidth);
						halfWidth = Math.Max(Math.Abs(body.Location.Z), halfWidth);
					}

				// Initialize the root tree and add the bodies. The root tree needs to be 
				// slightly larger than twice the determined half width. 
				_tree = new Octree(2.1f * halfWidth);
				foreach (Body body in _bodies)
					if (body != null)
						_tree.Add(body);

				// Accelerate the bodies in parallel. 
				Parallel.ForEach(_bodies, body =>
				{
					if (body != null)
						_tree.Accelerate(body);
				});

				// Update frame counter. 
				if (_tree.BodyCount > 0)
					Frames++;
			}

		// Update the camera. 
		_cameraZ += _cameraZVelocity * _cameraZ;
		_cameraZ = Math.Max(1, _cameraZ);
		_cameraZVelocity *= CameraZEasing;
		_renderer.Camera.Z = _cameraZ;
	}

	/// <summary>
	/// Rotates the world by calling the bodies' rotate methods. 
	/// </summary>
	/// <param name="point">The starting point for the axis of rotation.</param>
	/// <param name="direction">The direction for the axis of rotation</param>
	/// <param name="angle">The angle to rotate by.</param>
	public void Rotate(Vector3 point, Vector3 direction, float angle)
	{
		lock (_bodyLock)
			Parallel.ForEach(_bodies, body =>
			{
				body?.Rotate(point, direction, angle);
			});
	}
}
