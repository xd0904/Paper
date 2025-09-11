using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 시야각 그림자 시스템 - 벽 뒤의 적은 어둡게, 시야 내의 적은 밝게
/// </summary>
public class SimpleShadowSystem : MonoBehaviour
{
    [Header("🔍 그림자 설정")]
    [Tooltip("벽이나 장애물 레이어")]
    public LayerMask wallLayerMask = -1;
    
    [Header("🎨 색상 설정")]
    [Tooltip("시야 내 적 색상 (밝게)")]
    public Color visibleColor = Color.white;
    
    [Tooltip("그림자 속 적 색상 (검은색)")]
    public Color shadowColor = Color.black;
    
    [Header("🔧 디버그")]
    public bool showDebugRays = true;
    
    [Header("📊 실시간 상태")]
    [SerializeField] private int totalEnemies = 0;
    [SerializeField] private int visibleEnemies = 0;
    [SerializeField] private int shadowEnemies = 0;
    
    private List<EnemyData> enemies = new List<EnemyData>();
    
    [System.Serializable]
    public class EnemyData
    {
        public GameObject enemy;
        public SpriteRenderer renderer;
        public Color originalColor;
        public bool isInShadow;
        
        public EnemyData(GameObject obj)
        {
            enemy = obj;
            renderer = obj.GetComponent<SpriteRenderer>();
            if (renderer != null)
                originalColor = renderer.color;
            isInShadow = false;
        }
    }
    
    void Start()
    {
        FindAllEnemies();
        InvokeRepeating("UpdateShadows", 0f, 0.1f);
    }
    
    void FindAllEnemies()
    {
        enemies.Clear();
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(new EnemyData(enemy));
        }
        
        totalEnemies = enemies.Count;
        Debug.Log($"총 {totalEnemies}개의 적을 찾았습니다.");
    }
    
    void UpdateShadows()
    {
        visibleEnemies = 0;
        shadowEnemies = 0;
        
        foreach (EnemyData enemyData in enemies)
        {
            if (enemyData.enemy == null) continue;
            
            bool inShadow = IsEnemyInShadow(enemyData);
            enemyData.isInShadow = inShadow;
            
            ApplyShadowEffect(enemyData);
            
            // 카운트 업데이트
            if (inShadow)
                shadowEnemies++;
            else
                visibleEnemies++;
        }
    }
    
    bool IsEnemyInShadow(EnemyData enemyData)
    {
        if (enemyData.renderer == null) return true;
        
        Vector3 playerPos = transform.position;
        Vector3 enemyPos = enemyData.enemy.transform.position;
        
        Vector3 direction = enemyPos - playerPos;
        float distance = direction.magnitude;
        
        // 플레이어에서 적까지 레이캐스트
        RaycastHit2D hit = Physics2D.Raycast(playerPos, direction.normalized, distance, wallLayerMask);
        
        // 디버그 레이
        if (showDebugRays)
        {
            Color rayColor = (hit.collider == null) ? Color.green : Color.red;
            Debug.DrawLine(playerPos, enemyPos, rayColor, 0.1f);
        }
        
        // 벽에 막혔으면 그림자 속
        return hit.collider != null;
    }
    
    void ApplyShadowEffect(EnemyData enemyData)
    {
        if (enemyData.renderer == null) return;
        
        if (enemyData.isInShadow)
        {
            // 그림자 속 - 어둡게 (하지만 보임)
            enemyData.renderer.color = shadowColor;
        }
        else
        {
            // 시야 내 - 밝게
            enemyData.renderer.color = visibleColor;
        }
    }
    
    [ContextMenu("🔄 적 다시 찾기")]
    void RefreshEnemies()
    {
        FindAllEnemies();
    }
    
    [ContextMenu("📊 상태 출력")]
    void PrintStatus()
    {
        Debug.Log($"=== 시야각 그림자 상태 ===");
        Debug.Log($"총 적: {totalEnemies}");
        Debug.Log($"시야 내 적: {visibleEnemies}");
        Debug.Log($"그림자 속 적: {shadowEnemies}");
    }
    
    void OnDestroy()
    {
        // 원래 색상으로 복원
        foreach (EnemyData enemyData in enemies)
        {
            if (enemyData.renderer != null)
            {
                enemyData.renderer.color = enemyData.originalColor;
            }
        }
    }
}
