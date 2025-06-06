using UnityEngine;

public class GameOver : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("앙기모띠");
        }
    }
}
