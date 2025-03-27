# Dynamic Wallpaper Engine

Professional-grade wallpaper animation system for Windows 10/11 maded with AI

![Screenshot](resources/screenshot.png)

## Features
- 🚀 Hardware-accelerated rendering (DirectX 11)
- 🎨 Customizable effects system
- 🖥️ Multi-monitor support
- ⚡️ Low resource usage (<1% CPU in idle)

## System Requirements
- Windows 10/11 (64-bit)
- DirectX 11 compatible GPU
- 2GB VRAM recommended

## Build Instructions

### Prerequisites
- Visual Studio 2022
- Windows 10/11 SDK
- CMake 3.20+

### Steps
`powershell
git clone https://github.com/yourname/DynamicWallpaperEngine.git
cd DynamicWallpaperEngine
mkdir build
cd build
cmake .. -A x64
cmake --build . --config Release
