using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 인벤토리 아이템 선택 시스템 (Inspector에서 수동 설정)
/// </summary>
public class ItemSelect : MonoBehaviour
{
    [Header("🎒 인벤토리 설정")]
    [Tooltip("인벤토리 아이템들 (Inspector에서 직접 설정)")]
    public List<GameObject> inventoryItems = new List<GameObject>();
    
    [Header("🎨 색상 설정")]
    [Tooltip("기본 아이템 색상")]
    public Color defaultColor = Color.white;
    
    [Tooltip("선택된 아이템 색상")]
    public Color selectedColor = Color.yellow;
    
    [Header("📏 크기 설정")]
    [Tooltip("기본 아이템 크기")]
    [Range(0.1f, 2f)]
    public float defaultScale = 0.5f;
    
    [Tooltip("선택된 아이템 크기")]
    [Range(0.1f, 2f)]
    public float selectedScale = 0.8f;
    
    [Header("📊 현재 상태")]
    [SerializeField] private int selectedItemIndex = -1;
    [SerializeField] private bool[] itemStates = new bool[4];
    
    private List<Image> itemImages = new List<Image>();
    private List<Transform> itemTransforms = new List<Transform>();
    
    void Start()
    {
        InitializeItems();
        ResetAllItems();
    }
    
    void InitializeItems()
    {
        itemImages.Clear();
        itemTransforms.Clear();
        
        for (int i = 0; i < 4; i++)
        {
            if (i < inventoryItems.Count && inventoryItems[i] != null)
            {
                Image img = inventoryItems[i].GetComponent<Image>();
                if (img == null)
                {
                    img = inventoryItems[i].AddComponent<Image>();
                }
                
                itemImages.Add(img);
                itemTransforms.Add(inventoryItems[i].transform);
                
                int childCount = inventoryItems[i].transform.childCount;
                string status = childCount > 0 ? $"아이템 있음 ({childCount}개)" : "빈 슬롯";
                
                if (childCount > 0)
                {
                    string childName = inventoryItems[i].transform.GetChild(0).name;
                    status += $" - {childName}";
                }
                
                Debug.Log($"🎒 아이템 {i + 1}: {inventoryItems[i].name} - {status}");
            }
            else
            {
                itemImages.Add(null);
                itemTransforms.Add(null);
                Debug.Log($"📦 아이템 {i + 1}: 설정되지 않음");
            }
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SelectItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SelectItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SelectItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SelectItem(3);
    }
    
    void SelectItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= 4)
        {
            Debug.LogWarning($"⚠️ 잘못된 인덱스: {itemIndex}");
            return;
        }
        
        if (itemIndex >= inventoryItems.Count || inventoryItems[itemIndex] == null)
        {
            Debug.LogWarning($"⚠️ 아이템 {itemIndex + 1}이 설정되지 않았습니다!");
            return;
        }
        
        // 중복 선택 체크
        if (selectedItemIndex == itemIndex)
        {
            Debug.Log($"🔄 이미 선택된 아이템입니다!");
            return;
        }
        
        // 이전 선택 해제
        if (selectedItemIndex != -1)
        {
            DeselectItem(selectedItemIndex);
        }
        
        // 새 아이템 선택
        selectedItemIndex = itemIndex;
        itemStates[itemIndex] = true;
        
        // 배경은 항상 노란색으로 변경
        ApplySelectedEffect(itemIndex);
        
        // 자식 아이템이 있는지 체크
        int childCount = inventoryItems[itemIndex].transform.childCount;
        Debug.Log($"🔍 {inventoryItems[itemIndex].name} 자식 개수: {childCount}");
        
        if (childCount > 0)
        {
            string childName = inventoryItems[itemIndex].transform.GetChild(0).name;
            Debug.Log($"✅ 아이템 {itemIndex + 1} 선택! 내용: {childName}");
        }
        else
        {
            Debug.Log($"✅ 아이템 {itemIndex + 1} 선택! (빈 슬롯이지만 배경 변경됨)");
        }
    }
    
    void DeselectItem(int itemIndex)
    {
        if (itemIndex < 0 || itemIndex >= 4) return;
        
        itemStates[itemIndex] = false;
        ApplyDefaultEffect(itemIndex);
    }
    
    void ApplySelectedEffect(int itemIndex)
    {
        // Item 슬롯의 배경은 항상 노란색으로 변경
        if (itemIndex < itemImages.Count && itemImages[itemIndex] != null)
        {
            itemImages[itemIndex].color = selectedColor;
            Debug.Log($"🟨 아이템 {itemIndex + 1} 배경을 노란색으로 변경");
        }
        
        // 자식 아이템이 있으면 크기도 변경
        if (itemIndex < itemTransforms.Count && itemTransforms[itemIndex] != null)
        {
            if (itemTransforms[itemIndex].childCount > 0)
            {
                // 첫 번째 자식 아이템의 크기 변경
                Transform childItem = itemTransforms[itemIndex].GetChild(0);
                childItem.localScale = Vector3.one * selectedScale;
                Debug.Log($"✨ {childItem.name} 크기를 {selectedScale}로 변경");
            }
        }
    }
    
    void ApplyDefaultEffect(int itemIndex)
    {
        // Item 슬롯의 배경을 기본 색상으로 복원
        if (itemIndex < itemImages.Count && itemImages[itemIndex] != null)
        {
            itemImages[itemIndex].color = defaultColor;
            Debug.Log($"⚪ 아이템 {itemIndex + 1} 배경을 기본 색상으로 복원");
        }
        
        // 자식 아이템이 있으면 크기도 복원
        if (itemIndex < itemTransforms.Count && itemTransforms[itemIndex] != null)
        {
            if (itemTransforms[itemIndex].childCount > 0)
            {
                // 첫 번째 자식 아이템의 크기 복원
                Transform childItem = itemTransforms[itemIndex].GetChild(0);
                childItem.localScale = Vector3.one * defaultScale;
                Debug.Log($"🔙 {childItem.name} 크기를 {defaultScale}로 복원");
            }
        }
    }
    
    void ResetAllItems()
    {
        selectedItemIndex = -1;
        
        for (int i = 0; i < 4; i++)
        {
            itemStates[i] = false;
            ApplyDefaultEffect(i);
        }
    }
    
    [ContextMenu("📊 상태 출력")]
    void PrintStatus()
    {
        Debug.Log($"=== 🎒 인벤토리 상태 ===");
        Debug.Log($"선택된 아이템: {(selectedItemIndex == -1 ? "없음" : (selectedItemIndex + 1).ToString())}");
        
        for (int i = 0; i < 4; i++)
        {
            if (i < inventoryItems.Count && inventoryItems[i] != null)
            {
                int childCount = inventoryItems[i].transform.childCount;
                string hasItem = childCount > 0 ? "✅ 있음" : "❌ 없음";
                string status = itemStates[i] ? "선택됨" : "기본";
                Debug.Log($"아이템 {i + 1}: {inventoryItems[i].name} - {hasItem} - {status}");
            }
            else
            {
                Debug.Log($"아이템 {i + 1}: 설정 안됨");
            }
        }
    }
    
    public int GetSelectedItemIndex() => selectedItemIndex;
    public bool IsItemSelected(int itemIndex) => itemIndex >= 0 && itemIndex < 4 && itemStates[itemIndex];
    public GameObject GetSelectedItem() => selectedItemIndex >= 0 && selectedItemIndex < inventoryItems.Count ? inventoryItems[selectedItemIndex] : null;
}
