#!/bin/bash
set -e

source project.env

ARCH="amd64"
BUILD_DIR="./deb"
DEB_DIR="$BUILD_DIR/${APP_NAME}-deb"
LIBDIR="/usr/lib/$APP_NAME"

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
Package: $APP_NAME
Version: $VERSION
Section: utils
Priority: optional
Architecture: $ARCH
Maintainer: $MAINTAINER
Description: $DESCRIPTION
Depends: libcairo2, libadwaita-1-0, libgdk-pixbuf-2.0-0
License: $LICENSE
EOF

# Binary + files to the libdir
install -Dm755 "$PUBLISH_DIR/$CSHARP_NAME" "$DEB_DIR$LIBDIR/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Copied executable: $CSHARP_NAME -> $APP_NAME"

install -Dm644 "$PUBLISH_DIR/libSkiaSharp.so" "$DEB_DIR$LIBDIR/libSkiaSharp.so"
echo -e " ${GREEN}[✓]${NC} Copied library: ${YELLOW}libSkiaSharp.so${NC}"

install -Dm644 "$PUBLISH_DIR/appsettings.json" "$DEB_DIR$LIBDIR/appsettings.json"
echo -e " ${GREEN}[✓]${NC} Copied config: ${YELLOW}appsettings.json${NC}"

install -Dm644 "$PUBLISH_DIR/LICENSE" "$DEB_DIR$LIBDIR/LICENSE"
echo -e " ${GREEN}[✓]${NC} Copied license: ${YELLOW}LICENSE${NC}"

cp -r "$PUBLISH_DIR/I18N" "$DEB_DIR$LIBDIR/"
echo -e " ${GREEN}[✓]${NC} Copied ${YELLOW}I18N${NC} directory"

# Wrapper script in /usr/bin
cat > "$DEB_DIR/usr/bin/$APP_NAME" <<EOF
#!/bin/sh
exec "$LIBDIR/$APP_NAME"
EOF
chmod +x "$DEB_DIR/usr/bin/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Created wrapper script: ${YELLOW}$APP_NAME${NC}"

# Desktop + schema + icon
install -Dm644 "$APP_NAME.desktop" "$DEB_DIR/usr/share/applications/org.gnome.$APP_NAME.desktop"
echo -e " ${GREEN}[✓]${NC} Copied desktop file: ${YELLOW}$APP_NAME.desktop${NC}"

install -Dm644 "$APP_NAME.svg" "$DEB_DIR/usr/share/icons/hicolor/scalable/apps/org.gnome.$APP_NAME.svg"
echo -e " ${GREEN}[✓]${NC} Copied icon: ${YELLOW}$APP_NAME.svg${NC}"

install -Dm644 "org.gnome.$APP_NAME.gschema.xml" "$DEB_DIR/usr/share/glib-2.0/schemas/org.gnome.$APP_NAME.gschema.xml"
echo -e " ${GREEN}[✓]${NC} Copied schema: ${YELLOW}org.gnome.$APP_NAME.gschema.xml${NC}"

# Package build
dpkg-deb --root-owner-group --build "$DEB_DIR" "$BUILD_DIR/${APP_NAME}_${VERSION}_${ARCH}.deb"
echo ""
echo -e "${GREEN}Debian package built successfully!${NC}"
echo -e "Package location: ${YELLOW}$BUILD_DIR/${APP_NAME}_${VERSION}_${ARCH}.deb${NC}"
