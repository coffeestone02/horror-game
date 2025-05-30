using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckTargetDetect", story: "Compare values of [CurrentDistance] and [ChaseDistance] ,And Is target in [Self] insight( [ViewAngle] )",
        category: "Conditions", id: "80546b317d3fa87b085f9b738446db9c")]
public partial class CheckTargetDetectCondition : Condition
{
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;
    [SerializeReference] public BlackboardVariable<float> ChaseDistance;
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<float> ViewAngle;

    public override bool IsTrue()
    {
        Collider[] targets = Physics.OverlapSphere(Self.Value.transform.position, ChaseDistance); // 범위에 있는 오브젝트들

        for (int i = 0; i < targets.Length; i++)
        {
            Transform targetTransform = targets[i].transform;
            if (CurrentDistance.Value > ChaseDistance.Value || targetTransform.name != "Player") // 인식 범위보다 멀리 있거나 플레이어가 아니면 무시
            {
                continue;
            }

            Vector3 direction = (targetTransform.position - Self.Value.transform.position).normalized;
            float angle = Vector3.Angle(Self.Value.transform.forward + Self.Value.transform.up * 1.0f, direction); // AI와 타겟 사이의 각도를 계산
            Debug.DrawRay(Self.Value.transform.position + Self.Value.transform.up * 1.0f, direction, Color.red);
            if (angle <= ViewAngle.Value * 0.5f) // 시야각 범위 내에 있는지 확인
            {
                Debug.Log("플레이어 포착");
                return true;
            }
            else
            {
                Debug.Log("플레이어 못봄");
                return false;
            }

            // 근처에서 달리면 true 반환하는 코드 작성해야함
        }

        return false;
    }
}
