# Yaml.Localization

![Yaml.Localization](/assets/nuget/Yaml.Localization.svg)

> Better to fall asleep on [Elm Street](https://en.wikipedia.org/wiki/A_Nightmare_on_Elm_Street) than use Blazor localization.

A modern localization library for .NET applications that uses YAML files instead of traditional .resx files. Works with any .NET application where IStringLocalizer dependency injection is available - from console applications to Blazor platforms, with automatic RTL/LTR handling.

![Yaml Localization](/assets/Yaml.Localization.png)

## Core Features

- **YAML-based localization** - Readable `.yaml` format instead of `.resx` files
- **Microsoft IStringLocalizer** compatibility with standard .NET localization API
- **Embedded Resource** integration with compile-time embedded translations
- **File System** support for direct YAML file reading
- **Multi-platform Blazor** support - same code across all Blazor platforms
- **Automatic culture management** with platform-specific storage (localStorage/cookie)
- **RTL/LTR support** for automatic right-to-left language handling
- **Bootstrap theme integration** with automatic CSS switching between RTL/LTR
- **SEO-friendly rendering** with static content + interactive componets

## Platform Support

### Platforms

- **Blazor GirCore** - Linux desktop applications (WebKit + GTK4)
- **Blazor Maui** - Cross-platform desktop/mobile (Windows, macOS, iOS, Android, Tizen)
- **Blazor WebAssembly** - Standalone WebAssembly applications
- **Blazor Web App** - Server-side Blazor applications
- **Blazor Web App Client** - Hosted WebAssembly applications
- **Everything else** - Any platform/app where `IStringLocalizer` dependency injection is available

### System Requirements

- **.NET 8.0** or later

## Installation

```bash
dotnet add package Yaml.Localization --version 10.0.0-preview.6
```

## YAML File Structure

### Basic File Structure

#### en.yaml

```yaml
"Hello World": "Hello World"
"Welcome {0}": "Welcome {0}"
"Language ({0})": "Language ({0})"
```

#### hu.yaml

```yaml
"Hello World": "Helló Világ"
"Welcome {0}": "Üdvözöljük {0}"
"Language ({0})": "Nyelv ({0})"
```

#### CultureSettings.yaml

```yaml
Cultures:
  - Name: en-US
    Active: true
    Default: true
  - Name: hu
    Active: true
  - Name: de-DE
    Active: true
  - Name: es-ES
    Active: true
  - Name: ar
    Active: true
    Rtl: true
  - Name: he-IL
    Active: true
    Rtl: true

Selector:
  en-US: English
  hu: Magyar
  de-DE: Deutsch
  es-ES: Español
  ar: العربية
  he-IL: עברית

CookieName: .lang
RedirectEndpoint: culture/set

```

## Embedded Resource Configuration

### One file per language (recommended)

```xml
  <ItemGroup>
    <Content Remove="I18N\*.yaml" />
    <EmbeddedResource
      Exclude="I18N\cultures.yaml"
      Type="Non-Resx"
      WithCulture="false"
      Include="I18N\*.yaml"
      LogicalName="$(RootNamespace).Lang.%(Filename).yaml"
    />

    <Content Remove="I18N\cultures.yaml" />
    <EmbeddedResource
      Include="I18N\cultures.yaml"
      LogicalName="CultureSettings.yaml"
    />
  </ItemGroup>
```

#### Result per language file

- `I18N\en.yaml` → `MyApp.Lang.en.yaml`
- `I18N\hu.yaml` → `MyApp.Lang.hu.yaml`
- `I18N\cultures.yaml` → `CultureSettings.yaml`

### Component-level

```xml
<ItemGroup>
  <EmbeddedResource
    Exclude="Components\cultures.yaml"
    Type="Non-Resx"
    WithCulture="false"
    Include="Components\**\*.razor*.yaml"
    LogicalName="$(RootNamespace).$([System.String]::Copy('%(RelativeDir)').Replace('\','.').Replace('/','.'))$([System.String]::Copy('%(Filename)').Replace('.razor','')).yaml" />

  <Content Remove="Components\cultures.yaml" />
    <EmbeddedResource
      Include="Components\cultures.yaml"
      LogicalName="CultureSettings.yaml"
    />
</ItemGroup>

```

#### Result per component file

- `Components\cultures.yaml` → `CultureSettings.yaml`
- `Components\Home.razor.en.yaml` → `MyApp.Components.Home.en.yaml`
- `Components\Home.razor.hu.yaml` → `MyApp.Components.Home.hu.yaml`

### Embedded Resource Patterns

- [Yaml Localization Tests](/test/Yaml.Localization/YamlLocalizationTests/YamlLocalizationTests.csproj)
- [Simulated Resource Path Tests](/test/Yaml.Localization/SimulatedResourcePathTests/YamlLocalizationSimulatedResourcePathTests.csproj)
- [Mixed Namespace Tests](/test/Yaml.Localization/MixedNamespaceTests/YamlLocalizationMixedNamespaceTests.csproj)

### Documentation on using `LocalName`

- [MSBuild well-known item metadata](https://learn.microsoft.com/en-us/visualstudio/msbuild/msbuild-well-known-item-metadata)
- [Static property functions](https://learn.microsoft.com/en-us/visualstudio/msbuild/property-functions#static-property-functions)

## Platform Services

See the [`IPlatformService`](/src/Yaml.Localization/src/PlatformService.cs) interface for platform-specific services.

### RenderMode mapping for interactive components

- **GirCore**/**Maui**: `default!`
- **WebApp**: `InteractiveServer`
- **WebApp WebAssembly**: `InteractiveWebAssembly`
- **WebAssembly**: `InteractiveWebAssembly`

## Application and Blazor Integration

### Service Registration

```csharp
builder.Services
// GirCore
    .AddSingleton<IPlatformService, GirCorePlatformService>()
// Maui Desktop  
    .AddSingleton<IPlatformService, MauiPointerPlatformService>()
// Maui Mobile
    .AddSingleton<IPlatformService, MauiTouchPlatformService>()
// Standalone WebAssembly
    .AddScoped<IPlatformService, WebAssemblyPlatformService>()
// Blazor App
    .AddScoped<IPlatformService, WebAppPlatformService>()
// Blazor App WebAssembly
    .AddScoped<IPlatformService, WebAppWebAssemblyPlatformService>()
  
    .AddScoped<ChangeThemeStore>()
    .AddScoped<CultureSelectorStore>()
    .AddYamlEmbeddedResourceLocalization(typeof(BlazorShared.Lang).Assembly);
```

### Blazor Web App Server-side Configuration

```csharp
// immediately before calling MapRazorComponents.
app.SetWebAppYamlLocalization();
```

### Component Usage

```html
@inject IStringLocalizer<Lang> L

<h1>@L["Hello World"]</h1>
<p>@L["Welcome {0}", "User"]</p>
```

### Platform-specific rendering

#### SEO-friendly static content

```html
<NavLink href="counter">@L["Counter"]</NavLink>
<NavLink href="weather">@L["Weather"]</NavLink>
<p>@L["About"]</p>
```

#### Interactive components with platform-specific render mode

```html
<CultureSelector @rendermode="PlatformService.RenderMode" />
<ChangeTheme @rendermode="PlatformService.RenderMode" />
```

## RTL/LTR Support

### Writing direction settings in CultureSettings.yaml

```yaml
Cultures:
  - Name: en-US
    Active: true
    Default: true
  - Name: es-ES
    Active: true
  - Name: ar
    Active: true
    Rtl: true
  - Name: he-IL
    Active: true
    Rtl: true
```

### Bootstrap CSS automatic switching

#### Blazor GirCore/Maui/WebAssembly index.html

```html
<html lang="en" dir="ltr" data-bs-theme="auto">

  <link rel="stylesheet" href="css/bootstrap.min.css" id="ltr-css" disabled>
  <link rel="stylesheet" href="css/bootstrap.rtl.min.css" id="rtl-css" disabled>

  <script>
      window.BlazorLTRId = "ltr-css";
      window.BlazorRTLId = "rtl-css";
  </script>
  <script src="js/blazor.head.min.js"></script>
```

##### JavaScript Control - index.html - blazor.head.min.js

1. Detects the active culture's RTL property
2. Enable/disable the appropriate CSS file
3. Updates the `<html dir="">` attribute
4. Saves the setting
   - GirCore/Maui localStorage
   - WebAssembly cookie

### Blazor Web App App.razor

```html
<html lang="@CultureInfo.CurrentUICulture.TwoLetterISOLanguageName"
      dir="@(CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft ? "rtl" : "ltr")">

  @if (CultureInfo.CurrentUICulture.TextInfo.IsRightToLeft)
  {
      <link rel="stylesheet" href="css/bootstrap.rtl.min.css" />
  }
  else
  {
      <link rel="stylesheet" href="css/bootstrap.min.css" />
  }

  <script>
      window.BlazorLTRId = "ltr-css";
      window.BlazorRTLId = "rtl-css";
  </script>
  <script src="js/blazor.web.head.min.js"></script>

```

#### JavaScript Control - App.razor - blazor.web.head.min.js

1. Saves the setting to a cookie

## Examples

Full working examples can be found in the samples folder:

### Shared Components

- **[BlazorShared](/samples/Yaml.Localization/BlazorShared/)** - Shared UI components and localization files

### Multi Blazor localization

- **[BlazorGirCore](/samples/Yaml.Localization/BlazorGirCore/)** - Linux desktop application with WebKit
- **[BlazorMaui](/samples/Yaml.Localization/BlazorMaui/)** - Cross-platform desktop/mobile application
- **[BlazorWasm](/samples/Yaml.Localization/BlazorWasm/)** - Standalone WebAssembly application
- **[BlazorWebApp](/samples/Yaml.Localization/BlazorWebApp/)** - Server-side Blazor application
- **[BlazorWebAppClient](/samples/Yaml.Localization/BlazorWebAppClient/)** - Hosted WebAssembly application

### Desktop application

- **[GirCoreApp](/samples/Yaml.Localization/GirCoreApp/)** - Native GTK4 application with localization

### Samples contain  

- RTL/LTR language support (العربية, עברית, English, Magyar, Deutsch)
- Dark/Light/Auto theme switching
- Platform-specific culture handling
- SEO-friendly rendering for Web App

### Localization-related files in samples

- BlazorShared
  - [BlazorShared.csproj](/samples/Yaml.Localization/BlazorShared/BlazorShared.csproj)
  - I18N
    - [ar.yaml](/samples/Yaml.Localization/BlazorShared/I18N/ar.yaml)
    - [de.yaml](/samples/Yaml.Localization/BlazorShared/I18N/de.yaml)
    - [en.yaml](/samples/Yaml.Localization/BlazorShared/I18N/en.yaml)
    - [es.yaml](/samples/Yaml.Localization/BlazorShared/I18N/es.yaml)
    - [he.yaml](/samples/Yaml.Localization/BlazorShared/I18N/he.yaml)
    - [hu-HU.yaml](/samples/Yaml.Localization/BlazorShared/I18N/hu-HU.yaml)
    - [cultures.yaml](/samples/Yaml.Localization/BlazorShared/I18N/cultures.yaml)
  - [Lang.cs](/samples/Yaml.Localization/BlazorShared/Lang.cs) - Empty  class for `IStringLocalizer<Lang>` injection
  - wwwroot
    - css
      - bootstrap.min.css
      - bootstrap.rtl.min.css
    - js
      - [blazor.head.min.js](/packages/js/src/BlazorHead.ts) - TypeScript reference
      - [blazor.web.head.min.js](/packages/js/src/BlazorWebHead.ts) - TypeScript reference
      - [blazor.body.min.js](/packages/js/src/BlazorBody.ts) - TypeScript reference
      - bootstrap.bundle.min.js
- BlazorGirCore
  - [BlazorGirCore.csproj](/samples/Yaml.Localization/BlazorGirCore/BlazorGirCore.csproj)
  - Components
    - [App.razor](/samples/Yaml.Localization/BlazorGirCore/Components/App.razor)
  - [Program.cs](/samples/Yaml.Localization/BlazorGirCore/Program.cs)
  - wwwroot
    - [index.html](/samples/Yaml.Localization/BlazorGirCore/wwwroot/index.html)
- BlazorMaui
  - [BlazorMaui.csproj](/samples/Yaml.Localization/BlazorMaui/BlazorMaui.csproj)
  - Components
    - [App.razor](/samples/Yaml.Localization/BlazorMaui/Components/App.razor)
  - [MauiProgram.cs](/samples/Yaml.Localization/BlazorMaui/MauiProgram.cs)
  - wwwroot
    - [index.html](/samples/Yaml.Localization/BlazorMaui/wwwroot/index.html)
- BlazorWasm
  - [BlazorWasm.csproj](/samples/Yaml.Localization/BlazorWasm/BlazorWasm.csproj)
  - Components
    - [App.razor](/samples/Yaml.Localization/BlazorWasm/Components/App.razor)
  - [Program.cs](/samples/Yaml.Localization/BlazorWasm/Program.cs)
  - wwwroot
    - [index.html](/samples/Yaml.Localization/BlazorWasm/wwwroot/index.html)
- BlazorWebApp
  - [BlazorWebApp.csproj](/samples/Yaml.Localization/BlazorWebApp/BlazorWebApp.csproj)
  - Components
    - [App.razor](/samples/Yaml.Localization/BlazorWebApp/Components/App.razor)
  - [Program.cs](/samples/Yaml.Localization/BlazorWebApp/Program.cs)
- BlazorWebAppClient
  - [BlazorWebAppClient.csproj](/samples/Yaml.Localization/BlazorWebAppClient/BlazorWebAppClient.csproj)
  - Components
    - [Routes.razor](/samples/Yaml.Localization/BlazorWebAppClient/Components/Routes.razor)
  - [Program.cs](/samples/Yaml.Localization/BlazorWebAppClient/Program.cs)

## License

This project is licensed under the [MIT License](/LICENSE).
