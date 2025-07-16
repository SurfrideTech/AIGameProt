using UnityEngine;

/// <summary>
/// 3人称視点カメラ制御
/// プレイヤーを追従するカメラシステム
/// </summary>
public class CameraController : MonoBehaviour
{
    [Header("カメラ設定")]
    public Transform target;
    public float distance = 8f;
    public float height = 5f;
    public float damping = 2f;
    public float rotationDamping = 3f;
    
    [Header("マウス制御")]
    public bool enableMouseControl = true;
    public float mouseSensitivity = 2f;
    public float minYAngle = -20f;
    public float maxYAngle = 80f;
    
    private float currentX = 0f;
    private float currentY = 20f;
    private Vector3 currentVelocity;
    
    void Start()
    {
        // 初期角度の設定
        if (target != null)
        {
            Vector3 angles = transform.eulerAngles;
            currentX = angles.y;
            currentY = angles.x;
        }
    }
    
    void LateUpdate()
    {
        if (target == null) return;
        
        HandleMouseInput();
        UpdateCameraPosition();
    }
    
    /// <summary>
    /// マウス入力処理
    /// </summary>
    void HandleMouseInput()
    {
        if (enableMouseControl && Input.GetMouseButton(1)) // 右クリックドラッグ
        {
            currentX += Input.GetAxis("Mouse X") * mouseSensitivity;
            currentY -= Input.GetAxis("Mouse Y") * mouseSensitivity;
            
            // Y軸回転の制限
            currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);
        }
    }
    
    /// <summary>
    /// カメラ位置の更新
    /// </summary>
    void UpdateCameraPosition()
    {
        // 目標回転の計算
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // カメラの位置計算
        Vector3 direction = new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotation * direction;
        
        // スムーズな移動
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref currentVelocity, 
            damping * Time.deltaTime
        );
        
        // スムーズな回転
        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            targetRotation, 
            rotationDamping * Time.deltaTime
        );
    }
    
    /// <summary>
    /// ターゲットの設定
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        
        if (target != null)
        {
            // カメラの初期位置を設定
            Vector3 direction = new Vector3(0, height, -distance);
            transform.position = target.position + direction;
            transform.LookAt(target);
        }
    }
    
    /// <summary>
    /// カメラ設定のリセット
    /// </summary>
    public void ResetCamera()
    {
        if (target != null)
        {
            currentX = 0f;
            currentY = 20f;
            Vector3 direction = new Vector3(0, height, -distance);
            transform.position = target.position + direction;
            transform.LookAt(target);
        }
    }
    
    /// <summary>
    /// デバッグ用GUI
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(10, 50, 300, 20), "右クリックドラッグでカメラ回転");
        GUI.Label(new Rect(10, 70, 300, 20), $"Camera Angle: X={currentX:F1}, Y={currentY:F1}");
        
        if (GUI.Button(new Rect(10, 90, 100, 20), "Reset Camera"))
        {
            ResetCamera();
        }
    }
}