using System;
using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using Unity.VisualScripting;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Navigate Random Position", story: "[Self] navigate to Random [PatrolPoints] ", category: "Action", id: "c683b0cd47acaca0217acfcab898677a")]
public partial class NavigateRandomPositionAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<List<GameObject>> PatrolPoints;

    private int idx;
    private NavMeshAgent agent;
    private Animator animator;
    private bool isWalk;
    private float speed = 3.5f;

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        animator = Self.Value.GetComponent<Animator>();
        idx = UnityEngine.Random.Range(0, PatrolPoints.Value.Count);
        if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(PatrolPoints.Value[idx].transform.position);
        }

        isWalk = (agent.velocity.magnitude <= 1f) ? isWalk = false : isWalk = true;
        animator.SetBool("isWalk", isWalk);
        agent.speed = speed;

        return Status.Running;
    }
}

