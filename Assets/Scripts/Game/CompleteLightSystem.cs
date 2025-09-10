using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 완전한 2D 라이팅 시스템 매니저
/// 플레이어 라이트, 벽 그림자, 전역 조명 등을 자동으로 설정
/// </summary>
public class CompleteLightSystem : MonoBehaviour
{
    [Header("플레이어 설정")]
    public Transform player;
    
    [Header("라이트 설정")]
    public float lightRange = 8f;
    public float lightIntensity = 1.5f;
    public Color lightColor = Color.white;
    
    [Header("전역 조명 설정")]
    public float globalLightIntensity = 0.1f; // 어둡게
    
    void Start()
    {
        SetupCompleteSystem();
    }
    
    void SetupCompleteSystem()
    {
        // 1. 플레이어 찾기
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
                return;
            }
        }
        
        // 2. 플레이어에 라이트 추가
        SetupPlayerLight();
        
        // 3. 모든 벽에 그림자 추가
        SetupAllWalls();
        
        // 4. 모든 스프라이트를 라이트 시스템에 맞게 설정
        SetupAllSprites();
        
        // 5. 전역 조명 설정
        SetupGlobalLight();
        
        Debug.Log("완전한 2D 라이팅 시스템 설정 완료!");
    }
    
    void SetupPlayerLight()
    {
        Light2D playerLight = player.GetComponent<Light2D>();
        
        if (playerLight == null)
        {
            playerLight = player.gameObject.AddComponent<Light2D>();
        }
        
        playerLight.lightType = Light2D.LightType.Point;
        playerLight.intensity = lightIntensity;
        playerLight.pointLightOuterRadius = lightRange;
        playerLight.pointLightInnerRadius = 0f;
        playerLight.color = lightColor;
        playerLight.lightOrder = 0;
        
        Debug.Log("플레이어 라이트 설정 완료!");
    }
    
    void SetupAllWalls()
    {
        // Wall 태그가 있는 모든 오브젝트 찾기
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        
        foreach (GameObject wall in walls)
        {
            ShadowCaster2D shadowCaster = wall.GetComponent<ShadowCaster2D>();
            
            if (shadowCaster == null)
            {
                shadowCaster = wall.AddComponent<ShadowCaster2D>();
            }
            
            shadowCaster.useRendererSilhouette = true;
            shadowCaster.castsShadows = true;
        }
        
        Debug.Log($"벽 그림자 설정 완료: {walls.Length}개");
    }
    
    void SetupAllSprites()
    {
        // 모든 SpriteRenderer 찾기
        SpriteRenderer[] allSprites = FindObjectsOfType<SpriteRenderer>();
        
        foreach (SpriteRenderer sprite in allSprites)
        {
            // Sprite-Lit-Default 머티리얼로 변경
            if (sprite.material.name.Contains("Default") && !sprite.material.name.Contains("Lit"))
            {
                // URP Lit 머티리얼 찾기 시도
                Material litMaterial = Resources.Load<Material>("Sprite-Lit-Default");
                if (litMaterial == null)
                {
                    // 기본 URP 2D Lit 머티리얼 사용
                    sprite.material = new Material(Shader.Find("Universal Render Pipeline/2D/Sprite-Lit-Default"));
                }
                else
                {
                    sprite.material = litMaterial;
                }
            }
        }
        
        Debug.Log($"스프라이트 머티리얼 설정 완료: {allSprites.Length}개");
    }
    
    void SetupGlobalLight()
    {
        // Global Light 2D 찾기 또는 생성
        Light2D globalLight = FindObjectOfType<Light2D>();
        
        // Global Light가 플레이어 라이트가 아닌 경우에만
        if (globalLight != null && globalLight.lightType == Light2D.LightType.Global)
        {
            globalLight.intensity = globalLightIntensity;
        }
        else
        {
            // Global Light 생성
            GameObject globalLightObj = new GameObject("Global Light 2D");
            Light2D newGlobalLight = globalLightObj.AddComponent<Light2D>();
            newGlobalLight.lightType = Light2D.LightType.Global;
            newGlobalLight.intensity = globalLightIntensity;
        }
        
        Debug.Log("전역 조명 설정 완료!");
    }
    
    [ContextMenu("시스템 다시 설정")]
    void RefreshSystem()
    {
        SetupCompleteSystem();
    }
}
