# WSL2 Setup Guide

## Install

Open a **PowerShell** window with **administrative privileges** and run the following commands to **enable WSL**

```powershell
dism.exe /online /enable-feature /featurename:Microsoft-Windows-Subsystem-Linux /all /norestart
```

and **enable Virtual Machine Platform** features

```powershell
dism.exe /online /enable-feature /featurename:VirtualMachinePlatform /all /norestart
```

then **restart** to apply changes

```powershell
Restart-Computer
```

## Setup

**After restart**, open **PowerShell** and run the following command to **update** the **WSL**

```powershell
wsl --update
```

and set **WSL2** default **version** to **2**

```powershell
wsl --set-default-version 2
```

## Install Arch Linux

Arch Linux is **fast**, installs **minimal packages** far fewer than other distributions, has the **fastest package manager** and provides a **clean**, **lightweight system** perfect for development work.

```powershell
wsl --install archlinux
```

## Configure Arch Linux

### Update and install minimal packages

```bash
cd ~ && pacman -Syu --noconfirm && pacman -S --noconfirm wget nano less
```

### Download the setup script

```bash
wget https://raw.githubusercontent.com/czirok/apps/main/docs/archlinux.sh -O archlinux.sh
```

### Make the script executable

```bash
chmod +x archlinux.sh
```

### Edit `archlinux.sh` before running

Open the `archlinux.sh` file in a text editor `nano archlinux.sh`, and modify the following variables to suit your preferences:

```bash
TIMEZONE="Europe/Budapest"
LOCALE_PRIMARY="hu_HU.UTF-8"
USERNAME="username"
```

TIMEZONE is a directory under `/usr/share/zoneinfo/`, you can find your timezone by running:

`ls /usr/share/zoneinfo/`

LOCALE_PRIMARY is the locale you want to set as default, it should be available in `/etc/locale.gen`. You can check available locales by running:

`cat /etc/locale.gen | less`

### Run the script

Set password for the user when prompted.

```bash
./archlinux.sh

exit
```

## Restart WSL to apply user settings

```powershell
wsl --terminate archlinux
wsl --shutdown
wsl -d archlinux
```

## GNOME Control Center

You can change the theme, dark or light mode, and other settings using the GNOME Control Center. Run the following command to open it:

```bash
XDG_CURRENT_DESKTOP=GNOME gnome-control-center
```

## Install AUR (optional)

```bash
sudo pacman -S --noconfirm git base-devel

git clone https://aur.archlinux.org/yay.git

cd yay

makepkg -si --noconfirm
```

## pacman help

### Update OS

```bash
sudo pacman -Syu
```

### List installed packages

```bash
pacman -Q
```

### Search packages

```bash
pacman -Ss package
```

### Search installed packages

```bash
pacman -Qs package
```

### Install packages

```bash
sudo pacman -S package
```

### Remove packages

```bash
sudo pacman -R package
```

### Remove orphaned packages

```bash
sudo pacman -Rns $(pacman -Qdtq)
```
