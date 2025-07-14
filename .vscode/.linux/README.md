# Gir.Core Linux development environment

## Install .NET

```bash
rm $APPSLINUX/dotnet-install.sh
wget https://dot.net/v1/dotnet-install.sh -O $APPSLINUX/dotnet-install.sh
chmod +x $APPSLINUX/dotnet-install.sh
```

### .NET 10 SDK

```bash
$APPSLINUX/dotnet-install.sh --channel 10.0 --architecture x64 --os linux --install-dir $APPSLINUX/.dotnet
```

### .NET 8 Runtime

```bash
$APPSLINUX/dotnet-install.sh --channel 8.0 --runtime dotnet --architecture x64 --os linux --install-dir $APPSLINUX/.dotnet
$APPSLINUX/dotnet-install.sh --channel 8.0 --runtime aspnetcore --architecture x64 --os linux --install-dir $APPSLINUX/.dotnet

```

### .NET 9 Runtime

```bash
$APPSLINUX/dotnet-install.sh --channel 9.0 --runtime dotnet --architecture x64 --os linux --install-dir $APPSLINUX/.dotnet
$APPSLINUX/dotnet-install.sh --channel 9.0 --runtime aspnetcore --architecture x64 --os linux --install-dir $APPSLINUX/.dotnet
```

## Manage packages

### Install dotnet-outdated-tool

```bash
# https://github.com/dotnet-outdated/dotnet-outdated

dotnet tool install dotnet-outdated-tool --tool-path $APPSLINUX/.dotnet

# Update

dotnet tool update dotnet-outdated-tool --tool-path $APPSLINUX/.dotnet
```

### List outdated packages

```bash
dotnet outdated --maximum-version 8.0
dotnet outdated --maximum-version 9.0
dotnet outdated --maximum-version 10.0 --pre-release Always

```

## VS Code

### Fonted

After upgrade VS Code, you may need to change the permissions of the workbench files.

Fonted requires read and write permissions for the workbench files.

```bash
sudo chmod 666 /opt/visual-studio-code/resources/app/out/vs/code/electron-sandbox/workbench/*

# restore the original permissions
sudo chmod 644 /opt/visual-studio-code/resources/app/out/vs/code/electron-sandbox/workbench/*
```

## oh my pos

```bash
curl -s https://ohmyposh.dev/install.sh | bash -s -- -d "$APPSLINUX"

## fnm

curl -fsSL https://fnm.vercel.app/install | bash -s -- --install-dir "$APPSLINUX/.local/fnm" --skip-shell

curl -fsSL https://fnm.vercel.app/install | bash
```

## pnpm

```bash
curl -fsSL https://get.pnpm.io/install.sh | sh -s -- -g --prefix "$APPSLINUX/.local/pnpm"
```

## Certificates

### <https://localhost:54320>

```bash
# Install the tool

dotnet tool install linux-dev-certs --tool-path $APPSLINUX/.dotnet

dotnet tool install --global linux-dev-certs
# Update the tool
dotnet tool update --global linux-dev-certs

dotnet linux-dev-certs install

mkdir -p $HOME/.pki/nssdb
certutil -d sql:$HOME/.pki/nssdb -A -t "C,," -n localhost -i /etc/ca-certificates/trust-source/anchors/aspnet-dev-$USER.crt

# then log out and log in again to apply the changes.
```

bash -c "cd $APPS && exec bash --rcfile $APPS/.vscode/.linux/.bashrc"

npm install --global corepack@latest
