#!/bin/bash
set -x

TEMP_DIR="./arch"
PUBLISH_DIR="../publish"

mkdir -p "$TEMP_DIR"

# Copy license
cp "$PUBLISH_DIR/LICENSE" "$TEMP_DIR/LICENSE"

# Copy PKGBUILD
cp PKGBUILD "$TEMP_DIR/PKGBUILD"

# Copy binary files
cp "$PUBLISH_DIR/NBody" "$TEMP_DIR/nbody"
cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_DIR/libSkiaSharp.so"

# Copy configuration
cp "$PUBLISH_DIR/appsettings.json" "$TEMP_DIR/appsettings.json"

# Tar create for I18N
tar -czf "$TEMP_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" "I18N/"

# Copy desktop files  
cp nbody.desktop "$TEMP_DIR/nbody.desktop"
cp nbody.svg "$TEMP_DIR/nbody.svg"
cp org.gnome.nbody.gschema.xml "$TEMP_DIR/org.gnome.nbody.gschema.xml"

# Copy install hook
cp nbody.arch.install "$TEMP_DIR/nbody.install"

cd "$TEMP_DIR"
makepkg -f -C -s
