using UnityEngine;

/// <summary>
/// シンプルな移動テスト用スクリプト
/// PlayerControllerが動作しない場合の代替として使用
/// </summary>
public class SimpleMovement : MonoBehaviour
{
    [Header("シンプル移動設定")]
    public float moveSpeed = 5f;
    public bool enableSimpleMovement = false;
    
    private Rigidbody rb;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    void Update()
    {
        if (!enableSimpleMovement) return;
        
        // 直接的な入力処理
        float horizontal = 0f;
        float vertical = 0f;
        
        if (Input.GetKey(KeyCode.W)) vertical = 1f;
        if (Input.GetKey(KeyCode.S)) vertical = -1f;
        if (Input.GetKey(KeyCode.A)) horizontal = -1f;
        if (Input.GetKey(KeyCode.D)) horizontal = 1f;
        
        // シンプルな移動
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        
        if (movement.magnitude > 0.1f)
        {
            if (rb != null)
            {
                // Rigidbodyを使った移動
                Vector3 targetVelocity = movement.normalized * moveSpeed;
                rb.velocity = new Vector3(targetVelocity.x, rb.velocity.y, targetVelocity.z);
            }
            else
            {
                // Transformを使った移動
                transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);
            }
            
            Debug.Log($"SimpleMovement: {movement}, Position: {transform.position}");
        }
    }
    
    void OnGUI()
    {
        // 簡単な切り替えボタン
        if (GUI.Button(new Rect(Screen.width - 150, Screen.height - 50, 140, 30), 
            enableSimpleMovement ? "Simple Movement ON" : "Simple Movement OFF"))
        {
            enableSimpleMovement = !enableSimpleMovement;
            Debug.Log($"SimpleMovement: {enableSimpleMovement}");
        }
    }
}