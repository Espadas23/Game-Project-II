
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class FlashlightHP : MonoBehaviour
{
    [Header("Свет")]
    public Light2D flashlight;
    public Flashlight flashlightScript;
    public float maxRadius = 5f;
    public float minRadius = 0f;
    public float shrinkDuration = 10f;

    [Header("Таймер смерти без фонаря")]
    public float noFlashlightDuration = 15f;

    [Header("Overlay для затемнения")]
    public Image overlayImage;
    public float overlayFadeSpeed = 1f;

    [Header("UI таймеры")]
    public TextMeshProUGUI flashlightTimerText;
    public TextMeshProUGUI noFlashlightTimerText;

    [Header("GameOver UI")]
    public GameObject gameOverPanel;

    [HideInInspector] public bool hasTurnedOnOnce = false;

    // Таймеры
    private float timerFlashlight = 0f;
    private float timerNoFlashlight = 0f;
    private bool isShrinking = true;
    private bool noFlashlightCountdown = false;

    private bool isGameOver = false;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light2D>();
        if (flashlightScript == null)
            flashlightScript = GetComponent<Flashlight>();

        flashlight.pointLightOuterRadius = maxRadius;

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);

        if (gameOverPanel != null)
        {
            CanvasGroup cg = gameOverPanel.GetComponent<CanvasGroup>();
            if(cg != null) cg.alpha = 0f;
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        if (isGameOver) return;

        // --- Проверка состояния фонарика ---
        if (flashlightScript != null)
        {
            if (flashlightScript.isOn)
            {
                hasTurnedOnOnce = true;
                noFlashlightCountdown = false;
                timerNoFlashlight = 0f;
            }
            else
            {
                // Фонарик выключен или радиус = 0 → запускаем таймер без фонаря
                if (hasTurnedOnOnce && !noFlashlightCountdown)
                {
                    noFlashlightCountdown = true;
                    timerNoFlashlight = 0f;
                }
            }
        }

        // --- Таймер фонарика ---
        float remainingFlashlightTime = 0f;
        if (flashlightScript != null && flashlightScript.isOn && isShrinking)
        {
            timerFlashlight += Time.deltaTime;
            float t = Mathf.Clamp01(timerFlashlight / shrinkDuration);
            flashlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, t);

            remainingFlashlightTime = Mathf.Max(0f, shrinkDuration - timerFlashlight);

            if (timerFlashlight >= shrinkDuration)
            {
                isShrinking = false;
                flashlightScript.isOn = false; // фонарь выключается автоматически
                noFlashlightCountdown = true;
                timerNoFlashlight = 0f;
            }
        }

        // --- Таймер без фонаря ---
        float remainingNoFlashlightTime = 0f;
        if (noFlashlightCountdown)
        {
            timerNoFlashlight += Time.deltaTime;
            remainingNoFlashlightTime = Mathf.Max(0f, noFlashlightDuration - timerNoFlashlight);

            if (timerNoFlashlight >= noFlashlightDuration)
            {
                TriggerGameOver();
            }
        }

        // --- Обновление UI таймеров ---
        if (flashlightTimerText != null)
            flashlightTimerText.text = "Flashlight: " + remainingFlashlightTime.ToString("F1") + "s";

        if (noFlashlightTimerText != null)
            noFlashlightTimerText.text = "No Light: " + remainingNoFlashlightTime.ToString("F1") + "s";

        // --- Плавное затемнение ---
        if (overlayImage != null)
        {
            float targetAlpha = 0f;
            if (noFlashlightCountdown)
            {
                float t = Mathf.Clamp01(timerNoFlashlight / noFlashlightDuration);
                targetAlpha = Mathf.Lerp(0f, 0.8f, t);
            }
            overlayImage.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(overlayImage.color.a, targetAlpha, overlayFadeSpeed * Time.deltaTime));
        }
    }
    
    public void OnCrystalCollected()
    {
        if (flashlight != null)
        {
            // Включаем фонарь и восстанавливаем радиус
            flashlight.enabled = true;
            flashlight.pointLightOuterRadius = maxRadius;
        }

        if (flashlightScript != null)
        {
            // Обновляем состояние фонаря
            flashlightScript.RestoreRadius(maxRadius); // Сбрасывает таймер, разрешает включение/выключение
            flashlightScript.isOn = true;             // Включаем фонарь
            flashlightScript.canTurnOnFlashlight = true; 
            flashlightScript.isBurnedOut = false;
        }

        // Обновляем контроллер фонаря для подсветки объектов, если есть
        FlashlightController fc = flashlight.GetComponent<FlashlightController>();
        if (fc != null)
            fc.isOn = true;

        // Сбрасываем таймеры
        timerFlashlight = 0f;
        isShrinking = true;

        noFlashlightCountdown = false;
        timerNoFlashlight = 0f;

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);

        Debug.Log("CrystalCollected");
    }
    
    private void TriggerGameOver()
    {
        isGameOver = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            CanvasGroup cg = gameOverPanel.GetComponent<CanvasGroup>();
            if(cg != null) cg.alpha = 1f;
        }

        // Блокируем действия персонажа, но оставляем Time.timeScale = 1 для работы кнопок
    }

    // --- Кнопка Restart ---
    //public void RestartGame()
    //{
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        //Time.timeScale = 1f;
    //}
    
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }



    // --- Кнопка Exit ---
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}















