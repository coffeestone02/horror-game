using UnityEngine;

public class DoorTeleport : MonoBehaviour
{
    public Transform entryTarget;  // ????? ???
    public Transform exitTarget;   // ?????? ???

    public enum Direction { Entry, Exit }

    public Direction GetTeleportDirection(Vector3 playerPos)
    {
        Vector3 doorForward = transform.forward;
        Vector3 toPlayer = playerPos - transform.position;

        // Dot Product ?????? ???? ?????
        return Vector3.Dot(doorForward, toPlayer) < 0 ? Direction.Entry : Direction.Exit;
    }


    public Transform GetTarget(Vector3 playerPos)
    {
        return GetTeleportDirection(playerPos) == Direction.Entry ? entryTarget : exitTarget;
    }
}
