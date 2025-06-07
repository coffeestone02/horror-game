using UnityEngine;
using TMPro;

public class CarEscape : MonoBehaviour
{
    [Header("조건")]
    public AltarManager altarRef;         // 공물 상태 확인용
    public GameObject player;             // 플레이어 위치 확인용
    public float interactRange = 3f;      // E키 상호작용 거리

    [Header("타이머 설정")]
    public float escapeTime = 180f;       // 3분
    public TextMeshProUGUI timerText;     // 타이머 UI
    private float countdown;
    private bool timerRunning = false;
    private bool timerFinished = false;

    [Header("UI")]
    public TextMeshProUGUI escapePromptText; // "Press E to escape!"
    public GameObject clearUI;

    void Start()
    {
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        if (escapePromptText != null)
            escapePromptText.gameObject.SetActive(false);

        if (clearUI != null)
            clearUI.SetActive(false);
    }

    void Update()
    {
        if (altarRef == null || player == null)
            return;

        // 플레이어가 차 근처에서 E키 누를 때
        if (Vector3.Distance(transform.position, player.transform.position) < interactRange &&
            Input.GetKeyDown(KeyCode.E))
        {
            if (altarRef.isFirstItemPlaced && altarRef.isSecondItemPlaced)
            {
                if (!timerRunning && !timerFinished)
                {
                    StartTimer();
                }
                else if (timerFinished)
                {
                    if (clearUI != null)
                        clearUI.SetActive(true);

                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    Time.timeScale = 0f;
                }
            }
        }

        // 타이머 작동
        if (timerRunning)
        {
            countdown -= Time.deltaTime;
            UpdateTimerUI();

            if (countdown <= 0f)
            {
                countdown = 0f;
                timerRunning = false;
                timerFinished = true;

                // UI 전환 처리
                if (timerText != null)
                    timerText.gameObject.SetActive(false);

                if (escapePromptText != null)
                    escapePromptText.gameObject.SetActive(true);
            }
        }
    }

    void StartTimer()
    {
        countdown = escapeTime;
        timerRunning = true;

        if (timerText != null)
            timerText.gameObject.SetActive(true);
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(countdown / 60f);
            int seconds = Mathf.FloorToInt(countdown % 60f);
            timerText.text = $"{minutes:00}:{seconds:00}";
        }
    }
}
