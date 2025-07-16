#!/bin/bash
set -e

source project.env

BUILD_DIR="./rpm"

echo -e "Building RPM package for ${YELLOW}$APP_NAME-$VERSION${NC}"
echo ""

# Clean and create build structure
rm -rf "$BUILD_DIR"
mkdir -p "$BUILD_DIR"/{BUILD,RPMS,SOURCES,SPECS,SRPMS}
echo -e " ${GREEN}[✓]${NC} Cleaned build directory: ${YELLOW}$BUILD_DIR${NC}"

# Create source tarball
TEMP_SOURCE_DIR="$BUILD_DIR/${APP_NAME}-${VERSION}"
mkdir -p "$TEMP_SOURCE_DIR"
echo -e " ${GREEN}[✓]${NC} Created temporary source directory: ${YELLOW}$TEMP_SOURCE_DIR${NC}"

echo ""
echo "Copying source files..."
# Copy all source files
cp "$PUBLISH_DIR/LICENSE" "$TEMP_SOURCE_DIR/LICENSE"
echo -e " ${GREEN}[✓]${NC} Copied license: ${YELLOW}LICENSE${NC}"

cp "$PUBLISH_DIR/$CSHARP_NAME" "$TEMP_SOURCE_DIR/$APP_NAME"
echo -e " ${GREEN}[✓]${NC} Copied executable: $CSHARP_NAME -> $APP_NAME"

cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_SOURCE_DIR/libSkiaSharp.so"
echo -e " ${GREEN}[✓]${NC} Copied library: ${YELLOW}libSkiaSharp.so${NC}"

cp "$PUBLISH_DIR/appsettings.json" "$TEMP_SOURCE_DIR/appsettings.json"
echo -e " ${GREEN}[✓]${NC} Copied config: ${YELLOW}appsettings.json${NC}"

cp "$APP_NAME.desktop" "$TEMP_SOURCE_DIR/$APP_NAME.desktop"
echo -e " ${GREEN}[✓]${NC} Copied desktop file: ${YELLOW}$APP_NAME.desktop${NC}"

cp "$APP_NAME.svg" "$TEMP_SOURCE_DIR/$APP_NAME.svg"
echo -e " ${GREEN}[✓]${NC} Copied icon: ${YELLOW}$APP_NAME.svg${NC}"

cp "org.gnome.$APP_NAME.gschema.xml" "$TEMP_SOURCE_DIR/org.gnome.$APP_NAME.gschema.xml"
echo -e " ${GREEN}[✓]${NC} Copied schema: ${YELLOW}org.gnome.$APP_NAME.gschema.xml${NC}"

# Create I18N tar.gz in the source directory
tar -czf "$TEMP_SOURCE_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" "I18N/"
echo -e " ${GREEN}[✓]${NC} Created I18N tarball: ${YELLOW}I18N.tar.gz${NC}"

echo ""
echo "Processing spec file..."
# Process spec template
sed -e "s|{{APP_NAME}}|$APP_NAME|g" \
    -e "s|{{VERSION}}|$VERSION|g" \
    -e "s|{{DESCRIPTION}}|$DESCRIPTION|g" \
    -e "s|{{LICENSE}}|$LICENSE|g" \
    -e "s|{{URL}}|$URL|g" \
    "rpm.spec.template" > "$BUILD_DIR/SPECS/$APP_NAME.spec"
echo -e " ${GREEN}[✓]${NC} Generated spec file from template"

echo ""
echo "Creating source tarball..."
# Create source tarball
cd "$BUILD_DIR"
tar -czf "SOURCES/${APP_NAME}-${VERSION}.tar.gz" "${APP_NAME}-${VERSION}/"
echo -e " ${GREEN}[✓]${NC} Created source tarball: ${YELLOW}SOURCES/${APP_NAME}-${VERSION}.tar.gz${NC}"

echo ""
echo "Building RPM packages..."
# Build RPM
rpmbuild --define "_topdir $(pwd)" -ba "SPECS/$APP_NAME.spec"

echo ""
echo -e "${GREEN}RPM packages built successfully!${NC}"
echo ""
echo "Packages created:"
echo -e "  Binary: ${YELLOW}RPMS/x86_64/${APP_NAME}-${VERSION}-1.*.x86_64.rpm${NC}"
echo -e "  Source: ${YELLOW}SRPMS/${APP_NAME}-${VERSION}-1.*.src.rpm${NC}"

# Show actual files created
echo ""
echo "Actual files:"
ls -la "RPMS/x86_64/${APP_NAME}"-*.rpm 2>/dev/null || echo "  No binary RPMs found"
ls -la "SRPMS/${APP_NAME}"-*.rpm 2>/dev/null || echo "  No source RPMs found"