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
    public float overlayFadeSpeed = 1f;

    [Header("Таймер смерти")]
    public float deathDelay = 15f;

    [HideInInspector] public bool hasTurnedOnOnce = false;

    private float timerFlashlight = 0f;
    private float timerNoFlashlight = 0f;
    private bool isShrinking = true;
    private bool noFlashlightCountdown = false;
    private float overlayTargetAlpha = 0f;

    public TextMeshProUGUI flashlightTimerText;
    public TextMeshProUGUI noFlashlightTimerText;

    void Start()
    {
        if (flashlight == null) flashlight = GetComponent<Light2D>();
        if (flashlightScript == null) flashlightScript = GetComponent<Flashlight>();

        flashlight.pointLightOuterRadius = maxRadius;

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);
    }

    void Update()
    {
        float remainingFlashlightTime = 0f;
        float remainingNoFlashlightTime = 0f;

        // --- Таймер фонарика ---
        if (flashlightScript != null && flashlightScript.isOn && isShrinking)
        {
            hasTurnedOnOnce = true;
            timerFlashlight += Time.deltaTime;
            flashlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, timerFlashlight / shrinkDuration);
            remainingFlashlightTime = Mathf.Max(0f, shrinkDuration - timerFlashlight);

            if (timerFlashlight >= shrinkDuration)
            {
                // Радиус достиг 0 → фонарик выключается автоматически
                flashlight.pointLightOuterRadius = minRadius;
                flashlightScript.isOn = false;
                isShrinking = false;

                // Таймер без фонаря запускается
                noFlashlightCountdown = true;
                timerNoFlashlight = 0f;
                overlayTargetAlpha = 0.6f;
            }

            // Пока фонарик включен — таймер без фонаря сбрасывается
            noFlashlightCountdown = false;
            timerNoFlashlight = 0f;
            overlayTargetAlpha = 0f;
        }

        // --- Проверка для таймера без фонаря ---
        if ((flashlightScript != null && !flashlightScript.isOn) || (!isShrinking && hasTurnedOnOnce))
        {
            if (hasTurnedOnOnce)
                noFlashlightCountdown = true;
        }

        // --- Таймер без фонаря ---
        if (noFlashlightCountdown)
        {
            timerNoFlashlight += Time.deltaTime;
            remainingNoFlashlightTime = Mathf.Max(0f, noFlashlightDuration - timerNoFlashlight);
            overlayTargetAlpha = 0.6f;

            if (timerNoFlashlight >= noFlashlightDuration)
            {
                Die();
            }
        }

        // --- UI ---
        if (flashlightTimerText != null)
            flashlightTimerText.text = "Flashlight: " + remainingFlashlightTime.ToString("F1") + "s";

        if (noFlashlightTimerText != null)
            noFlashlightTimerText.text = "No Light: " + remainingNoFlashlightTime.ToString("F1") + "s";

        // --- Overlay ---
        if (overlayImage != null)
        {
            Color c = overlayImage.color;
            c.a = Mathf.MoveTowards(c.a, overlayTargetAlpha, overlayFadeSpeed * Time.deltaTime);
            overlayImage.color = c;
        }
    }

    public void OnCrystalCollected()
    {
        // Собрали батарейку — фонарик снова работает
        flashlight.pointLightOuterRadius = maxRadius;
        timerFlashlight = 0f;
        isShrinking = true;

        noFlashlightCountdown = false;
        timerNoFlashlight = 0f;
        overlayTargetAlpha = 0f;

        flashlightScript.isOn = true;
        Debug.Log("CrystalCollected");
    }

    private void Die()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
