# 3D 坦克大战 - CI/CD 部署指南

## 🚀 自动编译发布流程

本项目已配置 GitHub Actions，当代码推送到 `main` 分支时，将自动触发以下流程：

### 自动化流程
1. **代码检出** - 拉取最新代码
2. **Unity 环境设置** - 使用 Unity 2022.3.0f1
3. **构建游戏** - 编译 Windows 64位版本
4. **上传产物** - 保存构建文件到 GitHub Artifacts
5. **创建 Release** - 当推送标签时自动发布

## 📋 前置准备

### 1. 获取 Unity License
在 GitHub Secrets 中配置以下变量：

```bash
# 访问 https://github.com/你的用户名/3d-tank-battle/settings/secrets/actions

UNITY_EMAIL: <你的 Unity 账号邮箱>
UNITY_PASSWORD: <你的 Unity 账号密码>
UNITY_LICENSE: <Unity 许可证文件内容>
```

### 2. 获取 Unity License 文件
```bash
# 方法 1: 使用 Unity Hub 导出许可证
# 方法 2: 使用 unity-ci/license-request action 自动生成
```

### 3. 配置 Git 远程仓库
```bash
cd /workspace
git remote add origin https://github.com/你的用户名/3d-tank-battle.git
git branch -M main
git push -u origin main
```

## 🏷️ 发布新版本

### 创建 Release 版本
```bash
# 打标签并推送
git tag v1.0.0
git push origin v1.0.0
```

推送标签后，GitHub Actions 将：
- 自动构建游戏
- 创建 GitHub Release
- 附加可执行文件 (.exe)

## 📦 构建产物

构建完成后，可以在以下位置找到：

1. **Artifacts** (每次构建): 
   - GitHub Actions 页面 → 选择运行 → 下载 "WindowsBuild"
   
2. **Releases** (仅标签推送):
   - GitHub 仓库 → Releases → 下载对应版本的 .exe 文件

## ⚙️ 自定义配置

### 修改 Unity 版本
编辑 `.github/workflows/build.yml`:
```yaml
unityVersion: 2022.3.0f1  # 改为你需要的版本
```

### 添加其他平台构建
在 `build.yml` 的 jobs 中添加：
```yaml
build-linux:
  with:
    targetPlatform: StandaloneLinux64
    
build-webgl:
  with:
    targetPlatform: WebGL
```

### 修改构建参数
```yaml
buildName: TankBattle      # 游戏名称
buildVersion: 1.0.0        # 版本号
targetPlatform: StandaloneWindows64  # 目标平台
```

## 🔍 故障排查

### 常见问题

**1. License 错误**
```
Error: Unity license is not valid
```
解决：重新生成 UNITY_LICENSE secret

**2. 构建超时**
```
Error: The build operation timed out
```
解决：增加 timeout-minutes 或优化项目大小

**3. 缺少依赖**
```
Error: Missing Unity modules
```
解决：在 unity-builder 配置中添加 buildParameters

### 查看构建日志
1. 访问 GitHub 仓库
2. 点击 "Actions" 标签
3. 选择对应的构建任务
4. 查看详细日志输出

## 📊 构建状态徽章

将以下代码添加到 README.md:

```markdown
![Build Status](https://github.com/你的用户名/3d-tank-battle/actions/workflows/build.yml/badge.svg?branch=main)
```

## 🎯 下一步

1. ✅ 清理无用文件 (已完成)
2. ✅ 配置 CI/CD 流水线 (已完成)
3. ⏳ 推送代码到 main 分支
4. ⏳ 配置 GitHub Secrets
5. ⏳ 触发首次自动构建
6. ⏳ 验证构建产物
7. ⏳ 创建第一个 Release 版本

---

**提示**: 首次构建可能需要 15-30 分钟，因为需要下载 Unity 引擎和构建项目。
