using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

// Chase 액션(플레이어 쫓아갈 때 사용)
// Self가 Target을 ChaseSpeed의 속도로 쫓아감
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] Navigate To [Target] ( Chase Speed: [ChaseSpeed] )", category: "Action", id: "a403f30a2feac382017b39125a467ab6")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> ChaseSpeed;

    private NavMeshAgent agent;

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(Target.Value.transform.position); // 에이전트 목적지 설정
        agent.speed = ChaseSpeed.Value; // 에이전트 속도 설정

        return Status.Running;
    }
}

