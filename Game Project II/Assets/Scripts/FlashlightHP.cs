using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using TMPro;

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
    public float overlayFadeSpeed = 1f; // скорость плавного затемнения

    [Header("Таймер смерти")]
    public float deathDelay = 15f;

    [HideInInspector] public bool hasTurnedOnOnce = false;

    // Таймеры
    private float timerFlashlight = 0f;
    private float timerNoFlashlight = 0f;
    private bool isShrinking = true;
    private bool isDeadCountdown = false;
    private bool noFlashlightCountdown = false;

    // UI таймеры
    public TextMeshProUGUI flashlightTimerText;
    public TextMeshProUGUI noFlashlightTimerText;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light2D>();
        if (flashlightScript == null)
            flashlightScript = GetComponent<Flashlight>();

        flashlight.pointLightOuterRadius = maxRadius;

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);
    }

    void Update()
    {
        // --- Проверка состояния фонарика ---
        if (flashlightScript != null)
        {
            if (flashlightScript.isOn)
            {
                hasTurnedOnOnce = true;

                // Фонарик включен → таймер без света сбрасывается
                noFlashlightCountdown = false;
                timerNoFlashlight = 0f;
            }
            else
            {
                // Фонарик выключен вручную или автоматически, запускаем таймер без света
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
                isDeadCountdown = true;
                timerFlashlight = 0f;

                // Таймер без света запускается автоматически
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
                Die();
            }
        }

        // --- Обновление UI ---
        if (flashlightTimerText != null)
            flashlightTimerText.text = "Flashlight: " + remainingFlashlightTime.ToString("F1") + "s";

        if (noFlashlightTimerText != null)
            noFlashlightTimerText.text = "No Light: " + remainingNoFlashlightTime.ToString("F1") + "s";

        // --- Плавное затемнение экрана ---
        if (overlayImage != null)
        {
            float targetAlpha = 0f;
            if (noFlashlightCountdown)
            {
                // Чем меньше времени осталось без света, тем темнее
                float t = Mathf.Clamp01(timerNoFlashlight / noFlashlightDuration);
                targetAlpha = Mathf.Lerp(0f, 0.8f, t); // 0.8 – максимальная темнота
            }
            overlayImage.color = new Color(0f, 0f, 0f, Mathf.MoveTowards(overlayImage.color.a, targetAlpha, overlayFadeSpeed * Time.deltaTime));
        }
    }

    public void OnCrystalCollected()
    {
        flashlight.pointLightOuterRadius = maxRadius;
        timerFlashlight = 0f;
        isShrinking = true;
        isDeadCountdown = false;

        noFlashlightCountdown = false;
        timerNoFlashlight = 0f;

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);

        Debug.Log("CrystalCollected");
    }

    private void Die()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
