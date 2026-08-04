class TankBattleGame {
    constructor() {
        this.scene = null;
        this.camera = null;
        this.renderer = null;
        this.player = null;
        this.enemies = [];
        this.bullets = [];
        this.obstacles = [];
        this.score = 0;
        this.health = 100;
        this.wave = 1;
        this.isPlaying = false;
        this.lastTime = 0;
        this.enemySpawnTimer = 0;
        this.enemySpawnInterval = 3000;
        
        this.init();
        this.setupEventListeners();
        this.animate();
    }
    
    init() {
        // Scene setup
        this.scene = new THREE.Scene();
        this.scene.background = new THREE.Color(0x1a1a2e);
        this.scene.fog = new THREE.Fog(0x1a1a2e, 50, 150);
        
        // Camera setup
        this.camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 1000);
        this.camera.position.set(0, 30, 30);
        this.camera.lookAt(0, 0, 0);
        
        // Renderer setup
        const canvas = document.getElementById('game-canvas');
        this.renderer = new THREE.WebGLRenderer({ canvas, antialias: true });
        this.renderer.setSize(window.innerWidth, window.innerHeight);
        this.renderer.shadowMap.enabled = true;
        this.renderer.shadowMap.type = THREE.PCFSoftShadowMap;
        
        // Lighting
        this.setupLighting();
        
        // Create game objects
        this.createGround();
        this.createPlayer();
        this.createObstacles();
        
        // Handle resize
        window.addEventListener('resize', () => this.onWindowResize(), false);
    }
    
    setupLighting() {
        const ambientLight = new THREE.AmbientLight(0x404040, 0.6);
        this.scene.add(ambientLight);
        
        const directionalLight = new THREE.DirectionalLight(0xffffff, 0.8);
        directionalLight.position.set(50, 50, 50);
        directionalLight.castShadow = true;
        directionalLight.shadow.mapSize.width = 2048;
        directionalLight.shadow.mapSize.height = 2048;
        directionalLight.shadow.camera.near = 0.5;
        directionalLight.shadow.camera.far = 150;
        directionalLight.shadow.camera.left = -50;
        directionalLight.shadow.camera.right = 50;
        directionalLight.shadow.camera.top = 50;
        directionalLight.shadow.camera.bottom = -50;
        this.scene.add(directionalLight);
    }
    
    createGround() {
        const groundGeometry = new THREE.PlaneGeometry(200, 200);
        const groundMaterial = new THREE.MeshStandardMaterial({ 
            color: 0x2d4a2d,
            roughness: 0.8,
            metalness: 0.2
        });
        const ground = new THREE.Mesh(groundGeometry, groundMaterial);
        ground.rotation.x = -Math.PI / 2;
        ground.receiveShadow = true;
        this.scene.add(ground);
        
        // Grid helper for visual reference
        const gridHelper = new THREE.GridHelper(200, 20, 0x4ecca3, 0x1a1a2e);
        this.scene.add(gridHelper);
    }
    
    createPlayer() {
        const tankGroup = new THREE.Group();
        
        // Tank body
        const bodyGeometry = new THREE.BoxGeometry(4, 2, 6);
        const bodyMaterial = new THREE.MeshStandardMaterial({ color: 0x4ecca3 });
        const body = new THREE.Mesh(bodyGeometry, bodyMaterial);
        body.castShadow = true;
        body.receiveShadow = true;
        tankGroup.add(body);
        
        // Tank turret
        const turretGeometry = new THREE.CylinderGeometry(1.5, 1.5, 1, 16);
        const turretMaterial = new THREE.MeshStandardMaterial({ color: 0x3d9c8a });
        const turret = new THREE.Mesh(turretGeometry, turretMaterial);
        turret.position.y = 1.5;
        turret.castShadow = true;
        tankGroup.add(turret);
        
        // Tank barrel
        const barrelGeometry = new THREE.CylinderGeometry(0.3, 0.3, 4, 8);
        const barrelMaterial = new THREE.MeshStandardMaterial({ color: 0x2d7a6a });
        const barrel = new THREE.Mesh(barrelGeometry, barrelMaterial);
        barrel.rotation.x = Math.PI / 2;
        barrel.position.set(0, 1.5, 3);
        barrel.castShadow = true;
        tankGroup.add(barrel);
        
        tankGroup.position.set(0, 1, 0);
        this.scene.add(tankGroup);
        
        this.player = {
            mesh: tankGroup,
            body: body,
            turret: turret,
            barrel: barrel,
            speed: 0.15,
            rotationSpeed: 0.05,
            velocity: new THREE.Vector3(),
            keys: {}
        };
    }
    
    createObstacles() {
        const obstaclePositions = [
            { x: -20, z: -20 }, { x: 20, z: -20 },
            { x: -20, z: 20 }, { x: 20, z: 20 },
            { x: 0, z: -30 }, { x: 0, z: 30 },
            { x: -30, z: 0 }, { x: 30, z: 0 }
        ];
        
        obstaclePositions.forEach(pos => {
            const geometry = new THREE.BoxGeometry(8, 6, 8);
            const material = new THREE.MeshStandardMaterial({ color: 0x8b4513 });
            const obstacle = new THREE.Mesh(geometry, material);
            obstacle.position.set(pos.x, 3, pos.z);
            obstacle.castShadow = true;
            obstacle.receiveShadow = true;
            this.scene.add(obstacle);
            this.obstacles.push(obstacle);
        });
    }
    
    createEnemy(position) {
        const enemyGroup = new THREE.Group();
        
        // Enemy body
        const bodyGeometry = new THREE.BoxGeometry(4, 2, 6);
        const bodyMaterial = new THREE.MeshStandardMaterial({ color: 0xff6b6b });
        const body = new THREE.Mesh(bodyGeometry, bodyMaterial);
        body.castShadow = true;
        enemyGroup.add(body);
        
        // Enemy turret
        const turretGeometry = new THREE.CylinderGeometry(1.5, 1.5, 1, 16);
        const turretMaterial = new THREE.MeshStandardMaterial({ color: 0xe55a5a });
        const turret = new THREE.Mesh(turretGeometry, turretMaterial);
        turret.position.y = 1.5;
        enemyGroup.add(turret);
        
        // Enemy barrel
        const barrelGeometry = new THREE.CylinderGeometry(0.3, 0.3, 4, 8);
        const barrelMaterial = new THREE.MeshStandardMaterial({ color: 0xc54a4a });
        const barrel = new THREE.Mesh(barrelGeometry, barrelMaterial);
        barrel.rotation.x = Math.PI / 2;
        barrel.position.set(0, 1.5, 3);
        enemyGroup.add(barrel);
        
        enemyGroup.position.copy(position);
        this.scene.add(enemyGroup);
        
        this.enemies.push({
            mesh: enemyGroup,
            health: 3,
            speed: 0.08,
            lastShot: 0
        });
    }
    
    fireBullet(position, direction, isPlayerBullet = true) {
        const geometry = new THREE.SphereGeometry(0.5, 8, 8);
        const material = new THREE.MeshBasicMaterial({ 
            color: isPlayerBullet ? 0xffff00 : 0xff0000 
        });
        const bullet = new THREE.Mesh(geometry, material);
        bullet.position.copy(position);
        this.scene.add(bullet);
        
        this.bullets.push({
            mesh: bullet,
            velocity: direction.clone().normalize().multiplyScalar(0.8),
            isPlayerBullet: isPlayerBullet,
            life: 100
        });
    }
    
    setupEventListeners() {
        // Keyboard controls
        document.addEventListener('keydown', (e) => {
            if (this.player) {
                this.player.keys[e.code] = true;
            }
        });
        
        document.addEventListener('keyup', (e) => {
            if (this.player) {
                this.player.keys[e.code] = false;
            }
        });
        
        // Mouse controls
        document.addEventListener('mousemove', (e) => {
            if (this.isPlaying && this.player) {
                const mouseX = (e.clientX / window.innerWidth) * 2 - 1;
                const mouseY = -(e.clientY / window.innerHeight) * 2 + 1;
                
                const raycaster = new THREE.Raycaster();
                raycaster.setFromCamera(new THREE.Vector2(mouseX, mouseY), this.camera);
                
                const plane = new THREE.Plane(new THREE.Vector3(0, 1, 0), 0);
                const target = new THREE.Vector3();
                raycaster.ray.intersectPlane(plane, target);
                
                if (target) {
                    const angle = Math.atan2(target.x - this.player.mesh.position.x, target.z - this.player.mesh.position.z);
                    this.player.mesh.rotation.y = angle;
                }
            }
        });
        
        document.addEventListener('click', () => {
            if (this.isPlaying && this.player) {
                const barrelWorldPos = new THREE.Vector3();
                this.player.barrel.getWorldPosition(barrelWorldPos);
                const direction = new THREE.Vector3(0, 0, 1);
                direction.applyQuaternion(this.player.mesh.quaternion);
                this.fireBullet(barrelWorldPos, direction, true);
            }
        });
        
        // UI buttons
        document.getElementById('start-btn').addEventListener('click', () => this.startGame());
        document.getElementById('restart-btn').addEventListener('click', () => this.restartGame());
        
        // Gamepad support
        window.addEventListener('gamepadconnected', (e) => {
            console.log('Gamepad connected:', e.gamepad.id);
        });
    }
    
    startGame() {
        this.isPlaying = true;
        this.score = 0;
        this.health = 100;
        this.wave = 1;
        this.enemies = [];
        this.bullets = [];
        
        document.getElementById('start-screen').classList.add('hidden');
        document.getElementById('game-over').classList.add('hidden');
        document.getElementById('hud').classList.remove('hidden');
        
        this.updateUI();
    }
    
    restartGame() {
        this.startGame();
        if (this.player) {
            this.player.mesh.position.set(0, 1, 0);
        }
    }
    
    updateUI() {
        document.getElementById('score').textContent = this.score;
        document.getElementById('health').textContent = Math.max(0, Math.floor(this.health));
        document.getElementById('wave').textContent = this.wave;
    }
    
    handleInput() {
        if (!this.player || !this.isPlaying) return;
        
        const { keys, mesh, speed } = this.player;
        const moveVector = new THREE.Vector3(0, 0, 0);
        
        if (keys['KeyW'] || keys['ArrowUp']) moveVector.z -= 1;
        if (keys['KeyS'] || keys['ArrowDown']) moveVector.z += 1;
        if (keys['KeyA'] || keys['ArrowLeft']) moveVector.x -= 1;
        if (keys['KeyD'] || keys['ArrowRight']) moveVector.x += 1;
        
        if (moveVector.length() > 0) {
            moveVector.normalize().multiplyScalar(speed);
            
            const newPos = mesh.position.clone().add(moveVector);
            
            // Boundary check
            newPos.x = Math.max(-95, Math.min(95, newPos.x));
            newPos.z = Math.max(-95, Math.min(95, newPos.z));
            
            // Simple collision with obstacles
            let canMove = true;
            for (const obstacle of this.obstacles) {
                const dist = newPos.distanceTo(obstacle.position);
                if (dist < 8) {
                    canMove = false;
                    break;
                }
            }
            
            if (canMove) {
                mesh.position.copy(newPos);
            }
        }
        
        // Gamepad input
        const gamepads = navigator.getGamepads();
        if (gamepads[0]) {
            const gp = gamepads[0];
            const axisX = Math.abs(gp.axes[0]) > 0.1 ? gp.axes[0] : 0;
            const axisY = Math.abs(gp.axes[1]) > 0.1 ? gp.axes[1] : 0;
            
            if (axisX !== 0 || axisY !== 0) {
                const moveVector = new THREE.Vector3(axisX, 0, axisY).normalize().multiplyScalar(speed);
                const newPos = mesh.position.clone().add(moveVector);
                newPos.x = Math.max(-95, Math.min(95, newPos.x));
                newPos.z = Math.max(-95, Math.min(95, newPos.z));
                mesh.position.copy(newPos);
            }
            
            // Gamepad button for firing
            if (gp.buttons[0].pressed) {
                const barrelWorldPos = new THREE.Vector3();
                this.player.barrel.getWorldPosition(barrelWorldPos);
                const direction = new THREE.Vector3(0, 0, 1);
                direction.applyQuaternion(this.player.mesh.quaternion);
                this.fireBullet(barrelWorldPos, direction, true);
            }
        }
    }
    
    updateEnemies(deltaTime) {
        this.enemySpawnTimer += deltaTime;
        if (this.enemySpawnTimer > this.enemySpawnInterval) {
            this.enemySpawnTimer = 0;
            
            // Spawn enemy at random position on the edge
            const angle = Math.random() * Math.PI * 2;
            const distance = 80;
            const position = new THREE.Vector3(
                Math.cos(angle) * distance,
                1,
                Math.sin(angle) * distance
            );
            
            this.createEnemy(position);
        }
        
        // Update enemy behavior
        this.enemies.forEach((enemy, index) => {
            if (!this.player) return;
            
            // Move towards player
            const direction = new THREE.Vector3()
                .subVectors(this.player.mesh.position, enemy.mesh.position)
                .normalize();
            
            enemy.mesh.position.add(direction.multiplyScalar(enemy.speed));
            enemy.mesh.lookAt(this.player.mesh.position);
            
            // Shoot at player
            const now = Date.now();
            if (now - enemy.lastShot > 2000) {
                enemy.lastShot = now;
                const barrelWorldPos = new THREE.Vector3();
                enemy.mesh.children[2].getWorldPosition(barrelWorldPos);
                this.fireBullet(barrelWorldPos, direction, false);
            }
        });
    }
    
    updateBullets() {
        for (let i = this.bullets.length - 1; i >= 0; i--) {
            const bullet = this.bullets[i];
            bullet.mesh.position.add(bullet.velocity);
            bullet.life--;
            
            // Remove old bullets
            if (bullet.life <= 0) {
                this.scene.remove(bullet.mesh);
                this.bullets.splice(i, 1);
                continue;
            }
            
            // Check collisions with obstacles
            for (const obstacle of this.obstacles) {
                if (bullet.mesh.position.distanceTo(obstacle.position) < 5) {
                    this.scene.remove(bullet.mesh);
                    this.bullets.splice(i, 1);
                    break;
                }
            }
            
            // Check collisions with enemies
            if (bullet.isPlayerBullet) {
                for (let j = this.enemies.length - 1; j >= 0; j--) {
                    const enemy = this.enemies[j];
                    if (bullet.mesh.position.distanceTo(enemy.mesh.position) < 4) {
                        enemy.health--;
                        if (enemy.health <= 0) {
                            this.scene.remove(enemy.mesh);
                            this.enemies.splice(j, 1);
                            this.score += 100;
                            this.updateUI();
                            
                            // Increase difficulty
                            if (this.score % 500 === 0) {
                                this.wave++;
                                this.enemySpawnInterval = Math.max(1000, this.enemySpawnInterval - 300);
                                this.updateUI();
                            }
                        }
                        this.scene.remove(bullet.mesh);
                        this.bullets.splice(i, 1);
                        break;
                    }
                }
            } 
            // Check collisions with player
            else if (this.player && bullet.mesh.position.distanceTo(this.player.mesh.position) < 3) {
                this.health -= 10;
                this.updateUI();
                this.scene.remove(bullet.mesh);
                this.bullets.splice(i, 1);
                
                if (this.health <= 0) {
                    this.gameOver();
                }
            }
        }
    }
    
    gameOver() {
        this.isPlaying = false;
        document.getElementById('final-score').textContent = this.score;
        document.getElementById('hud').classList.add('hidden');
        document.getElementById('game-over').classList.remove('hidden');
    }
    
    onWindowResize() {
        this.camera.aspect = window.innerWidth / window.innerHeight;
        this.camera.updateProjectionMatrix();
        this.renderer.setSize(window.innerWidth, window.innerHeight);
    }
    
    animate() {
        requestAnimationFrame(() => this.animate());
        
        const currentTime = Date.now();
        const deltaTime = currentTime - this.lastTime;
        this.lastTime = currentTime;
        
        if (this.isPlaying) {
            this.handleInput();
            this.updateEnemies(deltaTime);
            this.updateBullets();
        }
        
        this.renderer.render(this.scene, this.camera);
    }
}

// Start the game when page loads
window.addEventListener('load', () => {
    new TankBattleGame();
});
