#!/bin/bash
set -e

APP_NAME="nbody"
VERSION=$(cat VERSION)
ARCH="x86_64"
BUILD_DIR="./targz"
PUBLISH_DIR="../publish"
PACKAGE_DIR="$BUILD_DIR/${APP_NAME}-${VERSION}"

# Clean and create build structure
rm -rf "$BUILD_DIR"
mkdir -p "$PACKAGE_DIR"

echo "Building $APP_NAME-$VERSION-$ARCH.tar.gz..."

# Copy files directly to package root
cp "$PUBLISH_DIR/NBody" "$PACKAGE_DIR/nbody"
cp "$PUBLISH_DIR/libSkiaSharp.so" "$PACKAGE_DIR/libSkiaSharp.so"
cp "$PUBLISH_DIR/appsettings.json" "$PACKAGE_DIR/appsettings.json"
cp "$PUBLISH_DIR/LICENSE" "$PACKAGE_DIR/LICENSE"

# Copy I18N directory structure
cp -r "$PUBLISH_DIR/I18N" "$PACKAGE_DIR/"

# Copy desktop integration files
cp nbody.desktop "$PACKAGE_DIR/org.gnome.nbody.desktop"
cp nbody.svg "$PACKAGE_DIR/org.gnome.nbody.svg"

# Create desktop integration install script
cat > "$PACKAGE_DIR/install-desktop-and-icon-file-to-home.sh" << 'EOF'
#!/bin/bash
set -e

CURRENT_DIR="$(pwd)"
DESKTOP_DIR="$HOME/.local/share/applications"
ICON_DIR="$HOME/.local/share/icons/hicolor/scalable/apps"

echo "Installing desktop integration to user home..."

# Create directories
mkdir -p "$DESKTOP_DIR"
mkdir -p "$ICON_DIR"

# Update desktop file with current path
sed "s|Exec=nbody|Exec=$CURRENT_DIR/nbody|g" org.gnome.nbody.desktop > "$DESKTOP_DIR/org.gnome.nbody.desktop"

# Copy icon
cp org.gnome.nbody.svg "$ICON_DIR/org.gnome.nbody.svg"

# Update desktop database and icon cache
if command -v update-desktop-database >/dev/null 2>&1; then
    update-desktop-database "$DESKTOP_DIR" 2>/dev/null || true
fi

if command -v gtk-update-icon-cache >/dev/null 2>&1; then
    gtk-update-icon-cache -q -t -f "$HOME/.local/share/icons/hicolor" 2>/dev/null || true
fi

echo "Desktop integration installed!"
echo "nbody should now appear in your application menu."
EOF

# Create desktop integration uninstall script
cat > "$PACKAGE_DIR/uninstall-desktop-and-icon-file-from-home.sh" << 'EOF'
#!/bin/bash
set -e

DESKTOP_DIR="$HOME/.local/share/applications"
ICON_DIR="$HOME/.local/share/icons/hicolor/scalable/apps"

echo "Removing desktop integration from user home..."

# Remove files
rm -f "$DESKTOP_DIR/org.gnome.nbody.desktop"
rm -f "$ICON_DIR/org.gnome.nbody.svg"

# Update desktop database and icon cache
if command -v update-desktop-database >/dev/null 2>&1; then
    update-desktop-database "$DESKTOP_DIR" 2>/dev/null || true
fi

if command -v gtk-update-icon-cache >/dev/null 2>&1; then
    gtk-update-icon-cache -q -t -f "$HOME/.local/share/icons/hicolor" 2>/dev/null || true
fi

echo "Desktop integration removed!"
EOF

# Make scripts executable
chmod +x "$PACKAGE_DIR/install-desktop-and-icon-file-to-home.sh"
chmod +x "$PACKAGE_DIR/uninstall-desktop-and-icon-file-from-home.sh"

# Create the tar.gz package
cd "$BUILD_DIR"
tar -czf "${APP_NAME}-${VERSION}-${ARCH}.tar.gz" "${APP_NAME}-${VERSION}/"

echo "Package created: $BUILD_DIR/${APP_NAME}-${VERSION}-${ARCH}.tar.gz"
echo "Size: $(du -h "${APP_NAME}-${VERSION}-${ARCH}.tar.gz" | cut -f1)"

# Show contents
echo -e "\nPackage contents:"
tar -tzf "${APP_NAME}-${VERSION}-${ARCH}.tar.gz" | sort