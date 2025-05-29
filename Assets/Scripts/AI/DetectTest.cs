using UnityEngine;

public class DetectTest : MonoBehaviour
{
    public float distance = 6f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 6f);
    }
}
