using UnityEngine;

// 테스트용
public class DetectTest : MonoBehaviour
{
    public float distance = 6f;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, distance);
    }
}
