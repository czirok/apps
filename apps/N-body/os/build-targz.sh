#!/bin/bash
set -e

source project.env

ARCH="x86_64"
BUILD_DIR="./targz"
PACKAGE_DIR="$BUILD_DIR/$APP_NAME-$VERSION"

echo -e "Building ${YELLOW}$APP_NAME-$VERSION-$ARCH.tar.gz${NC}"
echo ""
echo -e "App Name: ${YELLOW}$APP_NAME${NC}"
echo -e "C# Name: ${YELLOW}$CSHARP_NAME${NC}"
echo -e "Version: ${YELLOW}$VERSION${NC}"
echo -e "Architecture: ${YELLOW}$ARCH${NC}"
echo -e "Build Directory: ${YELLOW}$BUILD_DIR${NC}"
echo -e "Publish Directory: ${YELLOW}$PUBLISH_DIR${NC}"
echo -e "Package Directory: ${YELLOW}$PACKAGE_DIR${NC}"
echo ""

# Clean and create build structure
echo "Cleaning and creating build structure..."
rm -rf "$BUILD_DIR"
mkdir -p "$PACKAGE_DIR"
echo -e " ${GREEN}[✓]${NC} Created package directory: $PACKAGE_DIR"
echo ""

echo "Copying application files..."
# Copy files directly to package root
cp "$PUBLISH_DIR/$CSHARP_NAME" "$PACKAGE_DIR/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Copied executable: $CSHARP_NAME -> $APP_NAME"

cp "$PUBLISH_DIR/libSkiaSharp.so" "$PACKAGE_DIR/libSkiaSharp.so"
echo -e " ${GREEN}[✓]${NC} Copied library: libSkiaSharp.so"

cp "$PUBLISH_DIR/appsettings.json" "$PACKAGE_DIR/appsettings.json"
echo -e " ${GREEN}[✓]${NC} Copied config: appsettings.json"

cp "$PUBLISH_DIR/LICENSE" "$PACKAGE_DIR/LICENSE"
echo -e " ${GREEN}[✓]${NC} Copied license: LICENSE"

# Copy I18N directory structure
cp -r "$PUBLISH_DIR/I18N" "$PACKAGE_DIR/"
echo -e " ${GREEN}[✓]${NC} Copied localization: I18N/ directory"
echo ""

echo "Copying desktop integration files..."
# Copy desktop integration files
cp $APP_NAME.desktop "$PACKAGE_DIR/org.gnome.$APP_NAME.desktop"
echo -e " ${GREEN}[✓]${NC} Copied desktop file: $APP_NAME.desktop -> org.gnome.$APP_NAME.desktop"

cp $APP_NAME.svg "$PACKAGE_DIR/org.gnome.$APP_NAME.svg"
echo -e " ${GREEN}[✓]${NC} Copied icon: $APP_NAME.svg -> org.gnome.$APP_NAME.svg"

cp org.gnome.$APP_NAME.gschema.xml "$PACKAGE_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " ${GREEN}[✓]${NC} Copied schema: org.gnome.$APP_NAME.gschema.xml"
echo ""

echo "Creating install script..."
# Create desktop integration install script
cat > "$PACKAGE_DIR/install-to-home.sh" << EOF
#!/bin/bash

CURRENT_DIR="\$(pwd)"
DESKTOP_DIR="\$HOME/.local/share/applications"
ICON_DIR="\$HOME/.local/share/icons/hicolor/scalable/apps"
SCHEMA_DIR="\$HOME/.local/share/glib-2.0/schemas"

GREEN='\033[1;32m'
RED='\033[1;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "Installing \${YELLOW}$APP_NAME\${NC} desktop integration"
echo ""
echo -e "Current directory: \${YELLOW}\$CURRENT_DIR\${NC}"
echo -e "Desktop files: \${YELLOW}\$DESKTOP_DIR\${NC}"
echo -e "Icon directory: \${YELLOW}\$ICON_DIR\${NC}"
echo -e "Schema directory: \${YELLOW}\$SCHEMA_DIR\${NC}"
echo ""

