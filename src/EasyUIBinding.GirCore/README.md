# EasyUIBinding.GirCore

![EasyUIBinding.GirCore](/assets/nuget/EasyUIBinding.GirCore.svg)

**Declarative UI binding library for GTK4/Libadwaita applications using GirCore.**

This package provides a fluent API for creating data-bound preference interfaces with automatic property synchronization and change notifications, inspired by `Gio.Settings` binding patterns.

## Features

- **Data binding** - Two-way binding between UI controls and model properties
- **Fluent API** - Chainable configuration methods
- **GTK4/Libadwaita** - Native Linux UI components via GirCore
- **Type safety** - Generic input controls with compile-time type checking
- **Automatic disposal** - Proper resource cleanup and event unsubscription

## Requirements

- .NET 8.0 or later

## Installation

```bash
dotnet add package EasyUIBinding.GirCore
```

## Quick Start

See the [Easy UI Binding](/samples/EasyUIBinding/) sample.

![Easy UI Binding](/assets/EasyUIBinding.png)

## Controls

### Basic Inputs

- `Switch` - Boolean toggle switch (Adw.SwitchRow)
- `Text` - Text entry field (Adw.EntryRow)  
- `SpinInteger`/`SpinDouble`/`SpinFloat` - Numeric spinners (Adw.SpinRow)
- `Button` - Action button (Adw.ButtonRow)

### Selection Controls

- `WrapToggle<T>` - Multi-option toggle buttons (Adw.ActionRow + Adw.WrapBox)
- `Toggle<T>` - Single toggle button
- `Combo<T>` - Dropdown selection (Adw.ComboRow)

### Specialized Controls

- `ColorSelector` - Color picker
- `FontSelector` - Font chooser
- `SaveAsSelector` - File save dialog
- `ClipboardButton` - Copy to clipboard

## Data Binding

### Automatic Property Generation

Use `[GirCoreNotify]` attribute with the source generator:

```csharp
public partial class Settings : NotifyPropertyModel
{
    [GirCoreNotify]
    private bool enabled = true;

    [GirCoreNotify]
    private string title = "Easy UI Binding";

    // Generator creates:
    // public bool Enabled { get; set; }
    // public string Title { get; set; }
}
```

## License

This project is licensed under the [MIT License](/LICENSE).
