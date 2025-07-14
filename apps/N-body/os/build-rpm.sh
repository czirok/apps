#!/bin/bash
set -e

APP_NAME="nbody"
VERSION=$(cat VERSION)
BUILD_DIR="./rpm"
PUBLISH_DIR="../publish"

# Clean and create build structure
rm -rf "$BUILD_DIR"
mkdir -p "$BUILD_DIR"/{BUILD,RPMS,SOURCES,SPECS,SRPMS}

# Create source tarball
TEMP_SOURCE_DIR="$BUILD_DIR/${APP_NAME}-${VERSION}"
mkdir -p "$TEMP_SOURCE_DIR"

# Copy all source files
cp "$PUBLISH_DIR/LICENSE" "$TEMP_SOURCE_DIR/LICENSE"
cp "$PUBLISH_DIR/NBody" "$TEMP_SOURCE_DIR/nbody"
cp "$PUBLISH_DIR/libSkiaSharp.so" "$TEMP_SOURCE_DIR/libSkiaSharp.so"
cp "$PUBLISH_DIR/appsettings.json" "$TEMP_SOURCE_DIR/appsettings.json"
cp nbody.desktop "$TEMP_SOURCE_DIR/nbody.desktop"
cp nbody.svg "$TEMP_SOURCE_DIR/nbody.svg"
cp org.gnome.nbody.gschema.xml "$TEMP_SOURCE_DIR/org.gnome.nbody.gschema.xml"

# Create I18N tar.gz in the source directory
tar -czf "$TEMP_SOURCE_DIR/I18N.tar.gz" -C "$PUBLISH_DIR" "I18N/"

# Create source tarball
cd "$BUILD_DIR"
tar -czf "SOURCES/${APP_NAME}-${VERSION}.tar.gz" "${APP_NAME}-${VERSION}/"

# Copy spec file
cp "../nbody.spec" "SPECS/nbody.spec"

# Build RPM
rpmbuild --define "_topdir $(pwd)" -ba "SPECS/nbody.spec"

echo "RPM packages created in:"
echo "  RPMS/x86_64/${APP_NAME}-${VERSION}-1.*.x86_64.rpm"
echo "  SRPMS/${APP_NAME}-${VERSION}-1.*.src.rpm"