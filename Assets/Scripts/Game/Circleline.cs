using UnityEngine;

public class Circleline : MonoBehaviour
{
    [Header("Animation Settings")]
    public float minScale = 0f;
    public float maxScale = 4f;
    public float animationSpeed = 1f;
    
    private float currentTime = 0f;
    private Vector3 originalScale;
    
    void Start()
    {
        // 원래 스케일 저장
        originalScale = transform.localScale;
        // 시작할 때 최소 크기로 설정
        transform.localScale = Vector3.one * minScale;
    }
    
    void Update()
    {
        // 시간 업데이트
        currentTime += Time.deltaTime * animationSpeed;
        
        // 0에서 1까지의 진행도 계산 (반복)
        float progress = currentTime % 1f;
        
        // 0에서 4까지 선형으로 증가한 후 바로 0으로 떨어짐
        float scaleValue = Mathf.Lerp(minScale, maxScale, progress);
        
        // 스케일 적용 (모든 축에 동일하게 적용)
        transform.localScale = Vector3.one * scaleValue;
    }
    
    // 인스펙터에서 실시간으로 속도 조정 가능
    [Header("Runtime Controls")]
    [Range(0.1f, 5f)]
    public float speedMultiplier = 1f;
    
    void LateUpdate()
    {
        // 실시간 속도 조정
        animationSpeed = speedMultiplier;
    }
}