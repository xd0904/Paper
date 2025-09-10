using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 간단한 raycast 기반 시야각 시스템
/// </summary>
public class SimpleRaycastSight : MonoBehaviour
{
    [Header("설정")]
    public Transform player;
    public LayerMask wallLayer = -1;
    
    [Header("디버그")]
    public bool showDebugInfo = true;
    public bool enableSightSystem = true;
    
    private List<GameObject> enemies = new List<GameObject>();
    
    void Start()
    {
        // 플레이어 찾기
        if (player == null)
        {
            GameObject playerObj = GameObject.FindWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                if (showDebugInfo) Debug.Log("플레이어 자동 찾기 성공: " + playerObj.name);
            }
            else
            {
                if (showDebugInfo) Debug.LogError("Player 태그를 가진 오브젝트를 찾을 수 없습니다!");
                return;
            }
        }
        
        FindEnemies();
        
        if (enableSightSystem)
        {
            InvokeRepeating("UpdateVisibility", 0f, 0.1f); // 0.1초마다 업데이트
            if (showDebugInfo) Debug.Log("Raycast 시야각 시스템 시작됨");
        }
    }
    
    void FindEnemies()
    {
        GameObject[] foundEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        enemies.Clear();
        
        foreach (GameObject enemy in foundEnemies)
        {
            enemies.Add(enemy);
        }
        
        if (showDebugInfo) Debug.Log($"총 {enemies.Count}개의 적 발견");
    }
    
    void UpdateVisibility()
    {
        if (player == null || !enableSightSystem) return;
        
        int visibleCount = 0;
        int hiddenCount = 0;
        
        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;
            
            bool canSee = CanSeeAnyPartOfEnemy(enemy);
            
            SpriteRenderer renderer = enemy.GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = canSee;
            }
            
            SpriteRenderer[] childRenderers = enemy.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer childRenderer in childRenderers)
            {
                childRenderer.enabled = canSee;
            }
            
            if (canSee) visibleCount++;
            else hiddenCount++;
        }
        
        if (showDebugInfo && Time.time % 5f < 0.1f)
        {
            Debug.Log($"시야각 상태 - 보이는 적: {visibleCount}, 숨겨진 적: {hiddenCount}");
        }
    }
    
    bool CanSeeAnyPartOfEnemy(GameObject enemy)
    {
        Vector3 playerPos = player.position;
        Vector3 enemyPos = enemy.transform.position;
        
        // 플레이어 경계
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        float playerHalfWidth, playerHalfHeight;
        
        if (playerRenderer != null)
        {
            Bounds bounds = playerRenderer.bounds;
            playerHalfWidth = bounds.size.x * 0.5f;
            playerHalfHeight = bounds.size.y * 0.5f;
        }
        else
        {
            playerHalfWidth = player.localScale.x * 0.5f;
            playerHalfHeight = player.localScale.y * 0.5f;
        }
        
        Vector3[] playerCorners = {
            playerPos + new Vector3(-playerHalfWidth, playerHalfHeight, 0),   // 왼쪽 위
            playerPos + new Vector3(-playerHalfWidth, -playerHalfHeight, 0),  // 왼쪽 아래
            playerPos + new Vector3(playerHalfWidth, playerHalfHeight, 0),    // 오른쪽 위
            playerPos + new Vector3(playerHalfWidth, -playerHalfHeight, 0)    // 오른쪽 아래
        };
        
        // 적 경계
        SpriteRenderer enemyRenderer = enemy.GetComponent<SpriteRenderer>();
        float enemyHalfWidth, enemyHalfHeight;
        
        if (enemyRenderer != null)
        {
            Bounds enemyBounds = enemyRenderer.bounds;
            enemyHalfWidth = enemyBounds.size.x * 0.5f;
            enemyHalfHeight = enemyBounds.size.y * 0.5f;
        }
        else
        {
            enemyHalfWidth = enemy.transform.localScale.x * 0.5f;
            enemyHalfHeight = enemy.transform.localScale.y * 0.5f;
        }
        
        Vector3[] enemyCorners = {
            enemyPos + new Vector3(-enemyHalfWidth, enemyHalfHeight, 0),   // 왼쪽 위
            enemyPos + new Vector3(-enemyHalfWidth, -enemyHalfHeight, 0),  // 왼쪽 아래
            enemyPos + new Vector3(enemyHalfWidth, enemyHalfHeight, 0),    // 오른쪽 위
            enemyPos + new Vector3(enemyHalfWidth, -enemyHalfHeight, 0)    // 오른쪽 아래
        };
        
        // raycast 체크
        for (int i = 0; i < playerCorners.Length; i++)
        {
            for (int j = 0; j < enemyCorners.Length; j++)
            {
                if (!IsBlockedByWall(playerCorners[i], enemyCorners[j]))
                {
                    if (showDebugInfo)
                    {
                        Debug.DrawLine(playerCorners[i], enemyCorners[j], Color.green, 0.1f);
                    }
                    return true;
                }
                else if (showDebugInfo)
                {
                    Debug.DrawLine(playerCorners[i], enemyCorners[j], Color.red, 0.1f);
                }
            }
        }
        
        return false;
    }
    
    bool IsBlockedByWall(Vector3 start, Vector3 target)
    {
        Vector3 direction = target - start;
        float distance = direction.magnitude;
        
        Vector3 rayStart = start + direction.normalized * 0.1f;
        RaycastHit2D hit = Physics2D.Raycast(rayStart, direction.normalized, distance - 0.1f, wallLayer);
        
        return hit.collider != null;
    }
    
    [ContextMenu("적 다시 찾기")]
    void RefreshEnemies()
    {
        FindEnemies();
    }
    
    [ContextMenu("시야각 시스템 토글")]
    void ToggleSightSystem()
    {
        enableSightSystem = !enableSightSystem;
        
        if (!enableSightSystem)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue;
                
                SpriteRenderer renderer = enemy.GetComponent<SpriteRenderer>();
                if (renderer != null)
                {
                    renderer.enabled = true;
                }
                
                SpriteRenderer[] childRenderers = enemy.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer childRenderer in childRenderers)
                {
                    childRenderer.enabled = true;
                }
            }
        }
    }
}
