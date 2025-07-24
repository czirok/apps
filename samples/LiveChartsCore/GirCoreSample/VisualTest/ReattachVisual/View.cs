using EasyUIBinding.GirCore;
using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.VisualTest.ReattachVisual;

namespace GirCoreSample.VisualTest.ReattachVisual;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Gtk.Box, IDisposable
{
	private bool _isInVisualTree = true;
	private readonly CartesianChart cartesianChart;
	private readonly WrapPreferencesGroup wrapGroup;

	public View()
	{
		SetOrientation(Gtk.Orientation.Vertical);
		var viewModel = new ViewModel();

		wrapGroup = new WrapPreferencesGroup(
			[
				new Button("Toggle / Attach")
			.OnClick(ToggleAttach)
			]
		);

		cartesianChart = new CartesianChart
		{
			Series = viewModel.Series,
		};
		UpdateLayout();
	}

	void UpdateLayout()
	{
		RemoveAllChildren();
		Append(wrapGroup);
		if (_isInVisualTree)
		{
			Append(cartesianChart);
		}
	}

	private void RemoveAllChildren()
	{
		var child = GetFirstChild();
		while (child != null)
		{
			var next = child.GetNextSibling();
			Remove(child);
			child = next;
		}
	}

	private void ToggleAttach(object sender, System.EventArgs e)
	{
		_isInVisualTree = !_isInVisualTree;
		UpdateLayout();
	}

	public override void Dispose()
	{
		wrapGroup.Dispose();
		cartesianChart.Dispose();
		base.Dispose();
	}
}