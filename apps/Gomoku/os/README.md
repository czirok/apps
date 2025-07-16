# Packaging

Before running the packaging scripts, create the AOT compilation of the application. This is necessary for the packaging scripts to work correctly.

> [!IMPORTANT]
> After clone, you need copy the `org.gnome.pelagomoku.gschema.xml` file to `$HOME/.local/share/glib-2.0/schemas` and run `glib-compile-schemas $HOME/.local/share/glib-2.0/schemas` to update the schema cache.

## Clone

```bash
git clone https://github.com/czirok/apps.git

# copy and build schema
cp apps/Gomoku/os/org.gnome.pelagomoku.gschema.xml $HOME/.local/share/glib-2.0/schemas
glib-compile-schemas $HOME/.local/share/glib-2.0/schemas

# remove schema
rm $HOME/.cache/glib-2.0/schemas/org.gnome.pelagomoku.gschema.xml
glib-compile-schemas $HOME/.local/share/glib-2.0/schemas
```

## AOT compilation

```bash
cd apps/Gomoku/src
dotnet restore
dotnet publish -p:PublishProfile=Release.pubxml

# run
cd ../publish
./Gomoku
```

## Arch Linux

```bash
cd apps/Gomoku/os
chmod +x build-arch.sh
./build-arch.sh

# install the package
sudo pacman -U ./arch/pelagomoku-1.0.0-1-x86_64.pkg.tar.zst

# uninstall the package
sudo pacman -Rns pelagomoku
```

## Debian and Ubuntu

```bash
cd apps/Gomoku/os
chmod +x build-deb.sh
./build-deb.sh

# install the package
sudo dpkg -i ./deb/pelagomoku_1.0.0_amd64.deb

# uninstall the package
sudo apt remove --purge pelagomoku
```

## Flatpak

```bash
cd apps/Gomoku/os
chmod +x build-flatpak.sh
./build-flatpak.sh

# install the package
flatpak install --user --reinstall pelagomoku-1.0.0-x86_64.flatpak

# run the application
flatpak run org.gnome.pelagomoku

# uninstall the package
flatpak uninstall org.gnome.pelagomoku
```

## OpenSUSE

```bash
cd apps/Gomoku/os
chmod +x build-rpm.sh
./build-rpm.sh

# install the package
sudo zypper install --allow-unsigned-rpm pelagomoku-1.0.0-1.x86_64.rpm

# uninstall the package
sudo zypper remove pelagomoku
```

## Fedora

```bash
cd apps/Gomoku/os
chmod +x build-rpm.sh
./build-rpm.sh

# install the package
sudo dnf install pelagomoku-1.0.0-1.x86_64.rpm

# uninstall the package
sudo dnf remove pelagomoku
```

## tar.gz

```bash
cd apps/Gomoku/os
chmod +x build-targz.sh
./build-targz.sh

# extract the package to ~/ or any other directory you prefer
tar -xzf pelagomoku-1.0.0-x86_64.tar.gz -C ~/

# install .desktop and icon files
cd ~/pelagomoku-1.0.0
./install-to-home.sh

# run the application
cd ~/pelagomoku-1.0.0
./pelagomoku

# uninstall the package
cd ~/pelagomoku-1.0.0
./uninstall-from-home.sh
cd ..
rm -rf ~/pelagomoku-1.0.0
```
