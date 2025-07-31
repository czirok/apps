#!/bin/bash

# WSL Arch Linux Setup Script

# Configuration Variables
TIMEZONE="Europe/Budapest"
LOCALE_PRIMARY="hu_HU.UTF-8"
USERNAME="username"
LOCALE_FALLBACK="en_US.UTF-8"

# Colors
GREEN='\033[1;32m'
RED='\033[1;31m'
YELLOW='\033[1;33m'
BLUE='\033[1;34m'
CYAN='\033[1;36m'
NC='\033[0m' # No Color

# Helper Functions
print_step() {
    echo -e "${CYAN}[INFO]${NC} $1"
}

print_success() {
    echo -e "${GREEN}[✓]${NC} $1"
}

print_warning() {
    echo -e "${YELLOW}[!]${NC} $1"
}

print_error() {
    echo -e "${RED}[✗]${NC} $1"
}

print_header() {
    echo -e "${BLUE}"
    echo -e " █████╗ ██████╗  ██████╗██╗  ██╗    ██╗     ██╗███╗   ██╗██╗   ██╗██╗  ██╗"
    echo -e "██╔══██╗██╔══██╗██╔════╝██║  ██║    ██║     ██║████╗  ██║██║   ██║╚██╗██╔╝"
    echo -e "███████║██████╔╝██║     ███████║    ██║     ██║██╔██╗ ██║██║   ██║ ╚███╔╝ "
    echo -e "██╔══██║██╔══██╗██║     ██╔══██║    ██║     ██║██║╚██╗██║██║   ██║ ██╔██╗ "
    echo -e "██║  ██║██║  ██║╚██████╗██║  ██║    ███████╗██║██║ ╚████║╚██████╔╝██╔╝ ██╗"
    echo -e "╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝    ╚══════╝╚═╝╚═╝  ╚═══╝ ╚═════╝ ╚═╝  ╚═╝"
    echo -e "${NC}"
}

# Main Setup Functions
update_system() {
    print_step "Updating system packages..."
    pacman -Syu --noconfirm
    print_success "System updated"
}

install_packages() {
    print_step "Installing essential packages..."
    pacman -S --noconfirm sudo gnome-control-center ttf-dejavu pipewire-jack fastfetch
    print_success "Packages installed"
}

setup_locale() {
    print_step "Setting timezone: $TIMEZONE"
    ln -sf /usr/share/zoneinfo/$TIMEZONE /etc/localtime
    hwclock --systohc
    print_success "Timezone configured"

    print_step "Generating locales..."
    sed -i "s/^#\($LOCALE_PRIMARY UTF-8\)/\1/" /etc/locale.gen
    sed -i "s/^#\($LOCALE_FALLBACK UTF-8\)/\1/" /etc/locale.gen
    locale-gen
    echo "LANG=$LOCALE_PRIMARY" > /etc/locale.conf
    print_success "Locale configured: $LOCALE_PRIMARY"
}

setup_user_skeleton() {
    print_step "Setting up user skeleton (skel)..."
    mkdir -p /etc/skel/.config
    echo "LANG=$LOCALE_PRIMARY" > /etc/skel/.config/locale.conf
    echo 'source ~/.config/locale.conf' >> /etc/skel/.bashrc
    print_success "Skel configured with WSL locale fix"
}

create_user() {
    print_step "Creating user: $USERNAME"
    useradd -m -G wheel -s /bin/bash $USERNAME
    print_success "User created"
    
    print_warning "Password setup required:"
    passwd $USERNAME
}

setup_sudo() {
    print_step "Enabling sudo privileges..."
    sed -i 's/^# %wheel ALL=(ALL:ALL) ALL/%wheel ALL=(ALL:ALL) ALL/' /etc/sudoers
    print_success "Sudo enabled for wheel group"
}

setup_wsl_config() {
    print_step "Configuring WSL settings..."
    echo -e "[user]\ndefault=$USERNAME" >> /etc/wsl.conf
    print_success "WSL default user set: $USERNAME"
}

# Main Script
main() {
    print_header

    print_step "Configuration:"
    echo -e " Timezone: ${YELLOW}$TIMEZONE${NC}"
    echo -e " Locale:   ${YELLOW}$LOCALE_PRIMARY${NC}"
    echo -e " Username: ${YELLOW}$USERNAME${NC}"
    echo

    update_system
    install_packages
    setup_locale
    setup_user_skeleton
    create_user
    setup_sudo
    setup_wsl_config

    print_success "WSL Arch Linux setup completed!"
    print_warning "Restart WSL session to apply changes:"
    echo -e "${CYAN}wsl --terminate archlinux${NC}"
    echo -e "${CYAN}wsl -d archlinux${NC}"
    
    print_step "Exiting..."
    exit
}

# Script futtatása
main "$@"