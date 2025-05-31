using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Wander", story: "[Self] Navigate To WanderPosition", category: "Action", id: "e59110938980425ea76c08c33a4df10a")]
public partial class WanderAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private NavMeshAgent agent;
    private Vector3 wanderPostion;
    private float currentWanderTime = 0f;
    private float maxWanderTime = 5f;

    protected override Status OnStart()
    {
        int jitterMin = 0;
        int jitterMax = 360;
        float wanderRadius = UnityEngine.Random.Range(2.5f, 4f);
        int wanderJitter = UnityEngine.Random.Range(jitterMin, jitterMax);

        wanderPostion = Self.Value.transform.position + GetPositionFromAngle(wanderRadius, wanderJitter);
        agent = Self.Value.GetComponent<NavMeshAgent>();
        agent.SetDestination(wanderPostion);
        currentWanderTime = Time.time;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if ((wanderPostion - Self.Value.transform.position).sqrMagnitude < 0.1f || Time.time - currentWanderTime > maxWanderTime)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    private Vector3 GetPositionFromAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        angle = DegreeToRadian(angle);
        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    private float DegreeToRadian(float angle)
    {
        return Mathf.PI * angle / 180;
    }
}

