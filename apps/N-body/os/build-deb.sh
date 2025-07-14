#!/bin/bash
set -e

APP_ID="nbody"
VERSION=$(cat VERSION)
ARCH="amd64"
BUILD_DIR="./deb"
DEB_DIR="$BUILD_DIR/${APP_ID}-deb"
PUBLISH_DIR="../publish"
LIBDIR="/usr/lib/$APP_ID"

# Clean
rm -rf "$BUILD_DIR"
mkdir -p "$DEB_DIR/DEBIAN"
mkdir -p "$DEB_DIR$LIBDIR"
mkdir -p "$DEB_DIR/usr/bin"
mkdir -p "$DEB_DIR/usr/share/applications"
mkdir -p "$DEB_DIR/usr/share/icons/hicolor/scalable/apps"
mkdir -p "$DEB_DIR/usr/share/glib-2.0/schemas"

# Control file
cat > "$DEB_DIR/DEBIAN/control" <<EOF
Package: $APP_ID
Version: $VERSION
Section: utils
Priority: optional
Architecture: $ARCH
Maintainer: Ferenc Czirok <ferenc@czirok.net>
Description: N-body gravitational simulation
Depends: libcairo2, libadwaita-1-0, libgdk-pixbuf-2.0-0
License: CC BY 4.0
EOF

# Binary + files to the libdir
install -Dm755 "$PUBLISH_DIR/NBody" "$DEB_DIR$LIBDIR/nbody"
install -Dm644 "$PUBLISH_DIR/libSkiaSharp.so" "$DEB_DIR$LIBDIR/libSkiaSharp.so"
install -Dm644 "$PUBLISH_DIR/appsettings.json" "$DEB_DIR$LIBDIR/appsettings.json"
install -Dm644 "$PUBLISH_DIR/LICENSE" "$DEB_DIR$LIBDIR/LICENSE"
cp -r "$PUBLISH_DIR/I18N" "$DEB_DIR$LIBDIR/"

# Wrapper script in /usr/bin
cat > "$DEB_DIR/usr/bin/nbody" <<EOF
#!/bin/sh
exec "$LIBDIR/nbody"
EOF
chmod +x "$DEB_DIR/usr/bin/nbody"

# Desktop + schema + icon
install -Dm644 nbody.desktop "$DEB_DIR/usr/share/applications/org.gnome.nbody.desktop"
install -Dm644 nbody.svg "$DEB_DIR/usr/share/icons/hicolor/scalable/apps/org.gnome.nbody.svg"
install -Dm644 org.gnome.nbody.gschema.xml "$DEB_DIR/usr/share/glib-2.0/schemas/org.gnome.nbody.gschema.xml"

# Package build
dpkg-deb --build "$DEB_DIR" "$BUILD_DIR/${APP_ID}_${VERSION}_${ARCH}.deb"
