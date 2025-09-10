using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// 벽에 Shadow Caster 2D를 추가하여 그림자 생성
/// </summary>
public class WallShadow : MonoBehaviour
{
    void Start()
    {
        SetupShadowCaster();
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
        
        Debug.Log($"벽 그림자 설정 완료: {gameObject.name}");
    }
    
    [ContextMenu("그림자 설정 다시 적용")]
    void RefreshShadow()
    {
        SetupShadowCaster();
    }
}
