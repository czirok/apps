using System.Numerics;

namespace NBody;

internal partial class Renderer
{
	public Vector3 Camera = new(0.0f, 0.0f, 1000.0f);
	public Vector3 Light = new(0.0f, 1000.0f, 0.0f);
	float _ideal;
	float _fieldOfView;

	public Renderer()
	{
		FieldOfView = 1000.0f;
	}

	public float FieldOfView
	{
		get => _fieldOfView;
		set => _ideal = Camera.Z * Camera.Z / (_fieldOfView = value);
	}


	public float ComputeScale(Vector3 vector)
	{
		return _ideal / Vector3.Distance(vector, Camera);
	}
}