using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Unity.VisualScripting;

// 에이전트가 지정된 랜덤 위치로 이동하는 액션
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Navigate Random Position", story: "[Self] navigate to Random [PatrolPoints] ( Patrol Speed: [PatrolSpeed] )", category: "Action", id: "c683b0cd47acaca0217acfcab898677a")]
public partial class NavigateRandomPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPoints; // 순찰 지점 리스트
    [SerializeReference] public BlackboardVariable<float> PatrolSpeed; // 순찰 속도

    private int idx; // 현재 순찰 지점 인덱스
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWalk;

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        animator = Self.Value.GetComponent<Animator>();
        idx = UnityEngine.Random.Range(0, PatrolPoints.Value.Count); // 랜덤으로 순찰 지점 인덱스 설정
        if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance) // 순찰을 완료했을 때 새 경로로 출발
        {
            agent.SetDestination(PatrolPoints.Value[idx].transform.position);
        }

        isWalk = (agent.velocity.magnitude <= 1f) ? isWalk = false : isWalk = true; // 에이전트 애니메이션 업데이트
        animator.SetBool("isWalk", isWalk);
        agent.speed = PatrolSpeed.Value; // 순찰 속도 설정

        return Status.Running;
    }
}

