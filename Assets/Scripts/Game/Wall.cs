using UnityEngine;

/// <summary>
/// 벽 오브젝트를 시야각 시스템에 맞게 설정
/// </summary>
public class Wall : MonoBehaviour
{
    void Start()
    {
        SetupWall();
    }
    
    void SetupWall()
    {
        // 벽 레이어 설정 (기본: Default 레이어 사용)
        if (gameObject.layer == 0) // Default 레이어면
        {
            // 원하는 벽 레이어로 변경 (예: 레이어 8)
            // gameObject.layer = 8;
        }
        
        // 태그 설정
        if (!gameObject.CompareTag("Wall"))
        {
            gameObject.tag = "Wall";
        }
        
        // Collider2D가 없으면 추가
        if (GetComponent<Collider2D>() == null)
        {
            BoxCollider2D collider = gameObject.AddComponent<BoxCollider2D>();
            collider.isTrigger = false; // 물리적 충돌
        }
    }
}
