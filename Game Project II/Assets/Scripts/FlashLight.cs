using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.InputSystem; // новый Input System

public class Flashlight : MonoBehaviour
{
    [Header("Components")]
    public Light2D flashlight;

    [Header("Settings")]
    public bool isOn = false; // можно включать через инспектор во время игры
    public Key toggleKey = Key.F; // кнопка включения

    void Update()
    {
        // 1️⃣ Проверяем нажатие клавиши F через Input System
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            isOn = !isOn;
        }

        // 2️⃣ Включаем/выключаем свет
        if (flashlight != null)
            flashlight.enabled = isOn;

        // 3️⃣ Фонарик ровно смотрит на мышку
        Vector3 mousePos = Mouse.current.position.ReadValue(); // позиция курсора в пикселях
        mousePos = Camera.main.ScreenToWorldPoint(mousePos); // в мировые координаты
        mousePos.z = 0f; // фиксируем Z, чтобы фонарик не улетал

        // вычисляем направление
        Vector2 direction = (mousePos - transform.position).normalized;

        // угол поворота
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // поворачиваем фонарик
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}