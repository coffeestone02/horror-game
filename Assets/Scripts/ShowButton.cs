using UnityEngine;

public class ShowButton : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;
    [SerializeField] private GameObject developers;
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
