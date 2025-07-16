# Apps

## Nuget Packages

|Nuget 🔗|Readme 📄||
|:---|:---|:---|
|[![EasyUIBinding.GirCore](/assets/nuget/EasyUIBinding.GirCore.64.png)](https://www.nuget.org/packages/EasyUIBinding.GirCore/)|[EasyUIBinding.GirCore](/src/EasyUIBinding.GirCore/README.md)|Declarative UI binding for GTK4/Libadwaita|
|[![SkiaSharp.Views.GirCore](/assets/nuget/SkiaSharp.Views.GirCore.64.png)](https://www.nuget.org/packages/SkiaSharp.Views.GirCore/)|[SkiaSharp.Views.GirCore](/src/SkiaSharp.Views.GirCore/README.md)|SkiaSharp views on Linux with GTK4|
|[![Gtk.MauiGraphicsSkia.GirCore](/assets/nuget/Gtk.MauiGraphicsSkia.GirCore.64.png)](https://www.nuget.org/packages/Gtk.MauiGraphicsSkia.GirCore/)|[Gtk.MauiGraphicsSkia.GirCore](/src/Gtk.MauiGraphicsSkia.GirCore/README.md)|Microsoft.Maui.Graphics with SkiaSharp|
|[![WebKit.BlazorWebView.GirCore](/assets/nuget/WebKit.BlazorWebView.GirCore.64.png)](https://www.nuget.org/packages/WebKit.BlazorWebView.GirCore/)|[WebKit.BlazorWebView.GirCore](/src/WebKit.BlazorWebView.GirCore/README.md)|Blazor desktop apps with WebKit|
|[![Yaml.Localization](/assets/nuget/Yaml.Localization.64.png)](https://www.nuget.org/packages/Yaml.Localization/)|[Yaml.Localization](/src/Yaml.Localization/README.md)|Localization for .NET apps using YAML files|

## Applications

### N-body

[N-body simulation](/apps/N-body/README.md) built with GirCore and Maui Graphics. Packages are available for [Arch](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-1-x86_64.pkg.tar.zst), [DEB](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody_1.2.1_amd64.deb), [RPM](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-1.x86_64.rpm), [Flatpak](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-x86_64.flatpak), and as a [portable tar.gz](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/nbody-1.2.1-x86_64.tar.gz).

![N-body simulation](/apps/N-body/n-body.gif)

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
4561024 bytes 4.4M EasyUIBinding
6857808 bytes 6.6M GirCoreApp
5257888 bytes 5.1M GraphicsTester.Skia.GirCore
9030712 bytes 8.7M libSkiaSharp.so
3393616 bytes 3.3M QuickStart1
```

- Instant startup - no JIT compilation
- Self-contained - no .NET runtime required
- Small footprint - optimized native code
