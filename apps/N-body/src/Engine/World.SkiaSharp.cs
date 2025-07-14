using SkiaSharp;
using System.Numerics;

namespace NBody;

public partial class World
{
	public void Draw(SKCanvas g)
	{
		// Collect only non-null bodies and sort them by distance
		var visibleBodies = _bodies
			.Where(body => body != null)
			.OrderByDescending(body => Vector3.Distance(body.Location, _renderer.Camera))
			.ToArray();

		// Draw based on distance
		foreach (Body body in visibleBodies)
		{
			body.Draw(g, _renderer);
		}

		if (DrawTree)
			_tree.Draw(g, _renderer);
	}
}