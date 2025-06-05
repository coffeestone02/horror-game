using UnityEngine;

public class AltarSlot : MonoBehaviour
{
    [Tooltip("이 슬롯에 들어갈 수 있는 공물의 이름 (예: firstItem, secondItem)")]
    public string requiredItemName;

    [Tooltip("공물을 바쳤을 때 보여질 시각적 오브젝트 (예: PlacedFirstItem)")]
    public GameObject visualObject;

    [Tooltip("상태를 업데이트할 제단 매니저")]
    public AltarManager altarManager;

    public void PlaceItem()
    {
        visualObject.SetActive(true);

        if (altarManager != null)
        {
            if (requiredItemName == "firstItem")
                altarManager.isFirstItemPlaced = true;
            else if (requiredItemName == "secondItem")
                altarManager.isSecondItemPlaced = true;
        }
        else
        {
            Debug.LogWarning("AltarManager가 연결되어 있지 않습니다.");
        }
    }
}
