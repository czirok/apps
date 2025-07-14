# Gtk.MauiGraphicsSkia.GirCore

![Gtk.MauiGraphicsSkia.GirCore](/assets/nuget/Gtk.MauiGraphicsSkia.GirCore.svg)

**Use Microsoft.Maui.Graphics with SkiaSharp and GirCore on Linux using GTK4 and Cairo.**

This package provides a `GraphicsView` implementation (`GirCoreSkiaGraphicsView`) that integrates `Microsoft.Maui.Graphics` with SkiaSharp rendering on Linux, offering a familiar cross-platform drawing API with native performance.

![Graphics Tester](/assets/GraphicsTester.png)

## Features

- **Microsoft.Maui.Graphics API** - Use familiar `IDrawable` and `ICanvas` interfaces
- **Linux-native rendering** (GTK4 + Cairo)
- **High-performance graphics** with hardware acceleration
- **100% Compatible with SkiaSharp.Views.Desktop** patterns
- **Support** Linux x64 and ARM64

## Requirements

- .NET 8.0 or later
- Linux x64 and ARM64
- GNOME 48+

## Installation

Install the package via NuGet:

```bash
dotnet add package Gtk.MauiGraphicsSkia.GirCore
```

## Examples

Complete examples are available in the samples folder:

- **[Quick Start 1](/samples/GraphicsTester.Skia.GirCore/Program.cs)** - Basic SkiaSharp usage with GirCore
- **[Graphics Tester](/samples/GraphicsTester.Skia.GirCore/Program.cs)** - Comprehensive SkiaSharp demos

More advanced application found in the apps folder:

- **[N-body simulation](/apps/N-body/README.md)** - Physics visualization with real-time rendering

## License

This project is licensed under the [MIT License](/LICENSE).
