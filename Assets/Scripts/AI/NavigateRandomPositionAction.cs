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

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        animator = Self.Value.GetComponent<Animator>();
        idx = UnityEngine.Random.Range(0, PatrolPoints.Value.Count);
        if (agent.pathPending == false && agent.remainingDistance <= agent.stoppingDistance)
        {
            agent.SetDestination(PatrolPoints.Value[idx].transform.position);
        }

        Debug.Log(agent.velocity.magnitude);
        if (agent.velocity.magnitude <= 0.01f)
        {
            animator.SetBool("isWalk", false);
        }
        return Status.Running;
    }
}

