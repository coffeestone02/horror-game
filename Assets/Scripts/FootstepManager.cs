using UnityEngine;

[System.Serializable]
public class FootstepSet
{
    public string tagName; // "Wood", "Dirt", "Stone"
    public AudioClip[] walkSounds;
    public AudioClip[] runSounds;
    public AudioClip[] landSounds;
}
public class FootstepManager : MonoBehaviour
{
    public FootstepSet[] footstepSets;
    public AudioSource audioSource;

    private Player player;
    private float stepTimer;
    public float walkInterval = 0.5f;
    public float runInterval = 0.3f;

    private string currentSurfaceTag = "Default";
    private bool wasGrounded = true;

    void Start()
    {
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (player == null) return;

        bool isGrounded = player.IsGrounded();

        // 착지 사운드
        if (isGrounded && !wasGrounded)
        {
            PlayLandingSound();
        }
        wasGrounded = isGrounded;

        // 이동 중일 때만 걷기/달리기 사운드
        if (player.isMove && isGrounded)
        {
            stepTimer -= Time.deltaTime;

            float interval = player.isRun ? runInterval : walkInterval;
            if (stepTimer <= 0f)
            {
                PlayFootstepSound(player.isRun);
                stepTimer = interval;
            }
        }
        else
        {
            stepTimer = 0f;
        }
    }

    void PlayFootstepSound(bool isRunning)
    {
        UpdateSurfaceTag();
        FootstepSet set = GetCurrentSet();
        if (set == null) return;

        AudioClip[] clips = isRunning ? set.runSounds : set.walkSounds;
        if (clips.Length > 0)
        {
            AudioClip clip = clips[Random.Range(0, clips.Length)];
            audioSource.PlayOneShot(clip);
        }
    }

    void PlayLandingSound()
    {
        UpdateSurfaceTag();
        FootstepSet set = GetCurrentSet();
        if (set == null || set.landSounds.Length == 0) return;

        AudioClip clip = set.landSounds[Random.Range(0, set.landSounds.Length)];
        audioSource.PlayOneShot(clip);
    }

    void UpdateSurfaceTag()
    {
        Ray ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 2f))
        {
            currentSurfaceTag = hit.collider.tag;
        }
    }

    FootstepSet GetCurrentSet()
    {
        foreach (var set in footstepSets)
        {
            if (set.tagName == currentSurfaceTag)
                return set;
        }
        return null;
    }
}
