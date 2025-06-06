using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

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
        agent.SetDestination(Target.Value.transform.position);
        agent.speed = ChaseSpeed.Value;

        return Status.Running;
    }
}

