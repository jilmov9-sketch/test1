# 3D Tank Battle Game

A modern 3D tank combat game built with Unity, featuring cross-platform input support, optimized performance, and engaging gameplay mechanics.

![Build Status](https://github.com/your-username/3d-tank-battle/actions/workflows/build.yml/badge.svg?branch=main)

## 🎮 Features

### Core Gameplay
- **360° Turret Aiming**: Independent turret rotation with mouse/controller aim
- **Physics-based Movement**: Smooth tank movement using Unity physics
- **Object Pooling**: Optimized bullet system with zero instantiation overhead
- **Multi-platform Input**: Seamless keyboard/mouse and gamepad support

### Technical Highlights
- **Performance Optimized**: 60+ FPS target with efficient rendering
- **Cross-platform**: Windows, Linux, macOS support (configurable)
- **Accessible**: Configurable dead zones and sensitivity settings
- **Scalable**: Modular architecture for easy feature expansion

## 🚀 Quick Start

### Prerequisites
- Unity 2022.3 LTS or later
- Git

### Installation
```bash
# Clone the repository
git clone https://github.com/your-username/3d-tank-battle.git
cd 3d-tank-battle

# Open in Unity
# File → Open Project → Select this directory
```

### Controls
| Action | Keyboard/Mouse | Gamepad |
|--------|---------------|---------|
| Move | WASD | Left Stick |
| Aim/Turret | Mouse | Right Stick |
| Fire | Left Click | Right Trigger |
| Boost | Shift | A Button |

## 📁 Project Structure

```
Assets/
├── Scripts/
│   ├── Core/           # Core systems (Input, Object Pool)
│   ├── Gameplay/       # Game logic (Tank, Bullet)
│   ├── UI/            # User interface
│   └── Utils/         # Utilities
├── Scenes/            # Game scenes
├── Prefabs/           # Reusable objects
└── Materials/         # Visual materials
```

## 🔧 Configuration

### Input Settings
Configure in `InputManager.cs`:
- Mouse sensitivity
- Gamepad dead zone
- Key bindings

### Performance Settings
Adjust in Quality Settings:
- Shadow quality
- Anti-aliasing
- Draw distance

## 🏗️ Build & Deployment

This project includes automated CI/CD pipeline via GitHub Actions.

### Automatic Builds
Pushing to `main` branch triggers:
1. Unity project build
2. Windows 64-bit executable generation
3. Artifact upload to GitHub

### Creating Releases
```bash
# Tag a release version
git tag v1.0.0
git push origin v1.0.0
```

This will automatically:
- Build the game
- Create a GitHub Release
- Attach the executable

For detailed deployment instructions, see [DEPLOYMENT.md](DEPLOYMENT.md).

## 🛠️ Development

### Adding New Features
1. Create scripts in appropriate folder
2. Follow existing patterns (e.g., `TankController.cs`)
3. Update README with new features

### Testing
1. Open test scene in Unity
2. Press Play to test controls
3. Verify frame rate and performance

## 📄 License

MIT License - See LICENSE file for details

## 🤝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open Pull Request

## 📞 Support

- Issues: GitHub Issues tab
- Documentation: See DEPLOYMENT.md for setup help

---

**Built with Unity 2022.3 LTS** | **Target: 60+ FPS** | **Platforms: Windows, Linux, macOS**