echo "Creating directories..."
# Create directories
mkdir -p "\$DESKTOP_DIR"
echo -e " \${GREEN}[✓]\${NC} Created desktop directory: \$YELLOW\$DESKTOP_DIR\${NC}"

mkdir -p "\$ICON_DIR"
echo -e " \${GREEN}[✓]\${NC} Created icon directory: \$YELLOW\$ICON_DIR\${NC}"

mkdir -p "\$SCHEMA_DIR"
echo -e " \${GREEN}[✓]\${NC} Created schema directory: \$YELLOW\$SCHEMA_DIR\${NC}"
echo ""

echo "Installing files..."
# Update desktop file with current path
echo "Processing desktop file..."
sed "s|Exec=$APP_NAME|Exec=\$CURRENT_DIR/$APP_NAME|g" org.gnome.$APP_NAME.desktop > "\$DESKTOP_DIR/org.gnome.$APP_NAME.desktop"
echo -e " \${GREEN}[✓]\${NC} Installed desktop file: \$YELLOW\$DESKTOP_DIR/org.gnome.$APP_NAME.desktop\${NC}"
echo -e "  Executable path set to: \$YELLOW\$CURRENT_DIR/$APP_NAME\${NC}"

# Copy schema
cp org.gnome.$APP_NAME.gschema.xml "\$SCHEMA_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " \${GREEN}[✓]\${NC} Installed schema: \$YELLOW\$SCHEMA_DIR/org.gnome.$APP_NAME.gschema.xml\${NC}"

# Copy icon
cp org.gnome.$APP_NAME.svg "\$ICON_DIR/org.gnome.$APP_NAME.svg"
echo -e " \${GREEN}[✓]\${NC} Installed icon: \$YELLOW\$ICON_DIR/org.gnome.$APP_NAME.svg\${NC}"
echo ""

echo "Updating system databases..."
# Update desktop database and icon cache
if command -v update-desktop-database >/dev/null 2>&1; then
    echo "Updating desktop database..."
    update-desktop-database "\$DESKTOP_DIR"
    echo -e " \${GREEN}[✓]\${NC} Desktop database updated"
else
    echo -e " \${ERROR}[X]\${NC} update-desktop-database not found, skipping"
fi

if command -v gtk-update-icon-cache >/dev/null 2>&1; then
    echo "Updating icon cache..."
    gtk-update-icon-cache -q -t -f "\$HOME/.local/share/icons/hicolor"
    echo -e " \${GREEN}[✓]\${NC} Icon cache updated"
else
    echo -e " \${ERROR}[X]\${NC} gtk-update-icon-cache not found, skipping"
fi

if command -v glib-compile-schemas >/dev/null 2>&1; then
    echo "Compiling GSettings schemas..."
    glib-compile-schemas "\$SCHEMA_DIR"
    echo -e " \${GREEN}[✓]\${NC} GSettings schemas compiled"
else
    echo -e " \${ERROR}[X]\${NC} glib-compile-schemas not found, skipping"
fi

echo ""
echo "Desktop integration installed successfully!"
echo -e "\$YELLOW$APP_NAME\$NC should now appear in your application menu."
EOF

echo "Creating uninstall script..."
# Create desktop integration uninstall script
cat > "$PACKAGE_DIR/uninstall-from-home.sh" <<EOF
#!/bin/bash

DESKTOP_DIR="\$HOME/.local/share/applications"
ICON_DIR="\$HOME/.local/share/icons/hicolor/scalable/apps"
SCHEMA_DIR="\$HOME/.local/share/glib-2.0/schemas"

GREEN='\033[1;32m'
RED='\033[1;31m'
YELLOW='\033[1;33m'
NC='\033[0m'

