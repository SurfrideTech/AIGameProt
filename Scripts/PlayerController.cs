using UnityEngine;

/// <summary>
/// 基本的なプレイヤー移動制御
/// WASDキーでCubeを移動させる
/// Dedicated Server対応を考慮した設計
/// </summary>
public class PlayerController : MonoBehaviour
{
    [Header("移動設定")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 180f;
    
    [Header("物理設定")]
    public bool useRigidbody = true;
    
    private Rigidbody rb;
    private Vector3 movement;
    private bool isLocalPlayer = true; // 後でネットワーク対応時に使用
    
    void Start()
    {
        // Rigidbodyコンポーネントの取得
        rb = GetComponent<Rigidbody>();
        
        if (rb == null && useRigidbody)
        {
            Debug.LogWarning("Rigidbodyが見つかりません。物理移動を無効にします。");
            useRigidbody = false;
        }
    }
    
    void Update()
    {
        // ローカルプレイヤーのみ入力を受け付け（ネットワーク対応時に重要）
        if (!isLocalPlayer) return;
        
        HandleInput();
    }
    
    void FixedUpdate()
    {
        // 物理演算での移動
        if (useRigidbody && rb != null)
        {
            MoveWithRigidbody();
        }
    }
    
    /// <summary>
    /// 入力処理
    /// </summary>
    void HandleInput()
    {
        // WASD入力の取得
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical");     // W/S
        
        // 移動ベクトルの計算
        movement = new Vector3(horizontal, 0f, vertical).normalized;
        
        // Transform移動の場合はUpdateで直接移動
        if (!useRigidbody)
        {
            MoveWithTransform();
        }
        
        // 回転処理
        HandleRotation();
    }
    
    /// <summary>
    /// Transform移動
    /// </summary>
    void MoveWithTransform()
    {
        if (movement.magnitude > 0.1f)
        {
            Vector3 move = movement * moveSpeed * Time.deltaTime;
            transform.Translate(move, Space.World);
        }
    }
    
    /// <summary>
    /// Rigidbody移動（物理演算対応）
    /// </summary>
    void MoveWithRigidbody()
    {
        if (movement.magnitude > 0.1f)
        {
            Vector3 move = movement * moveSpeed;
            rb.MovePosition(transform.position + move * Time.fixedDeltaTime);
        }
    }
    
    /// <summary>
    /// 回転処理
    /// </summary>
    void HandleRotation()
    {
        if (movement.magnitude > 0.1f)
        {
            // 移動方向に向かって回転
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation, 
                targetRotation, 
                rotationSpeed * Time.deltaTime
            );
        }
    }
    
    /// <summary>
    /// ネットワーク対応用のメソッド（将来の拡張用）
    /// </summary>
    public void SetLocalPlayer(bool isLocal)
    {
        isLocalPlayer = isLocal;
    }
    
    /// <summary>
    /// デバッグ用の現在位置表示
    /// </summary>
    void OnGUI()
    {
        if (isLocalPlayer)
        {
            GUI.Label(new Rect(10, 10, 200, 20), $"Position: {transform.position}");
            GUI.Label(new Rect(10, 30, 200, 20), $"Movement: {movement}");
        }
    }
}