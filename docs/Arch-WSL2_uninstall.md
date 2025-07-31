# WSL2 Complete Removal Guide

Open a **PowerShell** window with **administrative privileges** and run the following commands to **remove WSL**

## Terminate Arch Linux

```powershell
wsl --terminate archlinux
```

## Remove Arch Linux

```powershell
wsl --unregister archlinux
```

## Stop WSL2 Services

```powershell
wsl --shutdown
```

## Disable Windows Features

```powershell
dism.exe /online /disable-feature /featurename:Microsoft-Windows-Subsystem-Linux /norestart
```

## Disable Virtual Machine Platform features

```powershell
dism.exe /online /disable-feature /featurename:VirtualMachinePlatform /norestart
```

## Restart Windows to apply changes

```powershell
Restart-Computer
```
