using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem;

public class Flashlight : MonoBehaviour
{
    [Header("Components")]
    public Light2D flashlight;

    [Header("Settings")]
    public bool isOn = false;              // Можно включать через инспектор
    public Key toggleKey = Key.F;          // Кнопка включения
    public float rotationSpeed = 10f;      // Скорость поворота
    public float intensitySpeed = 5f;      // Скорость изменения яркости
    public float maxIntensity = 1.5f;      // Максимальная яркость света
    public float minIntensity = 0f;        // Минимальная яркость (выключено)

    void Update()
    {
        // Переключение по кнопке
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            isOn = !isOn;
        }

        // Плавное изменение интенсивности
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

        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}