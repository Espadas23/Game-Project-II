using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightColorCycle : MonoBehaviour
{
    [Header("References")]
    public Light2D flashlightLight;   // фонарик
    public Key colorCycleKey = Key.C; // кнопка смены цвета

    private Color[] colors = new Color[]
    {
        Color.white,
        Color.red,
        Color.blue,
        Color.green
    };

    private int currentMode = 0;

    void Start()
    {
        if (flashlightLight != null)
            flashlightLight.color = colors[currentMode];
    }

    void Update()
    {
        if (Keyboard.current == null || flashlightLight == null) return;

        // Смена цвета по кругу при нажатии C
        if (Keyboard.current[colorCycleKey].wasPressedThisFrame)
        {
            currentMode = (currentMode + 1) % colors.Length;
            flashlightLight.color = colors[currentMode];
        }
    }
}