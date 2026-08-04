# 🚀 快速部署指南

## 完成 CI/CD 配置的最后步骤

### 1. 创建 GitHub 仓库并推送代码

```bash
# 在 GitHub 上创建新仓库后，执行以下命令：

# 添加远程仓库（替换为你的 GitHub 用户名）
git remote add origin https://github.com/YOUR_USERNAME/3d-tank-battle.git

# 切换到 main 分支
git branch -M main

# 推送到 main 分支（将触发自动构建）
git push -u origin main
```

### 2. 配置 GitHub Secrets

访问：`https://github.com/YOUR_USERNAME/3d-tank-battle/settings/secrets/actions`

添加以下三个 Secrets：

| Secret Name | Value | 说明 |
|-------------|-------|------|
| `UNITY_EMAIL` | 你的 Unity 账号邮箱 | 用于 Unity 激活 |
| `UNITY_PASSWORD` | 你的 Unity 账号密码 | 用于 Unity 激活 |
| `UNITY_LICENSE` | Unity 许可证文件内容 | 可选，GitHub Actions 可自动申请 |

#### 获取 Unity License 的方法

**方法 A：自动申请（推荐）**
GitHub Actions 会自动申请临时许可证，只需提供邮箱和密码即可。

**方法 B：手动导出**
```bash
# 在已安装 Unity 的机器上运行
Unity -quit -batchmode -logFile - \
  -username "your@email.com" \
  -password "yourpassword" \
  -manualLicenseFile ~/unity_license.ulf
```

### 3. 验证自动构建

推送代码后：
1. 访问 GitHub 仓库的 **Actions** 标签
2. 查看 "Build and Release" 工作流状态
3. 等待构建完成（首次约 15-30 分钟）
4. 下载构建产物（Artifacts）

### 4. 发布正式版本

```bash
# 打版本号标签
git tag v1.0.0

# 推送标签（将触发 Release 创建）
git push origin v1.0.0
```

推送标签后，GitHub Actions 将：
- ✅ 自动构建游戏
- ✅ 创建 GitHub Release
- ✅ 附加 Windows 可执行文件

---

## 📋 检查清单

在推送前请确认：

- [ ] 已删除无用文件（✅ 已完成）
- [ ] CI/CD 配置文件已添加（✅ 已完成）
- [ ] README 和 DEPLOYMENT.md 已更新（✅ 已完成）
- [ ] 代码已提交（✅ 已完成）
- [ ] GitHub 仓库已创建
- [ ] 远程仓库已配置
- [ ] GitHub Secrets 已设置
- [ ] 推送到 main 分支
- [ ] 验证构建成功

---

## 🔍 常见问题

### Q: 构建失败怎么办？
A: 查看 Actions 日志，常见原因：
- Unity 许可证问题 → 检查 Secrets 配置
- 构建超时 → 增加 timeout-minutes
- 缺少模块 → 检查 unity-builder 配置

### Q: 如何构建其他平台？
A: 编辑 `.github/workflows/build.yml`，添加：
```yaml
build-linux:
  with:
    targetPlatform: StandaloneLinux64
    
build-mac:
  with:
    targetPlatform: StandaloneOSX
```

### Q: 如何自定义构建参数？
A: 修改 `build.yml` 中的 `with` 部分：
```yaml
unityVersion: 2022.3.0f1    # Unity 版本
buildName: TankBattle       # 游戏名称
buildVersion: 1.0.0         # 版本号
```

---

## 📊 构建状态徽章

将以下代码添加到 README.md 顶部：

```markdown
![Build Status](https://github.com/YOUR_USERNAME/3d-tank-battle/actions/workflows/build.yml/badge.svg?branch=main)
```

---

**祝部署顺利！** 🎮
