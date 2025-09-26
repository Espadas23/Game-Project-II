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

    [HideInInspector] public bool hasTurnedOnOnce = false;

    // Таймеры
    private float timerFlashlight = 0f;
    private float timerNoFlashlight = 0f;
    private bool isShrinking = true;
    private bool noFlashlightCountdown = false;

    // блокировка возможности включения фонаря пока батарейка не собрана
    private bool canTurnOn = true;

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

        canTurnOn = true;
    }

    void Update()
    {
        // Если фонарик "заблокирован" (сгорел), не даем ему оставаться включенным
        if (flashlightScript != null && !canTurnOn && flashlightScript.isOn)
        {
            flashlightScript.isOn = false;
        }

        // --- Проверка состояния фонарика ---
        if (flashlightScript != null)
        {
            // если можно включать и фонарик включён, считаем что игрок включал его хотя бы раз
            if (canTurnOn && flashlightScript.isOn && flashlight.pointLightOuterRadius > 0.01f)
            {
                hasTurnedOnOnce = true;

                // Фонарик включен и есть радиус → сброс таймера без света
                noFlashlightCountdown = false;
                timerNoFlashlight = 0f;
            }
            else
            {
                // Фонарик выключен вручную или радиус 0 → запускаем таймер без света (если ранее включали)
                if (hasTurnedOnOnce && !noFlashlightCountdown)
                {
                    noFlashlightCountdown = true;
                    timerNoFlashlight = 0f;
                }
            }
        }

        // --- Таймер фонарика (идет только когда фонарь включен и разрешено включение) ---
        float remainingFlashlightTime = 0f;
        if (flashlightScript != null && flashlightScript.isOn && canTurnOn && isShrinking)
        {
            timerFlashlight += Time.deltaTime;
            float t = Mathf.Clamp01(timerFlashlight / shrinkDuration);
            flashlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, t);

            remainingFlashlightTime = Mathf.Max(0f, shrinkDuration - timerFlashlight);

            if (timerFlashlight >= shrinkDuration)
            {
                // радиус исчерпан — ставим в min, выключаем фонарь и блокируем повторное включение
                flashlight.pointLightOuterRadius = minRadius;
                flashlightScript.isOn = false;
                canTurnOn = false;
                isShrinking = false;
                timerFlashlight = 0f;

                // запускаем таймер без света
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

        // --- Плавное затемнение экрана (пропорционально таймеру без фонаря) ---
        if (overlayImage != null)
        {
            float targetAlpha = 0f;
            if (noFlashlightCountdown)
            {
                float t = Mathf.Clamp01(timerNoFlashlight / noFlashlightDuration);
                targetAlpha = Mathf.Lerp(0f, 0.8f, t); // от прозрачного до почти черного
            }
            overlayImage.color = new Color(
                0f, 0f, 0f,
                Mathf.MoveTowards(overlayImage.color.a, targetAlpha, overlayFadeSpeed * Time.deltaTime)
            );
        }
    }

    // Вызывается кристаллом при сборе батарейки/кристалла
    public void OnCrystalCollected()
    {
        // восстановление радиуса и разрешение снова включать фонарь
        flashlight.pointLightOuterRadius = maxRadius;
        timerFlashlight = 0f;
        isShrinking = true;

        noFlashlightCountdown = false;
        timerNoFlashlight = 0f;

        canTurnOn = true; // теперь фонарь можно включать
        // НЕ ставим автоматически flashlightScript.isOn = true — пусть игрок включит

        if (overlayImage != null)
            overlayImage.color = new Color(0f, 0f, 0f, 0f);

        Debug.Log("CrystalCollected - flashlight restored");
    }

    private void Die()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
