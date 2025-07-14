# Packaging

Before running the packaging scripts, create the AOT compilation of the application. This is necessary for the packaging scripts to work correctly.

## AOT compilation

```bash
cd apps/N-body/src
dotnet restore
dotnet publish -p:PublishProfile=Release.pubxml

# run
cd ../publish
./NBody
```

## Arch Linux

```bash
cd apps/N-body/os
./build-arch.sh

# install the package
sudo pacman -U ./arch/nbody-1.2.0-1-x86_64.pkg.tar.zst

# uninstall the package
sudo pacman -Rns nbody
```

## Debian and Ubuntu

```bash
cd apps/N-body/os
./build-deb.sh

# install the package
sudo dpkg -i ./deb/nbody_1.2.0_amd64.deb

# uninstall the package
sudo apt remove --purge nbody
```

## Flatpak

```bash
cd apps/N-body/os
./build-flatpak.sh

# install the package
flatpak install --user --reinstall nbody-1.2.0-x86_64.flatpak

# run the application
flatpak run org.gnome.nbody

# uninstall the package
flatpak uninstall org.gnome.nbody
```

## OpenSUSE

```bash
cd apps/N-body/os
./build-rpm.sh

# install the package
sudo zypper install nbody-1.2.0-1.x86_64.rpm

# uninstall the package
sudo zypper remove nbody
```

## Fedora

```bash
cd apps/N-body/os
./build-rpm.sh

# install the package
sudo dnf install nbody-1.2.0-1.x86_64.rpm

# uninstall the package
sudo dnf remove nbody
```

## tar.gz

```bash
cd apps/N-body/os
./build-targz.sh

# extract the package to ~/ or any other directory you prefer
tar -xzf nbody-1.2.0-x86_64.tar.gz -C ~/

# install .desktop and icon files
cd ~/nbody-1.2.0
./install-desktop-and-icon-file-to-home.sh

# run the application
cd ~/nbody-1.2.0
./nbody

# uninstall the package
cd ~/nbody-1.2.0
./uninstall-desktop-and-icon-file-from-home.sh
cd ..
rm -rf ~/nbody-1.2.0
```
