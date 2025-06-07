using UnityEngine;

// 플레이어가 특정 영역에 들어오면 상호작용 버튼을 보여주고, E 키를 눌러 개발자 정보를 확인하는 스크립트
public class ShowButton : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton; // 상호작용 버튼 오브젝트
    [SerializeField] private GameObject developers; // 개발자 정보 오브젝트
    private bool canInteract;

    private void OnTriggerEnter(Collider other)
    {
        interactionButton.SetActive(true);
        canInteract = true;
    }

    private void Update()
    {
        if (canInteract && developers != null && Input.GetKeyDown(KeyCode.E))
        {
            developers.SetActive(!developers.activeSelf);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        canInteract = false;
        if (developers != null)
        {
            developers.SetActive(false);
        }
        interactionButton.SetActive(false);
    }
}
