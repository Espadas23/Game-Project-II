
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Components")]
    public Light2D flashlight;

    [Header("Settings")]
    public bool isOn = false;
    public Key toggleKey = Key.F;
    public float rotationSpeed = 10f;
    public float intensitySpeed = 5f;
    public float maxIntensity = 1.5f;
    public float minIntensity = 0f;

    [Header("Radius Shrink Settings")]
    public float shrinkDelay = 1f;
    public float shrinkSpeed = 0.5f;
    public float minRadius = 1f;
    private float shrinkTimer;

    [Header("Sprite Orientation")]
    public bool spriteLooksUp = false;

    [HideInInspector]
    public bool hasActivatedOnce = false;

    [HideInInspector]
    public bool canTurnOnFlashlight = true;
    [HideInInspector]
    public bool isBurnedOut = false;

    void Start()
    {
        shrinkTimer = shrinkDelay;
        if (flashlight != null)
            flashlight.enabled = isOn;
    }

    void Update()
    {
        // --- Вкл/выкл света через F ---
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            if (isOn || (!isBurnedOut && canTurnOnFlashlight))
            {
                isOn = !isOn;
                if (flashlight != null)
                    flashlight.enabled = isOn;

                if (isOn) hasActivatedOnce = true;

                if (SoundManager.Instance != null)
                    SoundManager.Instance.PlayFlashlightToggle();
            }
        }

        // --- Интенсивность света ---
        if (flashlight != null)
        {
            float targetIntensity = isOn ? maxIntensity : minIntensity;
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
        }

        // --- Поворот к курсору ---
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (spriteLooksUp)
            targetAngle -= 90f;

        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // --- Таймер сужения радиуса ---
        if (isOn && flashlight != null && !isBurnedOut)
        {
            shrinkTimer -= Time.deltaTime;
            if (shrinkTimer <= 0f)
            {
                flashlight.pointLightOuterRadius = Mathf.Max(minRadius, flashlight.pointLightOuterRadius - shrinkSpeed * Time.deltaTime);

                if (flashlight.pointLightOuterRadius <= minRadius + 0.01f)
                {
                    BurnOut();
                }
            }
        }
    }

    private void BurnOut()
    {
        isOn = false;
        if (flashlight != null)
            flashlight.enabled = false;

        canTurnOnFlashlight = false;
        isBurnedOut = true;
    }

    public void RestoreRadius(float fullRadius)
    {
        if (flashlight != null)
        {
            flashlight.pointLightOuterRadius = fullRadius;
            shrinkTimer = shrinkDelay;

            isBurnedOut = false;
            canTurnOnFlashlight = true;

            isOn = true;
            flashlight.enabled = true;
            hasActivatedOnce = true;
        }
    }
}












































