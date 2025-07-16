using UnityEngine;

/// <summary>
/// 入力テスト用のヘルパークラス
/// プレイヤーの移動が動作しない場合のデバッグ用
/// </summary>
public class InputTester : MonoBehaviour
{
    [Header("入力テスト")]
    public bool enableInputTest = false;
    public bool showInputDebug = true;
    
    void Update()
    {
        if (showInputDebug)
        {
            TestInput();
        }
    }
    
    void TestInput()
    {
        // 基本的な入力テスト
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        
        if (Mathf.Abs(h) > 0.1f || Mathf.Abs(v) > 0.1f)
        {
            Debug.Log($"入力検出: Horizontal={h:F2}, Vertical={v:F2}");
        }
        
        // キー入力の直接テスト
        if (Input.GetKey(KeyCode.W))
        {
            Debug.Log("Wキー押下");
        }
        if (Input.GetKey(KeyCode.A))
        {
            Debug.Log("Aキー押下");
        }
        if (Input.GetKey(KeyCode.S))
        {
            Debug.Log("Sキー押下");
        }
        if (Input.GetKey(KeyCode.D))
        {
            Debug.Log("Dキー押下");
        }
    }
    
    void OnGUI()
    {
        if (showInputDebug)
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");
            
            GUI.Box(new Rect(Screen.width - 220, 10, 200, 120), "Input Debug");
            GUI.Label(new Rect(Screen.width - 210, 35, 180, 20), $"Horizontal: {h:F3}");
            GUI.Label(new Rect(Screen.width - 210, 55, 180, 20), $"Vertical: {v:F3}");
            
            GUI.Label(new Rect(Screen.width - 210, 80, 180, 20), "Keys:");
            string keys = "";
            if (Input.GetKey(KeyCode.W)) keys += "W ";
            if (Input.GetKey(KeyCode.A)) keys += "A ";
            if (Input.GetKey(KeyCode.S)) keys += "S ";
            if (Input.GetKey(KeyCode.D)) keys += "D ";
            GUI.Label(new Rect(Screen.width - 210, 100, 180, 20), keys);
        }
    }
}