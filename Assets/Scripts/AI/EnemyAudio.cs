using System.Collections;
using Unity.Behavior;
using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] searchClips;
    [SerializeField] private AudioClip[] chaseClips;
    [SerializeField] private BehaviorGraphAgent behaviorAgent;
    private EnemyState enemyState = EnemyState.Idle;

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
            enemyState = (EnemyState)variable.ObjectValue;
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
