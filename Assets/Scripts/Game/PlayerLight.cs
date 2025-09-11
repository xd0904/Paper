using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 플레이어에게 2D Point Light를 추가하고 설정
/// 이 스크립트를 플레이어 GameObject에 추가하세요
/// </summary>
public class PlayerLight : MonoBehaviour
{
    [Header("라이트 설정")]
    public float lightRange = 8f;
    public float lightIntensity = 1.5f;
    public Color lightColor = Color.white;
    
    private Light2D playerLight;
    
    void Start()
    {
        SetupPlayerLight();
    }
    
    void SetupPlayerLight()
    {
        // 기존 Light2D가 있는지 확인
        playerLight = GetComponent<Light2D>();
        
        if (playerLight == null)
        {
            // Light2D 컴포넌트 추가
            playerLight = gameObject.AddComponent<Light2D>();
        }
        
        // Point Light로 설정
        playerLight.lightType = Light2D.LightType.Point;
        playerLight.intensity = lightIntensity;
        playerLight.pointLightOuterRadius = lightRange;
        playerLight.pointLightInnerRadius = 0f;
        playerLight.color = lightColor;
        
        // 중요: Light Layer 설정
        playerLight.lightOrder = 0;
        
        Debug.Log("플레이어 라이트 설정 완료!");
    }
    
    [ContextMenu("라이트 설정 다시 적용")]
    void RefreshLight()
    {
        SetupPlayerLight();
    }
}
