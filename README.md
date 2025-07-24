# Apps

## Nuget Packages

|Nuget 🔗|Readme 📄||
|:---|:---|:---|
|[![EasyUIBinding.GirCore](/assets/nuget/EasyUIBinding.GirCore.64.png)](https://www.nuget.org/packages/EasyUIBinding.GirCore/)|[EasyUIBinding.GirCore](/src/EasyUIBinding.GirCore/README.md)|Declarative UI binding for GTK4/Libadwaita|
|[![Gtk.LiveChartsCore.SkiaSharpView.GirCore](/assets/nuget/Gtk.LiveChartsCore.SkiaSharpView.GirCore.64.png)](https://www.nuget.org/packages/Gtk.LiveChartsCore.SkiaSharpView.GirCore/)|[Gtk.LiveChartsCore.SkiaSharpView.GirCore](/src/Gtk.LiveChartsCore.SkiaSharpView.GirCore/README.md)|LiveCharts2 for GTK with SkiaSharp|
|[![SkiaSharp.Views.GirCore](/assets/nuget/SkiaSharp.Views.GirCore.64.png)](https://www.nuget.org/packages/SkiaSharp.Views.GirCore/)|[SkiaSharp.Views.GirCore](/src/SkiaSharp.Views.GirCore/README.md)|SkiaSharp views on Linux with GTK4|
|[![Gtk.MauiGraphicsSkia.GirCore](/assets/nuget/Gtk.MauiGraphicsSkia.GirCore.64.png)](https://www.nuget.org/packages/Gtk.MauiGraphicsSkia.GirCore/)|[Gtk.MauiGraphicsSkia.GirCore](/src/Gtk.MauiGraphicsSkia.GirCore/README.md)|Microsoft.Maui.Graphics with SkiaSharp|
|[![WebKit.BlazorWebView.GirCore](/assets/nuget/WebKit.BlazorWebView.GirCore.64.png)](https://www.nuget.org/packages/WebKit.BlazorWebView.GirCore/)|[WebKit.BlazorWebView.GirCore](/src/WebKit.BlazorWebView.GirCore/README.md)|Blazor desktop apps with WebKit|
|[![Yaml.Localization](/assets/nuget/Yaml.Localization.64.png)](https://www.nuget.org/packages/Yaml.Localization/)|[Yaml.Localization](/src/Yaml.Localization/README.md)|Localization for .NET apps using YAML files|

## Applications

### N-body

[N-body simulation](/apps/N-body/README.md) built with GirCore and Maui Graphics. Packages are available for [Arch](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-1-x86_64.pkg.tar.zst), [DEB](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody_1.2.1_amd64.deb), [RPM](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-1.x86_64.rpm), [Flatpak](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-x86_64.flatpak), and as a [portable tar.gz](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-x86_64.tar.gz).

![N-body simulation](/apps/N-body/n-body.gif)

## Games

### Pela Gomoku

[Pela Gomoku](/apps/Gomoku/README.md) - built with GirCore and Maui Graphics. Packages are available for [Arch](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-1-x86_64.pkg.tar.zst), [DEB](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku_1.0.0_amd64.deb), [RPM](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-1.x86_64.rpm), [Flatpak](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-x86_64.flatpak), and as a [portable tar.gz](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-x86_64.tar.gz).

![Pela Gomoku](/apps/Gomoku/gomoku.png)

## Samples

See the [samples](/samples/README.md) directory for various examples of using the libraries.

## GirCore + AOT = Blazing Fast

All samples compile to **native executables** with .NET AOT:

```bash
cd samples && chmod +x ./aot.sh && ./aot.sh
```

### Impressive result

```bash
 6039504 bytes 5,8M EasyUIBinding
 8645760 bytes 8,3M GirCoreApp
 5373456 bytes 5,2M GraphicsTester.Skia.GirCore
 2471408 bytes 2,4M libHarfBuzzSharp.so
 9030712 bytes 8,7M libSkiaSharp.so
10176640 bytes 9,8M LiveChartsCore
 3402128 bytes 3,3M QuickStart1
```

- Instant startup - no JIT compilation
- Self-contained - no .NET runtime required
- Small footprint - optimized native code

## Changelog

- [v2025.07.14-nuget](.github/releases/v2025.07.14-nuget.md)
- [v2025.07.14-apps](.github/releases/v2025.07.14-apps.md)
- [v2025.07.16-apps](.github/releases/v2025.07.16-apps.md)
- [v2025.07.20-nuget](.github/releases/v2025.07.20-nuget.md)
- [v2025.07.24-nuget](.github/releases/v2025.07.24-nuget.md)
- [v2025.07.24-apps](.github/releases/v2025.07.24-apps.md)
