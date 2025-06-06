using System;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

// 타겟을 감지하는 조건
[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance] ,And Is [target] in [Self] insight( IsStand : [IsStand] )",
        category: "Conditions", id: "80546b317d3fa87b085f9b738446db9c")]
public partial class CheckTargetDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CurrentDistance; // 현재 거리
    [SerializeReference] public BlackboardVariable<float> ChaseDistance; // 추적 거리
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<bool> IsStand; // StandType인지 여부

    // true 반환 시 플레이어를 감지했다고 판단
    public override bool IsTrue()
    {
        Vector3 selfPos = Self.Value.transform.position;
        Vector3 targetPos;
        Collider[] targets = Physics.OverlapSphere(selfPos, ChaseDistance); // 인식 범위에 있는 오브젝트들

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTransform = targets[i].transform;
            targetPos = targetTransform.position;

            // 인식 범위보다 멀리 있거나 플레이어가 아니면 무시
            if (CurrentDistance.Value > ChaseDistance.Value || targetTransform.tag != "Player")
            {
                continue;
            }

            Vector3 direction = (targetPos - selfPos).normalized; // 에이전트와 타겟의 방향
            direction.y = 0; // 좌우 시야만 확인하기 위해 y축 무시(이렇게 하지 않으면 수직 각도도 영향을 줘서 원하는 결과가 안 나옴)
            float angle = Vector3.Angle(Self.Value.transform.forward, direction);
            if (CheckStandType(angle) || CheckCrawlType(Vector3.Distance(targetPos, selfPos)))
            {
                return true;
            }

            // 근처에서 움직이거나 숙이고 있지만 너무 가까이 붙은 경우 true 반환(현재 거리만 확인함)
            // 거리에 들어옴 && 플레이어 움직이는 중 -> true 반환
            if (CheckNearStandType(Vector3.Distance(targetPos, selfPos)))
            {
                return true;
            }
        }

        return false;
    }
    
    // 근처에서 움직이거나 숙이고 있지만 너무 가까이 붙은 경우
    private bool CheckNearStandType(float distance) 
    {
        bool isMove = Target.Value.GetComponent<Player>().isMove;
        if ((IsStand.Value && distance <= (ChaseDistance / 2) && isMove) || distance <= 2f)
        {
            return true;
        }

        return false;
    }

    // StandType 몬스터의 확인 로직
    private bool CheckStandType(float angle)
    {
        if(IsStand.Value == false) { return false; }

        // 플레이어가 서있고 시야각 120도 이내면 true 반환
        if (angle <= 120f * 0.5f && Target.Value.GetComponent<Player>().isCrouch == false)
        {
            return true;
        }

        return false;
    }

    // CrawlType 몬스터의 확인 로직
    private bool CheckCrawlType(float distance)
    {
        if(IsStand.Value) { return false; }

        // 플레이어가 바닥에 있으면 true 반환
        if (Target.Value.GetComponent<Player>().isGround)
        {
            return true;
        }

        return false;
    }
}
