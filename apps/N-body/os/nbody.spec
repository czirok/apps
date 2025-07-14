Name:           nbody
Version:        1.2.0
Release:        1%{?dist}
Summary:        N-body gravitational simulation
License:        CC BY 4.0
URL:            https://github.com/czirok/apps/
Source0:        %{name}-%{version}.tar.gz

BuildRequires:  desktop-file-utils

# openSUSE dependencies
%if 0%{?suse_version}
Requires:       libcairo2
Requires:       libadwaita-1-0
Requires:       libglib-2_0-0
Requires:       gnome-shell >= 48
%endif

# Fedora/RHEL dependencies
%if 0%{?fedora} || 0%{?rhel}
Requires:       cairo
Requires:       libadwaita
Requires:       gdk-pixbuf2
Requires:       glib2
Requires:       gnome-shell >= 48
%endif

%description
Simulate multiple interacting bodies in a dynamic gravitational field.
An educational tool for physics simulation and visualization.

%prep
%setup -q

%build
# Nothing to build - precompiled binary

%install
rm -rf %{buildroot}

# Create directories
mkdir -p %{buildroot}%{_datadir}/nbody/I18N
mkdir -p %{buildroot}%{_bindir}
mkdir -p %{buildroot}%{_datadir}/applications
mkdir -p %{buildroot}%{_datadir}/icons/hicolor/scalable/apps
mkdir -p %{buildroot}%{_datadir}/glib-2.0/schemas

# Install license
install -Dm644 LICENSE %{buildroot}%{_datadir}/nbody/LICENSE

# Install executable
install -Dm755 nbody %{buildroot}%{_datadir}/nbody/nbody

# Install library
install -Dm644 libSkiaSharp.so %{buildroot}%{_datadir}/nbody/libSkiaSharp.so

# Install configuration
install -Dm644 appsettings.json %{buildroot}%{_datadir}/nbody/appsettings.json

# Install I18N files
tar -xzf I18N.tar.gz -C %{buildroot}%{_datadir}/nbody/

# Install desktop file
install -Dm644 nbody.desktop %{buildroot}%{_datadir}/applications/org.gnome.nbody.desktop

# Install icon
install -Dm644 nbody.svg %{buildroot}%{_datadir}/icons/hicolor/scalable/apps/org.gnome.nbody.svg

# Install GSettings schema
install -Dm644 org.gnome.nbody.gschema.xml %{buildroot}%{_datadir}/glib-2.0/schemas/org.gnome.nbody.gschema.xml

# Create wrapper script
cat > %{buildroot}%{_bindir}/nbody << 'EOF'
#!/bin/bash
exec %{_datadir}/nbody/nbody "$@"
EOF
chmod 755 %{buildroot}%{_bindir}/nbody

%check
desktop-file-validate %{buildroot}%{_datadir}/applications/org.gnome.nbody.desktop

%post
/bin/touch --no-create %{_datadir}/icons/hicolor &>/dev/null || :
/usr/bin/glib-compile-schemas %{_datadir}/glib-2.0/schemas &> /dev/null || :

%postun
if [ $1 -eq 0 ] ; then
    /bin/touch --no-create %{_datadir}/icons/hicolor &>/dev/null
    /usr/bin/gtk-update-icon-cache %{_datadir}/icons/hicolor &>/dev/null || :
fi
/usr/bin/glib-compile-schemas %{_datadir}/glib-2.0/schemas &> /dev/null || :

%posttrans
/usr/bin/gtk-update-icon-cache %{_datadir}/icons/hicolor &>/dev/null || :

%files
%{_bindir}/nbody
%{_datadir}/nbody/
%{_datadir}/applications/org.gnome.nbody.desktop
%{_datadir}/icons/hicolor/scalable/apps/org.gnome.nbody.svg
%{_datadir}/glib-2.0/schemas/org.gnome.nbody.gschema.xml

# Disable debuginfo package creation
%global debug_package %{nil}
