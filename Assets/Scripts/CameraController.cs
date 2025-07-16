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
    
    [Header("自動追従設定")]
    public bool autoFollow = true;
    public float autoFollowSpeed = 1f;
    
    private float currentX = 0f;
    private float currentY = 20f;
    private Vector3 currentVelocity;
    private bool isInitialized = false;
    
    void Start()
    {
        InitializeCamera();
    }
    
    void LateUpdate()
    {
        if (target == null) 
        {
            // ターゲットがない場合、プレイヤーを探す
            FindPlayerTarget();
            return;
        }
        
        if (!isInitialized)
        {
            InitializeCamera();
        }
        
        HandleMouseInput();
        UpdateCameraPosition();
    }
    
    /// <summary>
    /// カメラの初期化
    /// </summary>
    void InitializeCamera()
    {
        if (target != null)
        {
            // 初期角度の設定
            Vector3 angles = transform.eulerAngles;
            currentX = angles.y;
            currentY = angles.x;
            
            // 初期位置の設定
            SetInitialPosition();
            isInitialized = true;
            
            Debug.Log($"カメラが初期化されました: {target.name}");
        }
    }
    
    /// <summary>
    /// プレイヤーターゲットを自動探知
    /// </summary>
    void FindPlayerTarget()
    {
        GameObject player = GameObject.Find("LocalPlayer");
        if (player != null)
        {
            SetTarget(player.transform);
        }
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
    /// ターゲットの設定
    /// </summary>
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        isInitialized = false;
        
        if (target != null)
        {
            Debug.Log($"カメラターゲットを設定: {target.name}");
            InitializeCamera();
        }
    }
    
    /// <summary>
    /// 初期位置の設定
    /// </summary>
    void SetInitialPosition()
    {
        if (target == null) return;
        
        // カメラの初期位置を計算
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        Vector3 direction = new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotation * direction;
        
        // 即座に位置を設定（スムージングなし）
        transform.position = targetPosition;
        transform.LookAt(target.position);
    }
    
    /// <summary>
    /// カメラ位置の更新
    /// </summary>
    void UpdateCameraPosition()
    {
        if (target == null) return;
        
        // 目標回転の計算
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0);
        
        // カメラの位置計算
        Vector3 direction = new Vector3(0, height, -distance);
        Vector3 targetPosition = target.position + rotation * direction;
        
        // スムーズな移動
        float dampingValue = autoFollow ? damping * autoFollowSpeed : damping;
        transform.position = Vector3.SmoothDamp(
            transform.position, 
            targetPosition, 
            ref currentVelocity, 
            dampingValue * Time.deltaTime
        );
        
        // スムーズな回転
        Vector3 lookDirection = target.position - transform.position;
        if (lookDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                rotationDamping * Time.deltaTime
            );
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
            SetInitialPosition();
        }
    }
    
    /// <summary>
    /// デバッグ用GUI
    /// </summary>
    void OnGUI()
    {
        GUI.Label(new Rect(10, 50, 300, 20), "右クリックドラッグでカメラ回転");
        GUI.Label(new Rect(10, 70, 300, 20), $"Camera Angle: X={currentX:F1}, Y={currentY:F1}");
        
        if (target != null)
        {
            GUI.Label(new Rect(10, 90, 300, 20), $"Target: {target.name}");
            GUI.Label(new Rect(10, 110, 300, 20), $"Distance: {Vector3.Distance(transform.position, target.position):F2}");
        }
        else
        {
            GUI.Label(new Rect(10, 90, 300, 20), "Target: None");
        }
        
        if (GUI.Button(new Rect(10, 130, 100, 20), "Reset Camera"))
        {
            ResetCamera();
        }
    }
}