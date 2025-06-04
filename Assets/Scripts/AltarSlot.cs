using UnityEngine;

public class AltarSlot : MonoBehaviour
{
    [Tooltip("이 슬롯에 들어갈 수 있는 공물의 이름")]
    public string requiredItemName;  // 예: "firstItem", "secondItem"

    [Tooltip("공물을 바쳤을 때 활성화될 오브젝트")]
    public GameObject visualObject;  // 예: PlacedFirstItem
}
