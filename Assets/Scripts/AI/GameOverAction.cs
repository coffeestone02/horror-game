using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Game Over", story: "Gameover ( [Gameover Canvas] )", category: "Action", id: "99c67095131fd28490a1373061c3f1e1")]
public partial class GameOverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> GameoverCanvas;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

