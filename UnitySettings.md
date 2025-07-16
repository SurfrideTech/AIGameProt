# Unity プロジェクト設定メモ

## 推奨Unity設定

### プロジェクト設定
- **Unity Version**: 2022.3 LTS以上推奨
- **Render Pipeline**: Built-in Render Pipeline（後でURP移行予定）
- **Platform**: Windows, macOS, Linux（Dedicated Server対応）

### Input System
- 現在は旧Input Managerを使用
- 将来的にNew Input Systemに移行予定

### Physics設定
- Fixed Timestep: 0.02 (50Hz)
- Gravity: (0, -9.81, 0)
- Rigidbody使用でスムーズな物理移動

### Network設定（将来用）
- Unity Netcode for GameObjects使用予定
- Dedicated Server対応

## ビルド設定

### Development Build
- Development Build: ON
- Script Debugging: ON
- Deep Profiling: OFF

### Player Settings
- Company Name: あなたの会社名
- Product Name: AI Game Prototype
- Version: 0.1.0

## 必要なパッケージ（将来）
- Unity Netcode for GameObjects
- Unity Input System
- Unity URP (Universal Render Pipeline)
- Unity Cinemachine（カメラ制御拡張用）
