using System;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance] ,And Is [target] in [Self] insight( IsStand : [IsStand] )",
        category: "Conditions", id: "80546b317d3fa87b085f9b738446db9c")]
public partial class CheckTargetDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    [SerializeReference] public BlackboardVariable<float> ChaseDistance;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> IsStand;

    private bool isClose;

    public override bool IsTrue()
    {
        Collider[] targets = Physics.OverlapSphere(Self.Value.transform.position, ChaseDistance); // 범위에 있는 오브젝트들

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTransform = targets[i].transform;

            // 인식 범위보다 멀리 있거나 플레이어가 아니면 무시
            if (CurrentDistance.Value > ChaseDistance.Value || targetTransform.tag != "Player")
            {
                continue;
            }

            Vector3 direction = (targetTransform.position - Self.Value.transform.position).normalized; // 에이전트와 타겟의 방향
            direction.y = 0; // 좌우 시야만 확인하기 위해 y축 무시(이렇게 하지 않으면 수직 각도도 영향을 줌)
            float angle = Vector3.Angle(Self.Value.transform.forward, direction);
            Debug.DrawRay(Self.Value.transform.position, direction, Color.red);
            if ((IsStand.Value && CheckStandType(angle)) || (IsStand.Value == false && CheckCrawlType(angle)))
            {
                return true;
            }

            // 근처에서 달리거나 웅크려있지만 너무 가까이 붙은 경우 true 반환
            if (isClose)
            {
                return true;
            }
        }

        return false;
    }

    private void OnTriggerEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isClose = true;
            Debug.Log("너무 가까이 붙음");
        }
    }

    private void OnTriggerExit(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            isClose = false;
            Debug.Log("붙었다가 떨어짐");
        }
    }

    // StandType 몬스터의 확인 로직
    private bool CheckStandType(float angle)
    {
        if (angle <= 90f * 0.5f) // Target.Value.isCrouch == false 조건 추가 예정
        {
            Debug.Log("플레이어 포착");
            return true;
        }

        Debug.Log("시야 범위 내에 있으나 플레이어를 못봄");
        return false;
    }

    // CrawlType 몬스터의 확인 로직
    private bool CheckCrawlType(float angle)
    {
        float heightGap = Target.Value.transform.position.y - Self.Value.transform.position.y;
        Debug.Log(heightGap);
        if (angle <= 90f * 0.5f && heightGap < 2f)
        {
            Debug.Log("플레이어 포착");
            return true;
        }

        return false;
    }
}
