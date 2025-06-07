using UnityEngine;
using UnityEngine.AI;

// 회전 문제가 있는 NavMeshAgent의 방향을 올바르게 고쳐주기 위한 스크립트
public class FaceTarget : MonoBehaviour
{
    public NavMeshAgent agent;

    void Start()
    {
        agent.updateRotation = false;
    }

    void Update()
    {
        // 이동 방향 계산
        Vector3 moveDir = agent.velocity;
        moveDir.y = 0;
        if (moveDir.sqrMagnitude > 0.001f)
        {
            // 이동 방향을 바라보는 회전값
            Quaternion lookRot = Quaternion.LookRotation(moveDir);

            // 모델의 앞이 오른쪽(X축)이라면, Y축으로 -90도 오프셋 적용
            Quaternion offset = Quaternion.Euler(0, 90, 0);

            // 회전 적용
            transform.rotation = lookRot * offset;
        }
    }
}
