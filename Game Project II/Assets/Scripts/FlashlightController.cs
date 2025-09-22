using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class FlashlightController : MonoBehaviour
{
    public Light2D light2D;
    public bool isOn = true;

    void Update()
    {
        if (Keyboard.current == null) return;

        // Включение/выключение по кнопке F
        if (Keyboard.current.fKey.wasPressedThisFrame)
        {
            isOn = !isOn;
            if (light2D != null)
                light2D.enabled = isOn;
        }
    }
}