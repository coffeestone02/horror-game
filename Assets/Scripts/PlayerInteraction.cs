using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("상호작용 설정")]
    public float interactRange = 3f;
    public LayerMask interactLayer;
    public Transform holdPoint;

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
        Ray ray = new Ray(transform.position, transform.forward);
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

            // 문 텔레포트 (양방향)
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
            }
        }
    }

    void GrabItem(GameObject item)
    {
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        item.transform.SetParent(holdPoint);
        item.transform.localPosition = Vector3.zero;
        item.transform.localRotation = Quaternion.identity;

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
