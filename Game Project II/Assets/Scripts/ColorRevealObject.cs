/*using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class ColorRevealObject : MonoBehaviour
{
    [Header("References")]
    public Light2D flashlightLight;               // фонарик (Light2D)
    public Transform flashlightTransform;         // позиция и направление фонаря
    public TextMeshProUGUI revealText;            // текст с надписью

    [Header("Допустимые цвета")]
    public bool reactsToWhite = false;
    public bool reactsToRed = false;
    public bool reactsToBlue = false;
    public bool reactsToGreen = false;

    [Header("Эффект текста")]
    public float fadeSpeed = 3f;                  // скорость появления/исчезновения
    public float glowIntensity = 1.5f;            // сила свечения текста

    private float targetAlpha = 0f;

    void Start()
    {
        if (revealText != null)
        {
            revealText.alpha = 0f; // изначально скрыт
            revealText.fontMaterial.EnableKeyword("GLOW_ON");
            revealText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
        }
    }

    void Update()
    {
        if (flashlightLight == null || flashlightTransform == null || revealText == null)
            return;

        bool shouldReveal = false;

        // 🔴 Проверка: если фонарь выключен, объект сразу скрывается
        if (!flashlightLight.enabled)
        {
            SetTextVisible(false);
            return;
        }

        // --- 1. Проверка цвета ---
        bool colorMatch =
            (reactsToWhite && flashlightLight.color == Color.white) ||
            (reactsToRed && flashlightLight.color == Color.red) ||
            (reactsToBlue && flashlightLight.color == Color.blue) ||
            (reactsToGreen && flashlightLight.color == Color.green);

        if (colorMatch)
        {
            Vector2 toObject = (transform.position - flashlightTransform.position);
            float distance = toObject.magnitude;

            // --- 2. Проверяем радиус ---
            if (distance <= flashlightLight.pointLightOuterRadius)
            {
                // --- 3. Проверяем угол ---
                float angle = Vector2.Angle(flashlightTransform.up, toObject);

                if (angle <= flashlightLight.pointLightOuterAngle / 2f)
                {
                    shouldReveal = true;
                }
            }
        }

        SetTextVisible(shouldReveal);
    }

    private void SetTextVisible(bool visible)
    {
        targetAlpha = visible ? 1f : 0f;
        revealText.alpha = Mathf.Lerp(revealText.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        revealText.fontMaterial.SetFloat(
            ShaderUtilities.ID_GlowPower,
            visible ? glowIntensity : 0f
        );
    }
}*/

using UnityEngine;
using TMPro;
using UnityEngine.Rendering.Universal;

public class ColorRevealObject : MonoBehaviour
{
    [Header("References")]
    public Light2D flashlightLight;               // фонарик (Light2D)
    public Transform flashlightTransform;         // позиция и направление фонаря
    public TextMeshProUGUI revealText;            // текст с надписью

    [Header("Допустимые цвета")]
    public bool reactsToWhite = false;
    public bool reactsToRed = false;
    public bool reactsToBlue = false;
    public bool reactsToGreen = false;

    [Header("Эффект текста")]
    public float fadeSpeed = 3f;                  // скорость появления/исчезновения
    public float glowIntensity = 1.5f;            // сила свечения текста

    [Header("Аудио")]
    public AudioClip revealSound;                 // звук при появлении
    private bool wasVisible = false;              // предыдущее состояние

    private float targetAlpha = 0f;

    void Start()
    {
        if (revealText != null)
        {
            revealText.alpha = 0f; // изначально скрыт
            revealText.fontMaterial.EnableKeyword("GLOW_ON");
            revealText.fontMaterial.SetFloat(ShaderUtilities.ID_GlowPower, 0f);
        }
    }

    void Update()
    {
        if (flashlightLight == null || flashlightTransform == null || revealText == null)
            return;

        bool shouldReveal = false;

        // 🔴 Проверка: если фонарь выключен — скрыть текст
        if (!flashlightLight.enabled)
        {
            SetTextVisible(false);
            return;
        }

        // --- 1. Проверка цвета ---
        bool colorMatch =
            (reactsToWhite && flashlightLight.color == Color.white) ||
            (reactsToRed && flashlightLight.color == Color.red) ||
            (reactsToBlue && flashlightLight.color == Color.blue) ||
            (reactsToGreen && flashlightLight.color == Color.green);

        if (colorMatch)
        {
            Vector2 toObject = (transform.position - flashlightTransform.position);
            float distance = toObject.magnitude;

            // --- 2. Проверяем радиус ---
            if (distance <= flashlightLight.pointLightOuterRadius)
            {
                // --- 3. Проверяем угол ---
                float angle = Vector2.Angle(flashlightTransform.up, toObject);

                if (angle <= flashlightLight.pointLightOuterAngle / 2f)
                {
                    shouldReveal = true;
                }
            }
        }

        // --- Проверяем смену состояния (срабатывает только при переходе false -> true)
        if (shouldReveal && !wasVisible)
        {
            if (revealSound != null && SoundManager.Instance != null)
            {
                SoundManager.Instance.sfxSource.PlayOneShot(revealSound);
            }
        }

        wasVisible = shouldReveal;
        SetTextVisible(shouldReveal);
    }

    private void SetTextVisible(bool visible)
    {
        targetAlpha = visible ? 1f : 0f;
        revealText.alpha = Mathf.Lerp(revealText.alpha, targetAlpha, Time.deltaTime * fadeSpeed);

        revealText.fontMaterial.SetFloat(
            ShaderUtilities.ID_GlowPower,
            visible ? glowIntensity : 0f
        );
    }
}

