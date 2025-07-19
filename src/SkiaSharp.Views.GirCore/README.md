# SkiaSharp.Views.GirCore

![SkiaSharp.Views.GirCore](/assets/nuget/SkiaSharp.Views.GirCore.svg)

**Use SkiaSharp views with GirCore on Linux using GTK4 and Cairo.**

This package provides a custom GTK4 `DrawingArea` widget (`SKDrawingArea`) that supports SkiaSharp rendering using GirCore bindings, bringing native Linux performance to SkiaSharp applications.

![Quick Start 1](/assets/QuickStart1.png)

## Features

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
dotnet add package SkiaSharp.Views.GirCore --version 3.119.0-preview.6
```

## Usage

Call after GirCore modules initialization:

```csharp
// Initialize GirCore modules first
Gtk.Module.Initialize();
Adw.Module.Initialize();
// ... other modules

// Initialize SkiaSharp.Views.GirCore
SkiaSharp.Views.GirCore.Module.Initialize();
```

## Examples

Complete examples are available in the samples folder:

- **[Quick Start 1](/samples/GraphicsTester.Skia.GirCore/Program.cs)** - Basic SkiaSharp usage with GirCore
- **[Graphics Tester](/samples/GraphicsTester.Skia.GirCore/Program.cs)** - Comprehensive SkiaSharp demos

## License

This project is licensed under the [MIT License](/LICENSE).
