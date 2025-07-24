# EasyUIBinding.GirCore - XAML-like Data Binding for GTK

![EasyUIBinding.GirCore](/assets/nuget/EasyUIBinding.GirCore.svg)

## Overview

EasyUIBinding.GirCore brings **declarative, type-safe data binding** to GTK applications using C#. It provides a fluent API that mirrors XAML's binding capabilities while maintaining compile-time safety and IntelliSense support.

## Requirements

- .NET 8.0 or later and of course GirCore

## Installation

```bash
dotnet add package EasyUIBinding.GirCore --version 0.7.0-preview.1.4
```

## Quick Start

See the [Easy UI Binding](/samples/EasyUIBinding/) sample.

![Easy UI Binding](/assets/EasyUIBinding.png)

## XAML vs. GirCore Binding Comparison

### XAML Approach

```xml
<UserControl.DataContext>
    <vms:ViewModel/>
</UserControl.DataContext>

<ComboBox 
    ItemsSource="{Binding Positions}"
    DisplayMemberPath="Name"
    SelectedValuePath="Position"
    SelectedValue="{Binding Position}"/>

<lvc:CartesianChart 
    Series="{Binding Series}" 
    LegendPosition="{Binding Position}" />
```

### GirCore Equivalent

```csharp
private readonly CartesianChart cartesianChart;
private readonly WrapToggle<LegendPosition> wrapToggle;

var viewModel = new ViewModel();

cartesianChart = new CartesianChart
{
    Series = viewModel.Series,
    LegendPosition = LegendPosition.Right,
}.BindTo(viewModel, nameof(viewModel.Position), nameof(cartesianChart.LegendPosition));

wrapToggle = new WrapToggle<LegendPosition>("LegendPosition", "", new Dictionary<LegendPosition, string>
{
    { LegendPosition.Hidden, "Hidden" },
    { LegendPosition.Top, "Top" },
    { LegendPosition.Bottom, "Bottom" },
    { LegendPosition.Left, "Left" },
    { LegendPosition.Right, "Right" }
}, LegendPosition.Left, LegendPosition.Top)
    .BindTo(viewModel, nameof(viewModel.Position));

// OR

wrapToggle = new WrapToggle<LegendPosition>("LegendPosition", "", viewModel.Positions, LegendPosition.Left, LegendPosition.Top)
    .BindTo(viewModel, nameof(viewModel.Position), nameof(wrapToggle.SelectedValue));
```

## Key Advantages

### ✅ **Fluent API**

- Chainable method calls for clean, readable code
- Object initialization combined with binding in one expression

### ✅ **Memory Safe**

- Automatic cleanup with `IDisposable` pattern
- `WeakReference` usage prevents memory leaks

### ✅ **Two-Way Binding**

- Changes in UI automatically update ViewModel
- ViewModel changes reflect in UI components
- Central ViewModel acts as "single source of truth"

## Binding Architecture

The binding system connects three components:

```text
UI Component ↔ ViewModel Property ↔ Chart/Widget
```

**Example Flow:**

1. User selects "Top" in WrapToggle
2. `viewModel.Position` updates to `LegendPosition.Top`
3. CartesianChart automatically updates its `LegendPosition`

## Event Handling

Combine binding with event handling for complete control:

```csharp
new WrapToggle<LegendPosition>(...)
    .BindTo(viewModel, nameof(viewModel.Position))
    .OnWrapToggleChanged((sender, args) => {
        logger.LogInformation("Legend position changed to: {Position}", args.Value);
    });
```

## Result

**Modern, declarative UI development for Linux desktop applications** with XAML-like productivity.

## Controls

### Basic Inputs

- `Switch` - Boolean toggle switch (Adw.SwitchRow)
- `Text` - Text entry field (Adw.EntryRow)
- `Password` - Password entry field (Adw.PasswordEntryRow)
- `SpinInteger`/`SpinDouble`/`SpinFloat` - Numeric spinners (Adw.SpinRow)
- `ScaleDouble` - Range sliders (Gtk.Scale)
- `Button` - Action button (Adw.ButtonRow)

### Selection Controls

- `WrapToggle<T>` - Multi-option toggle buttons (Adw.ActionRow + Adw.WrapBox)
- `Toggle<T>` - Toggle buttons (Adw.ToggleGroup)
- `WrapToggle` - Wrapped toggle buttons (Adw.WrapBox)
- `Combo<T>` - Dropdown selection (Adw.ComboRow)
- `ComboTuple<T>` - Dropdown selection (Adw.ComboRow)

### Specialized Controls

- `ColorSelector` - Color picker
- `FontSelector` - Font chooser
- `SaveAsSelector` - File save dialog with status icon
- `FileSelector` - File open dialog with status icon
- `ClipboardButton` - Copy to clipboard with status icon

## License

This project is licensed under the [MIT License](/LICENSE).
