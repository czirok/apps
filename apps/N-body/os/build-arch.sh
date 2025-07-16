#!/bin/bash
set -e

source project.env

TEMP_DIR="./arch"

echo -e "Building Arch Linux package for ${YELLOW}$APP_NAME-$VERSION${NC}"
echo ""

mkdir -p "$TEMP_DIR"

echo "Copying files..."
# Copy license
cp "$PUBLISH_DIR/LICENSE" "$TEMP_DIR/LICENSE"
echo -e " ${GREEN}[✓]${NC} Copied license"

# Process PKGBUILD template
sed -e "s/{{APP_NAME}}/$APP_NAME/g" \
    -e "s/{{VERSION}}/$VERSION/g" \
    -e "s|{{DESCRIPTION}}|$DESCRIPTION|g" \
    -e "s|{{LICENSE}}|$LICENSE|g" \
    -e "s|{{URL}}|$URL|g" \
    PKGBUILD.template > "$TEMP_DIR/PKGBUILD"
echo -e " ${GREEN}[✓]${NC} Generated PKGBUILD from template"

# Copy binary files
cp "$PUBLISH_DIR/$CSHARP_NAME" "$TEMP_DIR/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Copied executable: $CSHARP_NAME -> $APP_NAME"

cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_DIR/libSkiaSharp.so"
echo -e " ${GREEN}[✓]${NC} Copied library: ${YELLOW}libSkiaSharp.so${NC}"

# Copy configuration
cp "$PUBLISH_DIR/appsettings.json" "$TEMP_DIR/appsettings.json"
echo -e " ${GREEN}[✓]${NC} Copied config: ${YELLOW}appsettings.json${NC}"

# Tar create for I18N
tar -czf "$TEMP_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" "I18N/"
echo -e " ${GREEN}[✓]${NC} Created ${YELLOW}I18N.tar.gz${NC} archive"

# Copy desktop files
cp "$APP_NAME.desktop" "$TEMP_DIR/$APP_NAME.desktop"
echo -e " ${GREEN}[✓]${NC} Copied desktop file: ${YELLOW}$APP_NAME.desktop${NC}"

cp "$APP_NAME.svg" "$TEMP_DIR/$APP_NAME.svg"
echo -e " ${GREEN}[✓]${NC} Copied icon: ${YELLOW}$APP_NAME.svg${NC}"

cp "org.gnome.$APP_NAME.gschema.xml" "$TEMP_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " ${GREEN}[✓]${NC} Copied schema: ${YELLOW}org.gnome.$APP_NAME.gschema.xml${NC}"

echo ""
echo "Building package..."
cd "$TEMP_DIR"
makepkg -f -C -s

echo ""
echo -e "${GREEN}Arch Linux package built successfully!${NC}"
echo -e "Package location: ${YELLOW}$TEMP_DIR/${NC}"