# 3D Tank Battle - Web Version

A fully functional 3D tank battle game built with pure HTML5, CSS3, JavaScript and Three.js. No Unity required!

## 🎮 Features

- **3D Graphics**: Powered by Three.js WebGL renderer
- **Smooth Controls**: Keyboard (WASD/Arrows), Mouse aiming, Gamepad support
- **Dynamic Gameplay**: Waves of enemies, increasing difficulty
- **Responsive UI**: Works on desktop and mobile browsers
- **No Installation**: Play directly in your browser

## 🚀 Quick Start

### Option 1: Play Online (After CI/CD)
Once pushed to GitHub, the game will automatically:
1. Build via GitHub Actions
2. Deploy to GitHub Pages
3. Available at: `https://jilmov9-sketch.github.io/test1/`

### Option 2: Local Testing
```bash
# Using Python
python3 -m http.server 8000

# Or using Node.js
npx http-server -p 8000

# Then open: http://localhost:8000
```

## 🎯 Controls

| Input | Action |
|-------|--------|
| W/↑ | Move Forward |
| S/↓ | Move Backward |
| A/← | Move Left |
| D/→ | Move Right |
| Mouse | Aim Turret |
| Left Click | Fire |
| Gamepad | Full Support |

## 📁 Project Structure

```
test1/
├── index.html          # Main HTML file
├── src/
│   ├── css/
│   │   └── style.css   # Game styles
│   ├── js/
│   │   └── game.js     # Game logic
│   └── assets/         # Future assets
└── .github/
    └── workflows/
        └── build.yml   # CI/CD pipeline
```

## 🔧 CI/CD Pipeline

The GitHub Actions workflow automatically:
1. ✅ Validates project files
2. 📦 Creates build artifact
3. 🚀 Deploys to GitHub Pages
4. 🏷️ Creates Release tags

### Manual Trigger
Go to Actions → "Build and Deploy WebGL" → "Run workflow"

## 🎨 Customization

### Change Tank Colors
Edit `src/js/game.js`:
```javascript
// Player tank (green)
color: 0x4ecca3

// Enemy tanks (red)
color: 0xff6b6b
```

### Adjust Difficulty
```javascript
this.enemySpawnInterval = 3000; // Spawn rate (ms)
enemy.health = 3;               // Enemy HP
this.health -= 10;              // Damage taken
```

## 🌐 Browser Compatibility

- ✅ Chrome/Edge (Recommended)
- ✅ Firefox
- ✅ Safari
- ✅ Mobile browsers (with touch controls fallback)

## 📝 License

MIT License - Feel free to modify and distribute!

## 🙏 Credits

- Three.js: https://threejs.org/
- Built with ❤️ for the web

---

**Enjoy the game!** 🎮✨
