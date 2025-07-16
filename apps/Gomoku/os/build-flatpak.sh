#!/bin/bash
set -e

source project.env

ARCH="x86_64"
TEMP_DIR="./flatpak"

echo -e "Building Flatpak package for ${YELLOW}$APP_NAME-$VERSION${NC}"
echo ""

# Clean and create temp directory
rm -rf "$TEMP_DIR"
mkdir -p "$TEMP_DIR"
echo -e " ${GREEN}[✓]${NC} Created build directory: ${YELLOW}$TEMP_DIR${NC}"

echo ""
echo "Processing flatpak manifest..."
# Copy flatpak manifest template
sed -e "s/{{APP_NAME}}/$APP_NAME/g" \
    -e "s/{{VERSION}}/$VERSION/g" \
    -e "s|{{DESCRIPTION}}|$DESCRIPTION|g" \
    -e "s|{{LICENSE}}|$LICENSE|g" \
    -e "s|{{URL}}|$URL|g" \
    flatpak.yaml.template > "$TEMP_DIR/$APP_NAME.yaml"
echo -e " ${GREEN}[✓]${NC} Generated manifest: ${YELLOW}$APP_NAME.yaml${NC}"

echo ""
echo "Copying source files..."
# Copy license
cp "$PUBLISH_DIR/LICENSE" "$TEMP_DIR/LICENSE"
echo -e " ${GREEN}[✓]${NC} Copied license: ${YELLOW}LICENSE${NC}"

# Copy binary files
cp "$PUBLISH_DIR/$CSHARP_NAME" "$TEMP_DIR/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Copied executable: $CSHARP_NAME -> $APP_NAME"

cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_DIR/libSkiaSharp.so"
echo -e " ${GREEN}[✓]${NC} Copied library: ${YELLOW}libSkiaSharp.so${NC}"

# Copy configuration
cp "$PUBLISH_DIR/appsettings.json" "$TEMP_DIR/appsettings.json"
echo -e " ${GREEN}[✓]${NC} Copied config: ${YELLOW}appsettings.json${NC}"

# Create I18N archive
tar -czf "$TEMP_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" I18N
echo -e " ${GREEN}[✓]${NC} Created I18N archive: ${YELLOW}I18N.tar.gz${NC}"

# Copy desktop integration files
cp "$APP_NAME.desktop" "$TEMP_DIR/$APP_NAME.desktop"
echo -e " ${GREEN}[✓]${NC} Copied desktop file: ${YELLOW}$APP_NAME.desktop${NC}"

cp "$APP_NAME.svg" "$TEMP_DIR/$APP_NAME.svg"
echo -e " ${GREEN}[✓]${NC} Copied icon: ${YELLOW}$APP_NAME.svg${NC}"

cp "org.gnome.$APP_NAME.gschema.xml" "$TEMP_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " ${GREEN}[✓]${NC} Copied schema: ${YELLOW}org.gnome.$APP_NAME.gschema.xml${NC}"

echo ""
echo "Building Flatpak..."
cd "$TEMP_DIR"

# Build flatpak
echo "Building application..."
flatpak-builder --force-clean --repo=repo build-dir "$APP_NAME.yaml"
echo -e " ${GREEN}[✓]${NC} Built application in repo"

echo "Creating bundle..."
flatpak build-bundle repo "$APP_NAME-${VERSION}-${ARCH}.flatpak" "org.gnome.$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Created bundle: ${YELLOW}$APP_NAME-${VERSION}-${ARCH}.flatpak${NC}"

echo ""
echo -e "${GREEN}Flatpak package built successfully!${NC}"
echo ""
echo -e "Package: ${YELLOW}$TEMP_DIR/$APP_NAME-${VERSION}-${ARCH}.flatpak${NC}"
echo -e "Size: ${YELLOW}$(du -h "$APP_NAME-${VERSION}-${ARCH}.flatpak" | cut -f1)${NC}"
echo ""
echo "Installation instructions:"
echo -e "  Install: ${YELLOW}flatpak install --user $APP_NAME-${VERSION}-${ARCH}.flatpak${NC}"
echo -e "  Run: ${YELLOW}flatpak run org.gnome.$APP_NAME${NC}"
echo -e "  Uninstall: ${YELLOW}flatpak uninstall org.gnome.$APP_NAME${NC}"