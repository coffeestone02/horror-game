using UnityEngine;

public class ShowButton : MonoBehaviour
{
    [SerializeField] private GameObject interactionButton;

    private void OnTriggerEnter(Collider other)
    {
        interactionButton.SetActive(true);
    }
    
    private void OnTriggerExit(Collider other)
    {
        interactionButton.SetActive(false);
    }
}
