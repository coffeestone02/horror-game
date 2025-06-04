using UnityEngine;

public class AltarManager : MonoBehaviour
{
    public bool isFirstItemPlaced = false;
    public bool isSecondItemPlaced = false;

    public int monsterState = 1; // 1단계: 아무것도 없음, 2단계: 하나, 3단계: 둘 다

    private int lastState = -1; // 상태 변경 감지용

    void Update()
    {
        UpdateMonsterState();
    }

    void UpdateMonsterState()
    {
        int currentState;

        if (isFirstItemPlaced && isSecondItemPlaced)
            currentState = 3;
        else if (isFirstItemPlaced || isSecondItemPlaced)
            currentState = 2;
        else
            currentState = 1;

        // 상태가 바뀌었을 때만 디버그 출력
        if (currentState != lastState)
        {
            Debug.Log("몬스터 상태 단계: " + currentState);
            lastState = currentState;
        }

        monsterState = currentState;
    }
}
