using Gtk.LiveChartsCore.SkiaSharpView.GirCore;
using System.Runtime.Versioning;
using ViewModelsSamples.Maps.World;

namespace GirCoreSample.Maps.World;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public class View : Adw.Bin
{
	private readonly GeoMap geoMap;

	public View()
	{
		var viewModel = new ViewModel();

		geoMap = new GeoMap
		{
			Series = viewModel.Series,
			MapProjection = LiveChartsCore.Geo.MapProjection.Mercator,
		};

		Child = geoMap;
	}

	public override void Dispose()
	{
		geoMap.Dispose();
		base.Dispose();
	}
}
