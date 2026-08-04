# 3D Tank Battle Game

A modern 3D remake of the classic Tank Battle game with enhanced playability, smooth performance, and universal accessibility.

## Project Overview

- **Core Gameplay**: 3D tank combat with customizable tanks and dynamic environments
- **Target Platforms**: PC (Windows/Mac/Linux), with potential for Web and Mobile expansion
- **Game Engine**: Unity 2022 LTS with HDRP (High Definition Render Pipeline)
- **Development Status**: Phase 1 - Prototype

## Features

### Universality (普适性)
- Multi-platform input support (Keyboard, Gamepad, Touch)
- Resolution adaptive UI (4:3, 16:9, 21:9)
- Performance scaling (Low/Medium/High/Ultra presets)
- Accessibility options (colorblind modes, customizable controls, subtitle settings)

### Smoothness (流畅性)
- Optimized rendering with LOD and occlusion culling
- Efficient physics with layered collision detection
- Object pooling for bullets and effects
- Memory management with minimal garbage collection
- Network synchronization with client-side prediction (for multiplayer)

### Playability (可玩性)
- Deep tank customization system (chassis, weapons, accessories)
- Multiple game modes (Campaign, Survival, Multiplayer, Challenges)
- Intelligent AI with behavior trees
- Dynamic environments with destructible terrain
- Progression and reward systems

## Project Structure

```
Assets/
├── Scripts/
│   ├── Core/          # Core systems (Input, ObjectPool, GameManager)
│   ├── Gameplay/      # Game logic (Tank, Weapon, AI)
│   ├── UI/            # User interface components
│   └── Utils/         # Utility functions and extensions
├── Prefabs/           # Reusable prefabs (Tanks, Bullets, Effects)
├── Scenes/            # Game scenes
├── Art/               # 3D models, textures, materials
└── Audio/             # Sound effects and music
```

## Getting Started

### Prerequisites
- Unity Hub 3.x
- Unity 2022.3 LTS or later
- Git (for version control)

### Installation
1. Clone this repository
2. Open the project in Unity Hub
3. Let Unity import all assets
4. Open the `Scenes/Prototype` scene to start

### Controls
- **WASD**: Move tank
- **Mouse**: Aim turret
- **Left Click**: Fire main weapon
- **Right Click**: Use special ability
- **Space**: Brake/Stop
- **R**: Reload (if applicable)

## Development Roadmap

- [x] Phase 1: Prototype (Weeks 1-4) - Core gameplay validation
- [ ] Phase 2: Core Systems (Weeks 5-12) - Alpha version
- [ ] Phase 3: Content & Polish (Weeks 13-22) - Beta version
- [ ] Phase 4: Testing & Release (Weeks 23-30) - Version 1.0

## Current Status

**Week 1**: Project initialization and basic tank movement prototype

## Contributing

This is a demonstration project. For actual development, please refer to the detailed plan in `3d_tank_battle_plan.md`.

## License

MIT License - See LICENSE file for details

## Acknowledgments

Inspired by classic Tank Battle games and modern titles like World of Tanks and Battlefield series.
