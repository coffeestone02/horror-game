using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange = 3f;
    public LayerMask interactLayer;
    public Transform holdPoint;
    [SerializeField] private Camera cam;

    [Header("획득 여부")]
    public bool hasFirstItem = false;
    public bool hasSecondItem = false;

    private GameObject heldItem;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TryInteract();
        }
    }

    void TryInteract()
    {
        Ray ray = new Ray(cam.transform.position, cam.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, interactRange, interactLayer))
        {
            GameObject target = hit.collider.gameObject;

            // 아이템 잡기
            if (heldItem == null && target.CompareTag("Item"))
            {
                GrabItem(target);
                return;
            }

            // 문 텔레포트
            if (target.CompareTag("Door"))
            {
                DoorTeleport door = target.GetComponent<DoorTeleport>();
                if (door != null)
                {
                    Transform tpTarget = door.GetTarget(transform.position);
                    if (tpTarget != null)
                    {
                        transform.position = tpTarget.position;
                    }
                }
                return;
            }

            // 제단 상호작용
            if (target.CompareTag("Altar"))
            {
                if (heldItem != null)
                {
                    AltarSlot slot = target.GetComponent<AltarSlot>();
                    if (slot != null && heldItem.name == slot.requiredItemName)
                    {
                        // 공물 파괴
                        Destroy(heldItem);
                        heldItem = null;

                        //  bool 값 초기화 (플레이어 상태)
                        if (slot.requiredItemName == "firstItem") hasFirstItem = false;
                        else if (slot.requiredItemName == "secondItem") hasSecondItem = false;

                        // 슬롯의 연출 처리 + 상태 반영
                        slot.PlaceItem();

                        Debug.Log($"{slot.requiredItemName} successfully placed on altar.");
                    }
                    else
                    {
                        Debug.Log("Wrong item for this altar slot.");
                    }
                }
            }
        }
    }

    void GrabItem(GameObject item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;

        heldItem = item;

        if (item.name == "firstItem")
        {
            hasFirstItem = true;
        }
        else if (item.name == "secondItem")
        {
            hasSecondItem = true;
        }
    }
}
