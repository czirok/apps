using Gtk.MauiGraphicsSkia.GirCore;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System.Runtime.Versioning;

namespace Gomoku;

[UnsupportedOSPlatform("OSX")]
[UnsupportedOSPlatform("Windows")]
public partial class Board : GirCoreSkiaGraphicsView, IDisposable
{
	protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
	{
		switch (_boardModel.BoardTheme)
		{
			case BoardTheme.WoodWithStones:
				WoodWithStones(e);
				break;
			case BoardTheme.PaperAndPencil:
				PaperAndPencil(e);
				break;
		}
	}

	StoneCoordinate _stoneCoordinates = default!;

	public StoneCoordinate StoneCoordinate
	{
		get { return _stoneCoordinates; }
		set
		{
			var coordinates = _stoneCoordinates;
			if (coordinates != null && (value == null || coordinates.Row != value.Row || coordinates.Col != value.Col))
			{
				_stoneCoordinates = default!;
				InvalidateOnMainThread();
			}
			if (value != null && (coordinates == null || coordinates.Row != value.Row || coordinates.Col != value.Col) && Stones[value.Row, value.Col] == StoneColor.Empty)
			{
				_stoneCoordinates = value;
				InvalidateOnMainThread();
			}
		}
	}

}