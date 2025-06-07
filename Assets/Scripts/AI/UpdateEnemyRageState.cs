using Unity.Behavior;
using UnityEngine;

// 공물 획득 시 적의 상태를 업데이트하는 스크립트
public class UpdateEnemyRageState : MonoBehaviour
{
    [SerializeField] private BehaviorGraphAgent behaviorAgent;
    [SerializeField] private float firstPatrolSpeed;
    [SerializeField] private float firstChaseSpeed;
    [SerializeField] private float secondPatrolSpeed;
    [SerializeField] private float secondChaseSpeed;

    public void SetFirstPhase()
    {
        behaviorAgent.SetVariableValue("patrolSpeed", firstPatrolSpeed);
        behaviorAgent.SetVariableValue("chaseSpeed", firstChaseSpeed);
    }

    public void SetSecondPhase()
    {
        behaviorAgent.SetVariableValue("patrolSpeed", secondPatrolSpeed);
        behaviorAgent.SetVariableValue("chaseSpeed", secondChaseSpeed);
    }
}
