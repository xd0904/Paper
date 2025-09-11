using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 손전등 배터리 시스템 - 아이템4 선택시 슬라이더 감소하며 조명 밝아짐
/// </summary>
public class LightRemain : MonoBehaviour
{
    [Header("🔋 배터리 설정")]
    [Tooltip("배터리 슬라이더")]
    public Slider batterySlider;
    
    [Tooltip("배터리 감소 속도 (초당)")]
    [Range(0.01f, 1f)]
    public float batteryDrainRate = 0.1f;
    
    [Header("💡 조명 설정")]
    [Tooltip("플레이어 조명 (Light2D)")]
    public Light2D playerLight;
    
    [Tooltip("기본 조명 강도")]
    [Range(0f, 5f)]
    public float defaultLightIntensity = 1f;
    
    [Tooltip("손전등 사용시 조명 강도")]
    [Range(0f, 10f)]
    public float flashlightIntensity = 3f;
    
    [Tooltip("기본 조명 범위")]
    [Range(0f, 10f)]
    public float defaultLightRadius = 2f;
    
    [Tooltip("손전등 사용시 조명 범위")]
    [Range(0f, 20f)]
    public float flashlightRadius = 5f;
    
    [Header("🔗 연결")]
    [Tooltip("아이템 선택 시스템")]
    public ItemSelect itemSelect;
    
    [Header("📊 현재 상태")]
    [SerializeField] private bool isFlashlightActive = false;
    [SerializeField] private float currentBattery = 100f;
    
    void Start()
    {
        InitializeComponents();
        SetDefaultLight();
    }
    
    void InitializeComponents()
    {
        // 슬라이더 자동 찾기
        if (batterySlider == null)
        {
            batterySlider = GetComponent<Slider>();
            if (batterySlider == null)
            {
                Debug.LogWarning("⚠️ 배터리 슬라이더를 찾을 수 없습니다!");
            }
        }
        
        // ItemSelect 자동 찾기
        if (itemSelect == null)
        {
            itemSelect = FindFirstObjectByType<ItemSelect>();
            if (itemSelect == null)
            {
                Debug.LogWarning("⚠️ ItemSelect를 찾을 수 없습니다!");
            }
        }
        
        // Light2D 자동 찾기 또는 생성
        if (playerLight == null)
        {
            // 플레이어에서 Light2D 찾기
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null)
            {
                playerLight = player.GetComponent<Light2D>();
                if (playerLight == null)
                {
                    // Light2D가 없으면 자동으로 추가
                    playerLight = player.AddComponent<Light2D>();
                    playerLight.lightType = Light2D.LightType.Point;
                    playerLight.intensity = defaultLightIntensity;
                    playerLight.pointLightOuterRadius = defaultLightRadius;
                    playerLight.color = Color.white;
                    Debug.Log("🔦 플레이어에 Light2D 자동 추가됨!");
                }
            }
            else
            {
                // 플레이어 태그가 없으면 일반 검색
                playerLight = FindFirstObjectByType<Light2D>();
            }
            
            if (playerLight == null)
            {
                Debug.LogWarning("⚠️ Light2D를 찾을 수 없습니다! 플레이어에 'Player' 태그를 설정해주세요!");
            }
        }
        
        // 초기 배터리 값 설정
        if (batterySlider != null)
        {
            currentBattery = batterySlider.value * 100f;
        }
        
        Debug.Log("🔋 LightRemain 초기화 완료");
    }
    
    void Update()
    {
        CheckFlashlightStatus();
        UpdateBattery();
        UpdateLight();
    }
    
    void CheckFlashlightStatus()
    {
        if (itemSelect == null) return;
        
        // 아이템 4가 선택되었는지 체크
        bool item4Selected = itemSelect.IsItemSelected(3); // 인덱스 3 = 아이템 4
        
        if (item4Selected && !isFlashlightActive)
        {
            // 손전등 켜짐
            isFlashlightActive = true;
            Debug.Log("🔦 손전등 켜짐!");
        }
        else if (!item4Selected && isFlashlightActive)
        {
            // 손전등 꺼짐
            isFlashlightActive = false;
            Debug.Log("🔦 손전등 꺼짐!");
        }
    }
    
    void UpdateBattery()
    {
        if (batterySlider == null) return;
        
        // 손전등이 켜져있고 배터리가 0보다 클 때만 감소
        if (isFlashlightActive && currentBattery > 0)
        {
            currentBattery -= batteryDrainRate * Time.deltaTime * 100f;
            currentBattery = Mathf.Max(0f, currentBattery);
            
            // 슬라이더 업데이트
            batterySlider.value = currentBattery / 100f;
            
            if (currentBattery <= 0)
            {
                Debug.Log("🔋 배터리 방전!");
            }
        }
    }
    
    void UpdateLight()
    {
        if (playerLight == null) return;
        
        if (isFlashlightActive && currentBattery > 0)
        {
            // 손전등 모드 - 밝고 넓게
            SetFlashlightMode();
        }
        else
        {
            // 기본 모드
            SetDefaultLight();
        }
    }
    
    void SetFlashlightMode()
    {
        if (playerLight == null) return;
        
        playerLight.intensity = flashlightIntensity;
        playerLight.pointLightOuterRadius = flashlightRadius;
    }
    
    void SetDefaultLight()
    {
        if (playerLight == null) return;
        
        playerLight.intensity = defaultLightIntensity;
        playerLight.pointLightOuterRadius = defaultLightRadius;
    }
    
    [ContextMenu("🔋 배터리 충전")]
    void RechargeBattery()
    {
        currentBattery = 100f;
        if (batterySlider != null)
        {
            batterySlider.value = 1f;
        }
        Debug.Log("🔋 배터리 완전 충전!");
    }
    
    [ContextMenu("📊 상태 출력")]
    void PrintStatus()
    {
        Debug.Log($"=== 🔦 손전등 상태 ===");
        Debug.Log($"손전등 활성화: {isFlashlightActive}");
        Debug.Log($"배터리: {currentBattery:F1}%");
        Debug.Log($"조명 강도: {(playerLight != null ? playerLight.intensity.ToString("F1") : "없음")}");
        Debug.Log($"조명 범위: {(playerLight != null ? playerLight.pointLightOuterRadius.ToString("F1") : "없음")}");
    }
    
    // Public 접근자들
    public bool IsFlashlightActive() => isFlashlightActive;
    public float GetBatteryPercent() => currentBattery;
    public bool HasBattery() => currentBattery > 0;
}
