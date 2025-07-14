#!/bin/bash
set -x

VERSION=$(cat VERSION)
ARCH="x86_64"
TEMP_DIR="./flatpak"
PUBLISH_DIR="../publish"

mkdir -p "$TEMP_DIR"

# Copy PKGBUILD
cp flatpak.yaml "$TEMP_DIR/nbody.yaml"

# Copy license
cp "$PUBLISH_DIR/LICENSE" "$TEMP_DIR/LICENSE"

# Copy binary files
cp "$PUBLISH_DIR/NBody" "$TEMP_DIR/nbody"
cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_DIR/libSkiaSharp.so"

# Copy configuration
cp "$PUBLISH_DIR/appsettings.json" "$TEMP_DIR/appsettings.json"

# Tar create for I18N
tar -czf "$TEMP_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" I18N

# Copy desktop files  
cp nbody.desktop "$TEMP_DIR/nbody.desktop"
cp nbody.svg "$TEMP_DIR/nbody.svg"
cp org.gnome.nbody.gschema.xml "$TEMP_DIR/org.gnome.nbody.gschema.xml"

cd "$TEMP_DIR"
# flatpak-builder --user --install --force-clean build-dir nbody.yaml
flatpak-builder --force-clean --repo=repo build-dir nbody.yaml
flatpak build-bundle repo "nbody-${VERSION}-${ARCH}.flatpak" org.gnome.nbody