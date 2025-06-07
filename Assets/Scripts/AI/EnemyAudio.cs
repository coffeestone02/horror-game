using System.Collections;
using Unity.Behavior;
using UnityEngine;

// 적의 오디오 관리 스크립트
public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] searchClips; // 탐색 중 소리
    [SerializeField] private AudioClip[] chaseClips; // 추적 중 소리
    [SerializeField] private BehaviorGraphAgent behaviorAgent;
    private EnemyState enemyState = EnemyState.Idle; // 현재 적 상태

    void Start()
    {
        StartCoroutine(PlaySound());
    }

    IEnumerator PlaySound()
    {
        while (true)
        {
            int idx;
            BlackboardVariable variable;
            behaviorAgent.GetVariable("currentState", out variable);
            enemyState = (EnemyState)variable.ObjectValue; // 현재 적 상태 가져오기

            switch (enemyState)
            {
                case EnemyState.Idle:
                case EnemyState.Patrol:
                    idx = Random.Range(0, searchClips.Length);
                    audioSource.PlayOneShot(searchClips[idx]);
                    yield return new WaitForSeconds(Random.Range(3f, 8f));
                    break;
                case EnemyState.Chase:
                    idx = Random.Range(0, chaseClips.Length);
                    audioSource.PlayOneShot(chaseClips[idx]);
                    yield return new WaitForSeconds(Random.Range(2f, 4f));
                    break;
                default:
                    break;
            }
        }
    }
}
