using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightColorCycle : MonoBehaviour
{
    public Light2D flashlightLight;     // 2D свет фонарика
    public Key toggleKey = Key.C;       // клавиша для смены цвета

    private Color[] colors = new Color[]
    {
        Color.white,   // белый
        Color.red,     // красный
        Color.blue,    // синий
        Color.green    // зеленый
    };

    private int currentMode = 0;        // 0 = белый, 1 = красный, 2 = синий, 3 = зеленый, 4 = выключен

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            currentMode = (currentMode + 1) % 5; // 0-4 циклически

            if (currentMode == 4)
            {
                // выключаем фонарик
                flashlightLight.enabled = false;
            }
            else
            {
                // включаем фонарик и устанавливаем цвет
                flashlightLight.enabled = true;
                flashlightLight.color = colors[currentMode];
            }
        }
    }
}