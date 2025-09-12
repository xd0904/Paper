using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 리듬게임 시스템 - HitLine이 좌우로 움직이며 WASD 입력에 따라 색깔별 반응
/// </summary>
public class RythmBar : MonoBehaviour
{
    [Header("🎵 리듬게임 설정")]
    [Tooltip("판정선 (HitLine)")]
    public RectTransform hitLine;
    
    [Header("🎨 색깔 영역 설정")]
    [Tooltip("빨간 영역 범위")]
    public float redZoneStart = -550f;
    public float redZoneEnd = -275f;
    
    [Tooltip("노란 영역 범위")]
    public float yellowZoneStart = -275f;
    public float yellowZoneEnd = 275f;
    
    [Tooltip("초록 영역 범위")]
    public float greenZoneStart = 275f;
    public float greenZoneEnd = 550f;
    
    [Header("⚙️ 움직임 설정")]
    [Tooltip("HitLine 이동 속도")]
    [Range(1f, 10f)]
    public float moveSpeed = 3f;
    
    [Tooltip("좌우 이동 범위 (최소값)")]
    public float minX = -550f;
    
    [Tooltip("좌우 이동 범위 (최대값)")]
    public float maxX = 550f;
    
    [Tooltip("플레이어 이동 거리 (한 칸)")]
    [Range(1f, 10f)]
    public float stepDistance = 1f;
    
    [Header("📊 현재 상태")]
    [SerializeField] private bool movingRight = true;
    [SerializeField] private Vector3 startPosition;
    [SerializeField] private string currentZone = "None";
    
    void Start()
    {
        InitializeHitLine();
    }
    
    void Update()
    {
        MoveHitLine();
        CheckCurrentZone();
        HandleInput();
    }
    
    void InitializeHitLine()
    {
        if (hitLine == null)
        {
            // HitLine 자동 찾기
            GameObject hitLineObj = GameObject.Find("HitLine");
            if (hitLineObj != null)
            {
                hitLine = hitLineObj.GetComponent<RectTransform>();
            }
            else
            {
                Debug.LogWarning("⚠️ HitLine을 찾을 수 없습니다!");
                return;
            }
        }
        
        // 시작 위치 저장 (UI 앵커드 포지션)
        if (hitLine != null)
        {
            startPosition = hitLine.anchoredPosition;
        }
        Debug.Log("🎵 리듬게임 시스템 초기화 완료!");
    }
    
    void MoveHitLine()
    {
        if (hitLine == null) return;
        
        // 좌우로 계속 움직임 (UI 좌표계 사용)
        float moveDirection = movingRight ? 1f : -1f;
        Vector2 currentPos = hitLine.anchoredPosition;
        currentPos.x += moveDirection * moveSpeed * Time.deltaTime * 100f; // 빠른 속도
        hitLine.anchoredPosition = currentPos;
        
        // 범위 체크 및 방향 전환 (-550 ~ 550)
        float currentX = hitLine.anchoredPosition.x;
        
        // X 좌표 실시간 출력 (UI 앵커드 포지션)
        Debug.Log($"🎯 HitLine UI X 좌표: {currentX:F2}");
        
        if (currentX >= maxX)
        {
            movingRight = false;
            // 위치를 최대값으로 제한
            Vector2 pos = hitLine.anchoredPosition;
            pos.x = maxX;
            hitLine.anchoredPosition = pos;
            Debug.Log($"🔄 최대값 도달! X를 {maxX}로 제한");
        }
        else if (currentX <= minX)
        {
            movingRight = true;
            // 위치를 최소값으로 제한
            Vector2 pos = hitLine.anchoredPosition;
            pos.x = minX;
            hitLine.anchoredPosition = pos;
            Debug.Log($"🔄 최소값 도달! X를 {minX}로 제한");
        }
    }
    
