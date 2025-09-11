using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 벽에 Shadow Caster 2D를 추가하여 검은색 그림자 생성
/// </summary>
public class WallShadow : MonoBehaviour
{
    [Header("🖤 그림자 설정")]
    [Tooltip("그림자 색상")]
    public Color shadowColor = Color.black;
    
    [Tooltip("그림자 강도 (0~1)")]
    [Range(0f, 1f)]
    public float shadowIntensity = 1f;
    void Start()
    {
        SetupShadowCaster();
        SetupGlobalShadowSettings();
    }
    
    void SetupShadowCaster()
    {
        // Shadow Caster 2D가 없으면 추가
        ShadowCaster2D shadowCaster = GetComponent<ShadowCaster2D>();
        
        if (shadowCaster == null)
        {
            shadowCaster = gameObject.AddComponent<ShadowCaster2D>();
        }
        
        // 그림자 설정 (Unity 6 API)
        shadowCaster.castsShadows = true;
        
        // 자동으로 스프라이트 모양을 사용하도록 설정
        
        // 벽 태그 설정
        if (!gameObject.CompareTag("Wall"))
        {
            gameObject.tag = "Wall";
        }
        
        Debug.Log($"🖤 벽 그림자 설정 완료: {gameObject.name}");
    }
    
    void SetupGlobalShadowSettings()
    {
        // Global Light 2D 찾기 (씬의 메인 조명)
        Light2D[] allLights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
        
        foreach (Light2D light in allLights)
        {
            if (light.lightType == Light2D.LightType.Global)
            {
                // Global Light의 그림자 설정
                Debug.Log($"🌍 Global Light 발견: {light.gameObject.name}");
                // Global Light는 Project Settings에서 설정해야 함
            }
        }
        
        // URP Asset에서 그림자 색상 설정하는 방법을 로그로 안내
        Debug.Log("🖤 그림자를 검은색으로 만들려면:");
        Debug.Log("1. Project Settings → Graphics → URP Asset 선택");
        Debug.Log("2. 2D Renderer Data → Shadow Color를 검은색으로 설정");
        Debug.Log("3. 또는 Lighting → Environment → Ambient Color를 검은색으로 설정");
    }
    
    [ContextMenu("🖤 그림자 설정 다시 적용")]
    void RefreshShadow()
    {
        SetupShadowCaster();
        SetupGlobalShadowSettings();
    }
    
    [ContextMenu("📋 그림자 색상 설정 가이드")]
    void ShowShadowColorGuide()
    {
        Debug.Log("=== 🖤 검은색 그림자 만드는 방법 ===");
        Debug.Log("1️⃣ Project Settings → Graphics");
        Debug.Log("2️⃣ URP Asset (Scriptable Render Pipeline Settings) 클릭");
        Debug.Log("3️⃣ 2D Renderer Data → Shadow Color를 Black (0,0,0,1)으로 설정");
        Debug.Log("4️⃣ 또는 Window → Lighting → Environment → Ambient Color를 Black으로 설정");
        Debug.Log("5️⃣ Lighting → Mixed Lighting → Realtime Shadow Color도 Black으로 설정");
    }
}
