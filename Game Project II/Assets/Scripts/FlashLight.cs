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

    [Header("Sprite Orientation")]
    public bool spriteLooksUp = false; // если спрайт смотрит вверх, поставь галочку

    void Update()
    {
        // Вкл/выкл света
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
            isOn = !isOn;

        if (flashlight != null)
        {
            float targetIntensity = isOn ? maxIntensity : minIntensity;
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
        }

        // Поворот к курсору
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Сдвиг угла если спрайт смотрит вверх
        if (spriteLooksUp)
            targetAngle -= 90f;

        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}