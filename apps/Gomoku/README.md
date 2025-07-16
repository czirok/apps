# Pela Gomoku

## Inspiration

Thanks to Petr Laštovička amazing [Piskvork](https://github.com/plastovicka/Piskvork) Gomoku game, which inspired this project.

Thanks to Marcel Tiede's fantastic work on [GirCore](https://github.com/gircore).

![Demo image](/apps/Gomoku/gomoku.png)

## Features

* **Play Gomoku** against the 2003 Microsoft competition-winning AI engine (Pela).
* **Adjust AI thinking time** from 1 to 60 seconds.
* **Choose who starts**: human or AI.
* **Undo / Redo support** with full move history tracking.
* **Board themes**: Wood with stones, or Paper and pencil style.
* **Multi-language support** with a dynamic language selector.
* **Copy game state** using Gomoku AI protocol (GomokuCup compatible).
* **Modern UI** powered by GTK4 and Libadwaita.
* **Rendering with Maui.Graphics + SkiaSharp** – clean, scalable, hardware-accelerated board rendering.
* **Linux-native** desktop application built with GirCore.

## Supported platforms

> [!NOTE]
> GNOME 48 or later is required to run this application.

Packages tested on:

|OS|Version|
|---|---|
|Arch Linux (latest)|[pelagomoku-1.0.0-1-x86_64.pkg.tar.zst](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-1-x86_64.pkg.tar.zst)|
|Debian Bookworm (12)|[pelagomoku_1.0.0_amd64.deb](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku_1.0.0_amd64.deb)|
|Ubuntu Plucky Puffin (25.04)|[pelagomoku_1.0.0_amd64.deb](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku_1.0.0_amd64.deb)|
|Fedora Adams (42)|[pelagomoku-1.0.0-1.x86_64.rpm](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-1.x86_64.rpm)|
|OpenSUSE Tumbleweed (latest)|[pelagomoku-1.0.0-1.x86_64.rpm](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-1.x86_64.rpm)|
|Portable (tar.gz)|[pelagomoku-1.0.0-x86_64.tar.gz](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-x86_64.tar.gz)|

Flatpak package tested on:

|OS|Version|
|---|---|
|Ubuntu Noble Numbat (24.04)|[pelagomoku-1.0.0-x86_64.flatpak](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-x86_64.flatpak)|
|CentOS Stream 10|[pelagomoku-1.0.0-x86_64.flatpak](https://github.com/czirok/apps/releases/download/v2025.07.16-apps/pelagomoku-1.0.0-x86_64.flatpak)|

## Installation

### Package Installation

|OS|Package|
|---|---|
|**Arch Linux:**|`sudo pacman -U pelagomoku-1.0.0-1-x86_64.pkg.tar.zst`|
|**Debian/Ubuntu:**|`sudo dpkg -i pelagomoku_1.0.0_amd64.deb`|
|**Fedora:**|`sudo dnf install pelagomoku-1.0.0-1.x86_64.rpm`|
|**openSUSE:**|`sudo zypper install --allow-unsigned-rpm pelagomoku-1.0.0-1.x86_64.rpm`|
|**Flatpak:**|`flatpak install pelagomoku-1.0.0-x86_64.flatpak`|

**tar.gz (portable):**

Install `cairo libadwaita gdk-pixbuf2` dependencies, then:

  ```bash
  tar -xzf pelagomoku-1.0.0-x86_64.tar.gz
  cd pelagomoku-1.0.0
  ./install-to-home.sh     # Add to applications menu
  ./pelagomoku
  ```
  
### Package Removal

|OS|Command|
|---|---|
|**Arch Linux:**|`sudo pacman -R pelagomoku`|
|**Debian/Ubuntu:**|`sudo apt remove pelagomoku`|
|**Fedora:**|`sudo dnf remove pelagomoku`|
|**openSUSE:**|`sudo zypper remove pelagomoku`|
|**Flatpak:**|`flatpak uninstall org.gnome.pelagomoku`|
|**tar.gz:**|`./uninstall-from-home.sh` (if installed to menu)|

## Building

.NET 9.0 Runtime is required to run. To build the project, you can use the following command:

> [!IMPORTANT]
> After clone, you need copy the `org.gnome.pelagomoku.gschema.xml` file to `$HOME/.local/share/glib-2.0/schemas` and run `glib-compile-schemas $HOME/.local/share/glib-2.0/schemas` to update the schema cache.

```bash
git clone https://github.com/czirok/apps.git

# copy and build schema
cp apps/Gomoku/os/org.gnome.pelagomoku.gschema.xml $HOME/.local/share/glib-2.0/schemas
glib-compile-schemas $HOME/.local/share/glib-2.0/schemas

cd apps/Gomoku/src
dotnet restore
dotnet run
```

### AOT compilation

```bash
cd apps/Gomoku/src
dotnet restore
dotnet publish -p:PublishProfile=Release.pubxml
cd ../publish
ls -lh
```

The resulting binary is a fully self-contained, Ahead-of-Time compiled Linux desktop app. No .NET runtime is required to run it. Total app size (including everything):

```bash
...
8.7M libSkiaSharp.so
7.4M Gomoku
...
```

### Packaging

[Packaging scripts](/apps/Gomoku/os/README.md) are available for various Linux distributions, including Arch Linux, Debian, Ubuntu, Fedora, OpenSUSE, and tar.gz. Flatpak packaging is also supported.

## License

This project is licensed under the [GNU General Public License](/apps/Gomoku/LICENSE) license.
