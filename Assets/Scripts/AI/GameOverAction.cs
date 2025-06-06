using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Game Over", story: "Gameover ( [GameoverCanvas] )", category: "Action", id: "99c67095131fd28490a1373061c3f1e1")]
public partial class GameOverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> GameoverCanvas;
    private float delay;

    protected override Status OnStart()
    {
        delay = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        delay += Time.deltaTime;
        if (delay >= 0.5f)
        {
            Time.timeScale = 0;
            GameoverCanvas.Value.SetActive(true);
            return Status.Success;
        }

        return Status.Running;
    }
}

