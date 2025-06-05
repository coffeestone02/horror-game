using UnityEngine;

public class AxisTransformer : MonoBehaviour
{
    void Update()
    {
        Vector3 localZ = transform.InverseTransformDirection(Vector3.forward);
        transform.rotation = Quaternion.LookRotation(localZ);
    }
}
