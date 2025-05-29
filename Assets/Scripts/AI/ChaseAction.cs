using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Self] Navigate To [Target]", category: "Action", id: "a403f30a2feac382017b39125a467ab6")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;

    private NavMeshAgent agent;
    private Animator animator;

    protected override Status OnStart()
    {
        animator = Self.Value.GetComponentInChildren<Animator>();
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.speed = 5f;
        agent.SetDestination(Target.Value.transform.position);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        animator.SetFloat("MoveSpeed", agent.speed);

        return Status.Running;
    }

    protected override void OnEnd()
    {
        animator.SetFloat("MoveSpeed", 0f);
    }
}

