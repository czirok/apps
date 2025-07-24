# Gtk.LiveChartsCore.SkiaSharpView.GirCore - LiveCharts2 for GTK

![LiveCharts2 GTK](/assets/nuget/Gtk.LiveChartsCore.SkiaSharpView.GirCore.svg)

## Overview

**Gtk.LiveChartsCore.SkiaSharpView.GirCore** brings the powerful [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2) charting library to GTK applications using GirCore. Create beautiful, interactive charts with the same API you know from WPF, MAUI, and other platforms.

![LiveChart2](/assets/Gtk.LiveChartsCore.SkiaSharpView.GirCore.gif)

## Features

- **High Performance** - Hardware accelerated with SkiaSharp
- **Rich Chart Types** - Line, Column, Pie, Scatter, Polar, Geographic maps
- **Cross-Platform** - Same API across WPF, MAUI, Avalonia, and now GTK
- **Customizable** - Themes, animations, legends, tooltips
- **Responsive** - Automatic layout and scaling
- **Real-time** - Live data updates and smooth animations

## Requirements

- .NET 8.0 or later
- GirCore
- [LiveCharts2](https://github.com/beto-rodriguez/LiveCharts2) core packages

## Installation

```bash
dotnet add package Gtk.LiveChartsCore.SkiaSharpView.GirCore --version 2.0.0-rc5.4
```

## Quick Start

Check out the [Live Charts samples](/samples/LiveChartsCore/) to see 104 Adwaita examples of various chart types and configurations in action.

**Official Live Charts Documentation**: [livecharts.dev](https://livecharts.dev/)

## Data Binding Integration

Works seamlessly with [EasyUIBinding.GirCore](/src/EasyUIBinding.GirCore/README.md):

```csharp
var cartesianChart = new CartesianChart
{
    Series = viewModel.Series,
    LegendPosition = LegendPosition.Right
}.BindTo(viewModel, nameof(viewModel.LegendPosition), nameof(cartesianChart.LegendPosition));
```

## License

This project is licensed under the [MIT License](/LICENSE).
