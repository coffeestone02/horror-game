using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Transform entryTarget;  // 들어가는 위치
    public Transform exitTarget;   // 나오는 위치

    public enum Direction { Entry, Exit }

    public Direction GetTeleportDirection(Vector3 playerPos)
    {
        Vector3 doorForward = transform.forward;
        Vector3 toPlayer = playerPos - transform.position;

        // Dot Product 조건을 반대로 바꿔줌
        return Vector3.Dot(doorForward, toPlayer) < 0 ? Direction.Entry : Direction.Exit;
    }


    public Transform GetTarget(Vector3 playerPos)
    {
        return GetTeleportDirection(playerPos) == Direction.Entry ? entryTarget : exitTarget;
    }
}
