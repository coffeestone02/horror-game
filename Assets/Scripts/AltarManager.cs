using UnityEngine;

public class AltarManager : MonoBehaviour
{
    public bool isFirstItemPlaced = false;
    public bool isSecondItemPlaced = false;

    public int monsterState = 1; // 1�ܰ�: �ƹ��͵� ����, 2�ܰ�: �ϳ�, 3�ܰ�: �� ��

    private int lastState = -1; // ���� ���� ������

    public GameObject standType1;
    public GameObject standType2;
    public GameObject standType3;

    private UpdateEnemyRageState standTypePhase1;
    private UpdateEnemyRageState standTypePhase2;
    private UpdateEnemyRageState standTypePhase3;

    void Start()
    {
        standTypePhase1 = standType1.GetComponent<UpdateEnemyRageState>();
        standTypePhase2 = standType2.GetComponent<UpdateEnemyRageState>();
        standTypePhase3 = standType3.GetComponent<UpdateEnemyRageState>();
    }

    void Update()
    {
        UpdateMonsterState();
    }

    void UpdateMonsterState()
    {
        int currentState;

        if (isFirstItemPlaced && isSecondItemPlaced)
        {
            currentState = 3;
            standTypePhase1.SetSecondPhase();
            standTypePhase2.SetSecondPhase();
            standTypePhase3.SetSecondPhase();
        }
        else if (isFirstItemPlaced || isSecondItemPlaced)
        {
            currentState = 2;
            standTypePhase1.SetFirstPhase();
            standTypePhase2.SetFirstPhase();
            standTypePhase3.SetFirstPhase();
        }
        else
        {
            currentState = 1;
        }

        // ���°� �ٲ���� ���� ����� ���
        if (currentState != lastState)
        {
            Debug.Log("���� ���� �ܰ�: " + currentState);
            lastState = currentState;
        }

        monsterState = currentState;
    }
}