    void CheckCurrentZone()
    {
        if (hitLine == null) return;
        
        float hitLineX = hitLine.anchoredPosition.x;
        string newZone = "None";
        
        // 우선순위: Green(가장 정확) -> Yellow(보통) -> Red(실패)
        if (hitLineX >= greenZoneStart && hitLineX <= greenZoneEnd)
        {
            newZone = "Green";
        }
        else if (hitLineX >= yellowZoneStart && hitLineX <= yellowZoneEnd)
        {
            newZone = "Yellow";
        }
        else if (hitLineX >= redZoneStart && hitLineX <= redZoneEnd)
        {
            newZone = "Red";
        }
        
        // 영역이 바뀌었을 때만 업데이트
        if (currentZone != newZone)
        {
            currentZone = newZone;
            Debug.Log($"🎯 현재 영역: {currentZone} (UI X: {hitLineX:F1})");
        }
    }
    
    void HandleInput()
    {
        bool inputDetected = false;
        Vector3 moveDirection = Vector3.zero;
        
        // WASD 입력 감지
        if (Input.GetKeyDown(KeyCode.W))
        {
            inputDetected = true;
            moveDirection = Vector3.up;
            Debug.Log("⬆️ W키 입력");
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            inputDetected = true;
            moveDirection = Vector3.left;
            Debug.Log("⬅️ A키 입력");
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            inputDetected = true;
            moveDirection = Vector3.down;
            Debug.Log("⬇️ S키 입력");
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            inputDetected = true;
            moveDirection = Vector3.right;
            Debug.Log("➡️ D키 입력");
        }
        
        if (inputDetected)
        {
            ProcessInput(moveDirection);
        }
    }
    
    void ProcessInput(Vector3 direction)
    {
        switch (currentZone)
        {
            case "Green":
                // 초록: 소리없이 이동
                MovePlayer(direction);
                Debug.Log("✅ 완벽! 조용히 이동");
                break;
                
            case "Yellow":
                // 노랑: 소리나면서 이동
                MovePlayer(direction);
                Debug.Log("⚠️ 소음 발생! (소리: 삐삑)");
                break;
                
            case "Red":
                // 빨강: 소리나고 이동 불가
                Debug.Log("❌ 실패! 이동 불가 (소리: 경고음)");
                break;
                
            default:
                Debug.Log("🔍 판정 영역 밖");
                break;
        }
    }
    
    void MovePlayer(Vector3 direction)
    {
        // 플레이어를 정확히 1씩만 이동
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            // 플레이어가 UI 오브젝트인지 확인
            RectTransform playerRect = player.GetComponent<RectTransform>();
            
            if (playerRect != null)
            {
                // UI 플레이어인 경우 - anchoredPosition 사용
                Vector2 currentPos = playerRect.anchoredPosition;
                
                if (direction == Vector3.up)
                    currentPos.y += 1f;
                else if (direction == Vector3.down)
                    currentPos.y -= 1f;
                else if (direction == Vector3.left)
                    currentPos.x -= 1f;
                else if (direction == Vector3.right)
                    currentPos.x += 1f;
                
                playerRect.anchoredPosition = currentPos;
                Debug.Log($"🏃 UI 플레이어 이동: {direction} -> 위치: {currentPos}");
            }
            else
            {
                // 월드 플레이어인 경우 - position 사용
                Vector3 currentPos = player.transform.position;
                
                if (direction == Vector3.up)
                    currentPos.y += 1f;
                else if (direction == Vector3.down)
                    currentPos.y -= 1f;
                else if (direction == Vector3.left)
                    currentPos.x -= 1f;
                else if (direction == Vector3.right)
                    currentPos.x += 1f;
                
                player.transform.position = currentPos;
                Debug.Log($"🏃 월드 플레이어 이동: {direction} -> 위치: {currentPos}");
            }
        }
        else
        {
            Debug.Log($"🎯 이동 명령: {direction} (플레이어 없음)");
        }
    }
    
    [ContextMenu("🎵 상태 출력")]
    void PrintStatus()
    {
        Debug.Log($"=== 🎵 리듬게임 상태 ===");
        Debug.Log($"현재 영역: {currentZone}");
        Debug.Log($"이동 방향: {(movingRight ? "오른쪽" : "왼쪽")}");
        Debug.Log($"HitLine UI 위치: {(hitLine != null ? hitLine.anchoredPosition.ToString() : "없음")}");
    }
}
