using UnityEngine;

/// <summary>
/// シーンの初期セットアップを行うクラス
/// 地面、ライティング、基本オブジェクトの配置
/// </summary>
public class SceneSetup : MonoBehaviour
{
    [Header("地面設定")]
    public bool createGround = true;
    public Vector3 groundSize = new Vector3(20f, 0.1f, 20f);
    public Material groundMaterial;
    
    [Header("ライティング設定")]
    public bool setupLighting = true;
    public Color ambientColor = Color.gray;
    public float ambientIntensity = 0.3f;
    
    [Header("テスト用オブジェクト")]
    public bool createTestObjects = true;
    public int objectCount = 5;
    
    void Awake()
    {
        SetupScene();
    }
    
    /// <summary>
    /// シーンセットアップのメイン処理
    /// </summary>
    void SetupScene()
    {
        if (createGround)
        {
            CreateGround();
        }
        
        if (setupLighting)
        {
            SetupSceneLighting();
        }
        
        if (createTestObjects)
        {
            CreateTestObjects();
        }
        
        Debug.Log("シーンセットアップ完了");
    }
    
    /// <summary>
    /// 地面の作成
    /// </summary>
    void CreateGround()
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Ground";
        ground.transform.position = new Vector3(0, -0.05f, 0); // 地面を少し下げる
        ground.transform.localScale = groundSize;
        
        // 地面のマテリアル設定
        Renderer renderer = ground.GetComponent<Renderer>();
        if (groundMaterial != null)
        {
            renderer.material = groundMaterial;
        }
        else
        {
            // デフォルトの地面マテリアル
            Material defaultMaterial = new Material(Shader.Find("Standard"));
            defaultMaterial.color = new Color(0.5f, 0.7f, 0.3f); // 緑っぽい色
            renderer.material = defaultMaterial;
        }
        
        // 地面のタグ設定
        ground.tag = "Ground";
        
        Debug.Log("地面を作成しました");
    }
    
    /// <summary>
    /// ライティングのセットアップ
    /// </summary>
    void SetupSceneLighting()
    {
        // アンビエントライトの設定
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = ambientColor * ambientIntensity;
        
        // メインライト（太陽光）の設定
        Light mainLight = FindObjectOfType<Light>();
        if (mainLight == null)
        {
            GameObject lightObject = new GameObject("Main Light");
            mainLight = lightObject.AddComponent<Light>();
        }
        
        mainLight.type = LightType.Directional;
        mainLight.color = Color.white;
        mainLight.intensity = 1.2f;
        mainLight.transform.rotation = Quaternion.Euler(45f, 45f, 0f);
        
        Debug.Log("ライティングをセットアップしました");
    }
    
    /// <summary>
    /// テスト用オブジェクトの作成
    /// </summary>
    void CreateTestObjects()
    {
        for (int i = 0; i < objectCount; i++)
        {
            // ランダムな位置にオブジェクトを配置
            Vector3 position = new Vector3(
                Random.Range(-8f, 8f),
                1f,
                Random.Range(-8f, 8f)
            );
            
            // ランダムなプリミティブを選択
            PrimitiveType[] types = { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Cylinder };
            PrimitiveType selectedType = types[Random.Range(0, types.Length)];
            
            GameObject testObject = GameObject.CreatePrimitive(selectedType);
            testObject.name = $"TestObject_{i}";
            testObject.transform.position = position;
            
            // ランダムな色のマテリアル
            Renderer renderer = testObject.GetComponent<Renderer>();
            Material material = new Material(Shader.Find("Standard"));
            material.color = new Color(
                Random.Range(0.2f, 1f),
                Random.Range(0.2f, 1f),
                Random.Range(0.2f, 1f)
            );
            renderer.material = material;
            
            // Rigidbodyの追加（物理演算用）
            Rigidbody rb = testObject.AddComponent<Rigidbody>();
            rb.mass = Random.Range(0.5f, 2f);
        }
        
        Debug.Log($"{objectCount}個のテストオブジェクトを作成しました");
    }
    
    /// <summary>
    /// スカイボックスの設定
    /// </summary>
    public void SetSkybox(Material skyboxMaterial)
    {
        if (skyboxMaterial != null)
        {
            RenderSettings.skybox = skyboxMaterial;
            DynamicGI.UpdateEnvironment();
            Debug.Log("スカイボックスを設定しました");
        }
    }
    
    /// <summary>
    /// シーンのリセット
    /// </summary>
    public void ResetScene()
    {
        // 作成したオブジェクトの削除
        GameObject[] testObjects = GameObject.FindGameObjectsWithTag("TestObject");
        foreach (GameObject obj in testObjects)
        {
            if (Application.isPlaying)
            {
                Destroy(obj);
            }
            else
            {
                DestroyImmediate(obj);
            }
        }
        
        // 再セットアップ
        if (createTestObjects)
        {
            CreateTestObjects();
        }
        
        Debug.Log("シーンをリセットしました");
    }
}