using UnityEngine;

public class CameraBob : MonoBehaviour
{
    [Header("기본 설정")]
    public float baseFrequency = 10f;

    [Header("흔들림 세기")]
    public float xAmplitude = 0.03f;  // 좌우 기본 세기
    public float yAmplitude = 0.05f;  // 위아래 기본 세기

    [Header("상태별 Y축 배율")]
    public float walkYMultiplier = 1f;
    public float runYMultiplier = 3f;
    public float crouchYMultiplier = 0.3f;

    [Header("상태별 X축 배율")]
    public float walkXMultiplier = 2f;   // 걸을 때 좌우 흔들림 2배
    public float runXMultiplier = 1f;
    public float crouchXMultiplier = 0.3f;

    [Header("속도 배율 (상태별 주파수 가중치)")]
    public float walkSpeedMultiplier = 1f;
    public float runSpeedMultiplier = 2f;
    public float crouchSpeedMultiplier = 0.6f;

    private Vector3 originalPos;
    private Player player;

    void Start()
    {
        originalPos = transform.localPosition;
        player = GetComponentInParent<Player>();
    }

    void Update()
    {
        if (player == null || !player.isMove || !player.IsGrounded())
        {
            transform.localPosition = originalPos;
            return;
        }

        float frequency = baseFrequency;
        float xMult = walkXMultiplier;
        float yMult = walkYMultiplier;
        float speedMult = walkSpeedMultiplier;

        if (player.isRun)
        {
            xMult = runXMultiplier;
            yMult = runYMultiplier;
            speedMult = runSpeedMultiplier;
        }
        else if (player.isCrouch)
        {
            xMult = crouchXMultiplier;
            yMult = crouchYMultiplier;
            speedMult = crouchSpeedMultiplier;
        }

        frequency *= speedMult;

        float offsetY = Mathf.Sin(Time.time * frequency) * yAmplitude * yMult;
        float offsetX = Mathf.Cos(Time.time * frequency * 0.5f) * xAmplitude * xMult;

        transform.localPosition = originalPos + new Vector3(offsetX, offsetY, 0f);
    }
}
