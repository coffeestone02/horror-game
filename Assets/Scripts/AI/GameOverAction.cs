using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Collections;

// 게임 오버시 실행되는 액션
[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Game Over", story: "Gameover ( [GameoverCanvas] , [BurningBook] )", category: "Action", id: "99c67095131fd28490a1373061c3f1e1")]
public partial class GameOverAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> GameoverCanvas;
    [SerializeReference] public BlackboardVariable<GameObject> BurningBook;
    private float delay;

    protected override Status OnStart()
    {
        delay = 0f;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        delay += Time.deltaTime;
        if (delay >= 0.5f) // 0.5초 후에 불타는 책을 보여주고
        {
            BurningBook.Value.SetActive(true);
        }
        if (delay >= 1.5f) // 1.5초 후에 게임 오버 화면을 보여줌
        {
            Time.timeScale = 0;
            BurningBook.Value.SetActive(false);
            GameoverCanvas.Value.SetActive(true);
            return Status.Success;
        }

        return Status.Running;
    }
}

