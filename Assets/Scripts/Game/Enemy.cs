using UnityEngine;

/// <summary>
/// 적 오브젝트를 시야각 시스템에 맞게 설정
/// </summary>
public class Enemy : MonoBehaviour
{
    void Start()
    {
        SetupEnemy();
    }
    
    void SetupEnemy()
    {
        // 적 태그 설정
        if (!gameObject.CompareTag("Enemy"))
        {
            gameObject.tag = "Enemy";
        }
        
        // 적 레이어 설정 (원하는 레이어로 변경)
        if (gameObject.layer == 0) // Default 레이어면
        {
            // gameObject.layer = 9; // Enemy 레이어로 변경
        }
        
        // SpriteRenderer가 있는지 확인
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogWarning($"{gameObject.name}에 SpriteRenderer가 없습니다!");
        }
    }
}
