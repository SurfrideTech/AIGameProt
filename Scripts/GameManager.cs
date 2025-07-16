using UnityEngine;

/// <summary>
/// ゲーム全体の管理クラス
/// Dedicated Server対応を考慮した設計
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("ゲーム設定")]
    public GameObject playerPrefab;
    public Transform spawnPoint;
    
    [Header("デバッグ設定")]
    public bool isDebugMode = true;
    public bool isServer = false; // Dedicated Server判定用
    
    private GameObject localPlayer;
    
    void Start()
    {
        InitializeGame();
    }
    
    /// <summary>
    /// ゲーム初期化
    /// </summary>
    void InitializeGame()
    {
        if (isDebugMode)
        {
            Debug.Log("ゲーム初期化開始");
        }
        
        // プレイヤーの生成
        SpawnPlayer();
        
        // カメラのセットアップ
        SetupCamera();
    }
    
    /// <summary>
    /// プレイヤー生成
    /// </summary>
    void SpawnPlayer()
    {
        Vector3 spawnPosition = spawnPoint != null ? spawnPoint.position : Vector3.zero;
        
        if (playerPrefab != null)
        {
            localPlayer = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
            localPlayer.name = "LocalPlayer";
            
            if (isDebugMode)
            {
                Debug.Log($"プレイヤーを生成: {spawnPosition}");
            }
        }
        else
        {
            // Prefabが設定されていない場合、デフォルトのCubeを作成
            CreateDefaultPlayer(spawnPosition);
        }
    }
    
    /// <summary>
    /// デフォルトプレイヤー（Cube）の作成
    /// </summary>
    void CreateDefaultPlayer(Vector3 position)
    {
        localPlayer = GameObject.CreatePrimitive(PrimitiveType.Cube);
        localPlayer.transform.position = position;
        localPlayer.name = "LocalPlayer";
        
        // Rigidbodyの追加
        Rigidbody rb = localPlayer.AddComponent<Rigidbody>();
        rb.freezeRotation = true; // Y軸回転以外を固定
        
        // PlayerControllerの追加
        localPlayer.AddComponent<PlayerController>();
        
        // マテリアルの設定（識別用）
        Renderer renderer = localPlayer.GetComponent<Renderer>();
        if (renderer != null)
        {
            Material material = new Material(Shader.Find("Standard"));
            material.color = Color.blue;
            renderer.material = material;
        }
        
        if (isDebugMode)
        {
            Debug.Log("デフォルトプレイヤー（Cube）を作成");
        }
    }
    
    /// <summary>
    /// カメラのセットアップ
    /// </summary>
    void SetupCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null && localPlayer != null)
        {
            // カメラをプレイヤーの後ろに配置
            CameraController cameraController = mainCamera.GetComponent<CameraController>();
            if (cameraController == null)
            {
                cameraController = mainCamera.gameObject.AddComponent<CameraController>();
            }
            
            cameraController.SetTarget(localPlayer.transform);
        }
    }
    
    /// <summary>
    /// ゲーム終了処理
    /// </summary>
    public void QuitGame()
    {
        if (isDebugMode)
        {
            Debug.Log("ゲーム終了");
        }
        
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
    
    /// <summary>
    /// デバッグ用GUI
    /// </summary>
    void OnGUI()
    {
        if (isDebugMode)
        {
            GUI.Box(new Rect(10, Screen.height - 100, 200, 80), "Debug Info");
            GUI.Label(new Rect(15, Screen.height - 85, 190, 20), $"Server Mode: {isServer}");
            GUI.Label(new Rect(15, Screen.height - 65, 190, 20), $"Player Count: {(localPlayer != null ? 1 : 0)}");
            
            if (GUI.Button(new Rect(15, Screen.height - 45, 80, 20), "Quit"))
            {
                QuitGame();
            }
        }
    }
}