echo -e "Uninstalling \${YELLOW}$APP_NAME\${NC} desktop integration"
echo ""
echo -e "Desktop files: \${YELLOW}\$DESKTOP_DIR\${NC}"
echo -e "Icon directory: \${YELLOW}\$ICON_DIR\${NC}"
echo -e "Schema directory: \${YELLOW}\$SCHEMA_DIR\${NC}"
echo ""

echo "Removing files..."
# Remove files
rm -f "\$DESKTOP_DIR/org.gnome.$APP_NAME.desktop"
echo -e " \${GREEN}[✓]\${NC} Removed desktop file: \$YELLOW\$DESKTOP_DIR/org.gnome.$APP_NAME.desktop\${NC}"

rm -f "\$ICON_DIR/org.gnome.$APP_NAME.svg"
echo -e " \${GREEN}[✓]\${NC} Removed icon: \$YELLOW\$ICON_DIR/org.gnome.$APP_NAME.svg\${NC}"

rm -f "\$SCHEMA_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " \${GREEN}[✓]\${NC} Removed schema: \$YELLOW\$SCHEMA_DIR/org.gnome.$APP_NAME.gschema.xml\${NC}"
echo ""

echo "Updating system databases..."
# Update desktop database and icon cache
if command -v update-desktop-database >/dev/null 2>&1; then
    echo "Updating desktop database..."
    update-desktop-database "\$DESKTOP_DIR"
    echo -e " \${GREEN}[✓]\${NC} Desktop database updated"
else
    echo -e " \${ERROR}[X]\${NC} update-desktop-database not found, skipping"
fi

if command -v gtk-update-icon-cache >/dev/null 2>&1; then
    echo "Updating icon cache..."
    gtk-update-icon-cache -q -t -f "\$HOME/.local/share/icons/hicolor"
    echo -e " \${GREEN}[✓]\${NC} Icon cache updated"
else
    echo -e " \${ERROR}[X]\${NC} gtk-update-icon-cache not found, skipping"
fi

if command -v glib-compile-schemas >/dev/null 2>&1; then
    echo "Compiling GSettings schemas..."
    glib-compile-schemas "\$SCHEMA_DIR"
    echo -e " \${GREEN}[✓]\${NC} GSettings schemas compiled"
else
    echo -e " \${ERROR}[X]\${NC} glib-compile-schemas not found, skipping"
fi

echo ""
echo "Desktop integration removed successfully!"
EOF

# Make scripts executable
chmod +x "$PACKAGE_DIR/install-to-home.sh"
echo -e " ${GREEN}[✓]${NC} Created install script: install-to-home.sh"

chmod +x "$PACKAGE_DIR/uninstall-from-home.sh"
echo -e " ${GREEN}[✓]${NC} Created uninstall script: uninstall-from-home.sh"
echo ""

echo "Creating tar.gz package..."
# Create the tar.gz package
cd "$BUILD_DIR"
tar -czf "$APP_NAME-$VERSION-$ARCH.tar.gz" "$APP_NAME-$VERSION/"

echo ""
echo "Package created successfully!"
echo ""
echo "Package: $BUILD_DIR/$APP_NAME-$VERSION-$ARCH.tar.gz"
echo "Size: $(du -h "$APP_NAME-$VERSION-$ARCH.tar.gz" | cut -f1)"
echo ""

# Show contents
echo "Package contents:"
echo ""
tar -tzf "$APP_NAME-$VERSION-$ARCH.tar.gz" | sort

echo ""
echo "Installation instructions:"
echo ""
echo -e "1. Extract: ${YELLOW}tar -xzf ${GREEN}$APP_NAME-$VERSION-$ARCH.tar.gz${NC}"
echo -e "2. Enter directory: ${YELLOW}cd ${GREEN}$APP_NAME-$VERSION/${NC}"
echo -e "3. Install desktop integration: ${YELLOW}./install-to-home.sh${NC}"
echo -e "4. Run application: ${YELLOW}./$APP_NAME${NC}"
echo ""
echo -e "To uninstall desktop integration: ${YELLOW}./uninstall-from-home.sh${NC}"