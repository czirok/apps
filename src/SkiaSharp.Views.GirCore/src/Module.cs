using System.Runtime.InteropServices;

namespace SkiaSharp.Views.GirCore;

public static partial class Module
{
	[LibraryImport("libc", EntryPoint = "setlocale", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
	private static partial IntPtr Setlocale(int category, string locale);

	private const int LC_NUMERIC = 1;
	private static bool _localeFixed = false;
	private static readonly object _lockObject = new();

	/// <summary>
	/// Fixes LC_NUMERIC locale for SkiaSharp parsing functions.
	/// 
	/// Call after GirCore moules initialization, for example:
	/// 
	/// Gtk.Module.Initialize();
	/// Adw.Module.Initialize();
	/// ...
	/// SkiaSharp.Views.GirCore.Module.Initialize();
	/// 
	/// Call this once before using any SkiaSharp string-to-number parsing.
	/// </summary>
	public static void Initialize()
	{
		if (_localeFixed) return;

		lock (_lockObject)
		{
			if (!_localeFixed)
			{
				Setlocale(LC_NUMERIC, "C");
				_localeFixed = true;
			}
		}
	}
}