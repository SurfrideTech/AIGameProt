# 第五人格風ゲーム プロトタイプ

## 概要
Unity + Dedicated Serverを使用した第五人格風ゲームのプロトタイプです。
現在はWASDでCubeを移動させる基本システムを実装しています。

## セットアップ手順

### 1. Unityプロジェクトの作成
1. Unity Hubで新しい3Dプロジェクトを作成
2. プロジェクト名: AIGameProt
3. このフォルダの内容をプロジェクトにコピー

### 2. スクリプトの配置
- `Scripts/`フォルダ内のすべての.csファイルをUnityプロジェクトのAssetsフォルダに配置

### 3. シーンのセットアップ
1. 新しいシーンを作成（Game.scene）
2. 空のGameObjectを作成し、"GameManager"と命名
3. GameManagerスクリプトをアタッチ
4. 空のGameObjectを作成し、"SceneSetup"と命名
5. SceneSetupスクリプトをアタッチ

### 4. カメラの設定
1. Main Cameraに CameraController スクリプトをアタッチ
2. 必要に応じてカメラの初期位置を調整

## 操作方法
- **WASD**: プレイヤー移動
- **右クリック + ドラッグ**: カメラ回転
- **ESC**: ゲーム終了（デバッグモード）

## ファイル構成
```
AIGameProt/
├── Scripts/
│   ├── PlayerController.cs     # プレイヤー移動制御
│   ├── GameManager.cs          # ゲーム全体管理
│   ├── CameraController.cs     # カメラ制御
│   └── SceneSetup.cs          # シーン初期化
├── Scenes/                     # シーンファイル
├── Prefabs/                    # プレハブ
├── Claude.md                   # 開発記録
└── README.md                   # このファイル
```

## 今後の拡張予定
1. ネットワーク対応（Dedicated Server）
2. 第五人格風ゲームロジック
3. UI/UXシステム
4. サウンドシステム
5. キャラクターシステム

## 開発メモ
- Dedicated Server対応を考慮した設計
- 各スクリプトはネットワーク拡張を想定
- デバッグ機能を各所に配置
- コードコメントは日本語で記載

## トラブルシューティング
- プレイヤーが移動しない → PlayerControllerがアタッチされているか確認
- カメラが追従しない → CameraControllerのTargetが設定されているか確認
- 地面が表示されない → SceneSetupのcreateGroundがtrueになっているか確認
