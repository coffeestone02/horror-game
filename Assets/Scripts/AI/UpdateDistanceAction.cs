using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "UpdateDistance", story: "Update [Self] and [Target] [CurrentDistance]", category: "Action", id: "e50f5d54997ff6ca564cdaeed7c09e73")]
public partial class UpdateDistanceAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<float> CurrentDistance;

    private NavMeshAgent agent;
    private GameObject target;

    protected override Status OnStart()
    {
        agent = Self.Value.GetComponent<NavMeshAgent>();
        target = Target;

        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        CurrentDistance.Value = Vector3.Distance(Self.Value.transform.position, Target.Value.transform.position);

        return Status.Success;
    }
}

