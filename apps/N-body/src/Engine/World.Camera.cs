namespace NBody;

/// <summary>
/// Represents the world of the simulation. 
/// </summary>
public partial class World
{
	/// <summary>
	/// The camera field of view. 
	/// </summary>
	private const float CameraFOV = 9e8f;

	/// <summary>
	/// The default value for the camera's position on the z-axis. 
	/// </summary>
	private const float CameraZDefault = 1e6f;

	/// <summary>
	/// The acceleration constant for camera scrolling. 
	/// </summary>
	private const float CameraZAcceleration = -2e-4f;

	/// <summary>
	/// The easing factor for camera scrolling. 
	/// </summary>
	private const float CameraZEasing = 0.94f;

	/// <summary>
	/// The camera's position on the z-axis. 
	/// </summary>
	private float _cameraZ = CameraZDefault;

	/// <summary>
	/// The camera's velocity along the z-axis. 
	/// </summary>
	private float _cameraZVelocity = 0;

	/// <summary>
	/// The delta for camera scrolling, used to adjust the camera's position.
	/// </summary>
	private int _cameraDelta = 0;

	/// <summary>
	/// Moves the camera in association with the given mouse wheel delta. 
	/// </summary>
	/// <param name="delta">The signed number of dents the mouse wheel moved.</param>
	public void MoveCamera(int delta)
	{
		_cameraDelta += delta;
		_cameraZVelocity += delta * CameraZAcceleration;
	}

	/// <summary>
	/// Resets the camera to its initial position. 
	/// </summary>
	public void ResetCamera()
	{
		_cameraZ = CameraZDefault;
		_cameraZVelocity = 0;
	}
}
