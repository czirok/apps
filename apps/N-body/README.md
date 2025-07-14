# N-body simulation

## Inspiration

Thanks to Zong Zheng Li's amazing [N-body simulation](https://zongzhengli.github.io/nbody.html) work, which inspired this project.

Thanks to Marcel Tiede's fantastic work on [GirCore](https://github.com/gircore).

![Demo image](/apps/N-body/n-body.gif)

... and many more simulations.

## Features

- Real-time gravitational N-body simulation
- Interactive visualization with zoom and pan
- Multiple predefined scenarios
- Cross-platform Linux support
- Modern GTK4/Libadwaita UI
- Multi-language support (English, German, Hungarian)
- Ahead-of-Time (AOT) compiled for super small size

## Supported platforms

> [!NOTE]
> GNOME 48 or later is required to run this application.

Packages tested on:

|OS|Version|
|---|---|
|Arch Linux (latest)|[nbody-1.2.0-1-x86_64.pkg.tar.zst](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-1-x86_64.pkg.tar.zst)|
|Debian Bookworm (12)|[nbody_1.2.0_amd64.deb](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody_1.2.0_amd64.deb)|
|Ubuntu Plucky Puffin (25.04)|[nbody_1.2.0_amd64.deb](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody_1.2.0_amd64.deb)|
|Fedora Adams (42)|[nbody-1.2.0-1.x86_64.rpm](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-1.x86_64.rpm)|
|OpenSUSE Tumbleweed (latest)|[nbody-1.2.0-1.x86_64.rpm](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-1.x86_64.rpm)|
|Portable (tar.gz)|[nbody-1.2.0-x86_64.tar.gz](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-x86_64.tar.gz)|

Flatpak package tested on:

|OS|Version|
|---|---|
|Ubuntu Noble Numbat (24.04)|[nbody-1.2.0-x86_64.flatpak](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-x86_64.flatpak)|
|CentOS Stream 10|[nbody-1.2.0-x86_64.flatpak](https://github.com/czirok/apps/releases/download/v2025.07.14-apps/nbody-1.2.0-x86_64.flatpak)|

## Installation

### Package Installation

|OS|Package|
|---|---|
|**Arch Linux:**|`sudo pacman -U nbody-1.2.0-1-x86_64.pkg.tar.zst`|
|**Debian/Ubuntu:**|`sudo dpkg -i nbody_1.2.0_amd64.deb`|
|**Fedora:**|`sudo dnf install nbody-1.2.0-1.x86_64.rpm`|
|**openSUSE:**|`sudo zypper install nbody-1.2.0-1.x86_64.rpm`|
|**Flatpak:**|`flatpak install nbody-1.2.0-x86_64.flatpak`|

**tar.gz (portable):**

Install `cairo libadwaita gdk-pixbuf2` dependencies, then:

  ```bash
  tar -xzf nbody-1.2.0-x86_64.tar.gz
  cd nbody-1.2.0
  ./nbody                                        # Direct run
  ./install-desktop-and-icon-file-to-home.sh     # Add to applications menu
  ```
  
### Package Removal

|OS|Command|
|---|---|
|**Arch Linux:**|`sudo pacman -R nbody`|
|**Debian/Ubuntu:**|`sudo apt remove nbody`|
|**Fedora:**|`sudo dnf remove nbody`|
|**openSUSE:**|`sudo zypper remove nbody`|
|**Flatpak:**|`flatpak uninstall org.gnome.nbody`|
|**tar.gz:**|`./uninstall-desktop-and-icon-file-from-home.sh` (if installed to menu)|

## Building

.NET 9.0 Runtime is required to run. To build the project, you can use the following command:

```bash
git clone https://github.com/czirok/apps.git
cd apps/N-body/src
dotnet restore
dotnet run
```

### AOT compilation

```bash
cd apps/N-body/src
dotnet restore
dotnet publish -p:PublishProfile=Release.pubxml
cd ../publish
```

The resulting binary is a fully self-contained, Ahead-of-Time compiled Linux desktop app. No .NET runtime is required to run it. Total app size (including everything):

```bash
...
8.7M libSkiaSharp.so
6.8M NBody
...
```

### Packaging

[Packaging scripts](/apps/N-body/os/README.md) are available for various Linux distributions, including Arch Linux, Debian, Ubuntu, Fedora, OpenSUSE, and tar.gz. Flatpak packaging is also supported.

## License

This project is licensed under the [CC BY 4.0](/apps/N-body/LICENSE) license.
