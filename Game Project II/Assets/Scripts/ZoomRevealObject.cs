using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Rendering.Universal;

public class ZoomRevealObject : MonoBehaviour
{
    [Header("Свет и зум")]
    public FlashlightZoom flashlightZoom;   // скрипт зума + фонарь
    public Light2D flashlightLight;        // сам фонарь Light2D
    public float revealRadius = 1f;        // радиус, на котором цифра видна

    [Header("Цифра")]
    public CanvasGroup numberCanvasGroup;   // CanvasGroup цифры для управления прозрачностью

    [Header("Видимость")]
    public bool visibleWithLightOnly = false;      // видна только при свете фонаря
    public bool visibleWithZoom = false;           // видна при свете + обычный зум (ПКМ)
    public bool visibleWithExtraZoom = false;      // видна при свете + ПКМ + прокрутка колесика (экстра-зум)

    [Header("Плавность")]
    public float fadeSpeed = 5f;  // скорость появления/исчезновения

    private Transform player;

    void Start()
    {
        if (flashlightZoom != null)
            player = flashlightZoom.player;

        if (numberCanvasGroup != null)
            numberCanvasGroup.alpha = 0f; // полностью прозрачна изначально
    }

    void Update()
    {
        if (flashlightZoom == null || flashlightLight == null || numberCanvasGroup == null || player == null)
            return;

        // --- Проверка расстояния до игрока / фонаря ---
        float distance = Vector3.Distance(transform.position, player.position);
        bool withinRadius = distance <= flashlightLight.pointLightOuterRadius;

        // Проверка включения фонаря
        bool hasLight = flashlightZoom.flashlight.isOn && flashlightLight.enabled;

        // --- Проверка попадания в конус света ---
        bool inLightCone = true;
        Vector3 dirToObject = transform.position - flashlightLight.transform.position;
        if (dirToObject.sqrMagnitude > 0.0001f)
        {
            Vector3 flashlightDir = flashlightLight.transform.up; // направление света (предполагаем Up)
            float angle = Vector3.Angle(flashlightDir, dirToObject.normalized);
            inLightCone = angle <= flashlightLight.pointLightOuterAngle / 2f;
        }

        bool canSee = false;

        // --- Проверка видимости по флагам ---
        if (visibleWithExtraZoom && hasLight && flashlightZoom.isZoomed && flashlightZoom.isExtraZoomed && withinRadius && inLightCone)
        {
            canSee = true;
        }
        else if (visibleWithZoom && hasLight && flashlightZoom.isZoomed && !flashlightZoom.isExtraZoomed && withinRadius && inLightCone)
        {
            canSee = true;
        }
        else if (visibleWithLightOnly && hasLight && withinRadius && inLightCone)
        {
            canSee = true;
        }

        // --- Плавное обновление прозрачности цифры ---
        float targetAlpha = canSee ? 1f : 0f;
        numberCanvasGroup.alpha = Mathf.Lerp(numberCanvasGroup.alpha, targetAlpha, Time.deltaTime * fadeSpeed);
    }
}










