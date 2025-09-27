/*using UnityEngine;
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
        // --- Вкл/выкл света
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            isOn = !isOn;

            // 🎵 звук переключения фонарика
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayFlashlightToggle();
        }

        if (flashlight != null)
        {
            float targetIntensity = isOn ? maxIntensity : minIntensity;
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
        }

        // --- Поворот к курсору
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
}*/

/*using UnityEngine;
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
    public float shrinkDelay = 1f;       // через сколько секунд после включения фонарика начинается сжатие
    public float shrinkSpeed = 0.5f;     // скорость сужения
    public float minRadius = 1f;         // минимальный радиус
    private float shrinkTimer;

    [Header("Sprite Orientation")]
    public bool spriteLooksUp = false; // если спрайт смотрит вверх, поставь галочку

    [HideInInspector]
    public bool hasActivatedOnce = false; // включал ли игрок фонарик хотя бы один раз

    void Start()
    {
        shrinkTimer = shrinkDelay;
    }

    void Update()
    {
        // --- Вкл/выкл света
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            isOn = !isOn;

            // отмечаем, что игрок включил фонарик хотя бы один раз
            if (isOn) hasActivatedOnce = true;

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayFlashlightToggle();
        }

        // --- Интенсивность света
        if (flashlight != null)
        {
            float targetIntensity = isOn ? maxIntensity : minIntensity;
            flashlight.intensity = Mathf.Lerp(flashlight.intensity, targetIntensity, intensitySpeed * Time.deltaTime);
        }

        // --- Поворот к курсору
        Vector3 mousePos = Mouse.current.position.ReadValue();
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);
        mousePos.z = 0f;

        Vector2 direction = (mousePos - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (spriteLooksUp)
            targetAngle -= 90f;

        float angle = Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, rotationSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, angle);

        // --- Таймер сужения радиуса
        if (isOn && flashlight != null)
        {
            shrinkTimer -= Time.deltaTime;
            if (shrinkTimer <= 0f)
            {
                flashlight.pointLightOuterRadius = Mathf.Max(minRadius, flashlight.pointLightOuterRadius - shrinkSpeed * Time.deltaTime);
            }
        }
    }

    // Вызвать этот метод при сборе кристалла
    public void RestoreRadius(float fullRadius)
    {
        if (flashlight != null)
        {
            flashlight.pointLightOuterRadius = fullRadius;
            shrinkTimer = shrinkDelay; // сброс таймера
        }
    }
}*/

/*using UnityEngine;
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
    public bool canTurnOnFlashlight = true; // Можно ли включать фонарь кнопкой F
    [HideInInspector]
    public bool isBurnedOut = false; // Фонарик погас по таймеру

    void Start()
    {
        shrinkTimer = shrinkDelay;
    }

    void Update()
    {
        // --- Вкл/выкл света через F, учитывая блокировку ---
        if (Keyboard.current[toggleKey].wasPressedThisFrame && canTurnOnFlashlight && !isBurnedOut)
        {
            isOn = !isOn;

            if (isOn) hasActivatedOnce = true;

            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayFlashlightToggle();
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

                // Если радиус достиг минимального значения → фонарь "выгорел"
                if (flashlight.pointLightOuterRadius <= minRadius + 0.01f)
                {
                    BurnOut();
                }
            }
        }
    }

    // Вызвать этот метод при сборе кристалла
    public void RestoreRadius(float fullRadius)
    {
        if (flashlight != null)
        {
            flashlight.pointLightOuterRadius = fullRadius;
            shrinkTimer = shrinkDelay;
            isOn = true; // фонарь включается автоматически
            canTurnOnFlashlight = true; // кнопка F снова работает
            isBurnedOut = false; // сбрасываем состояние выгорания
        }
    }

    // Метод вызывается когда фонарь погас по таймеру
    private void BurnOut()
    {
        isOn = false;
        canTurnOnFlashlight = false; // блокируем кнопку включения
        isBurnedOut = true;          // фонарь погас, нельзя влиять на таймер смерти
    }
}*/


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
    public bool canTurnOnFlashlight = true; // Можно ли включать фонарь кнопкой F
    [HideInInspector]
    public bool isBurnedOut = false;        // Фонарик погас по таймеру

    void Start()
    {
        shrinkTimer = shrinkDelay;
    }

    void Update()
    {
        // --- Вкл/выкл света через F ---
        if (Keyboard.current[toggleKey].wasPressedThisFrame)
        {
            // Разрешаем выключать всегда, включать только если фонарь не выгорел
            if (isOn || (!isBurnedOut && canTurnOnFlashlight)) // <-- ключевая строка
            {
                isOn = !isOn;

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

                // Если радиус достиг минимального значения → фонарь "выгорел"
                if (flashlight.pointLightOuterRadius <= minRadius + 0.01f)
                {
                    BurnOut();
                }
            }
        }
    }

    // Вызвать этот метод при сборе кристалла
    public void RestoreRadius(float fullRadius)
    {
        if (flashlight != null)
        {
            flashlight.pointLightOuterRadius = fullRadius;
            shrinkTimer = shrinkDelay;

            isBurnedOut = false;        // сброс состояния выгорания
            canTurnOnFlashlight = true; // кнопка F снова работает

            isOn = true;                // включаем фонарь автоматически
            hasActivatedOnce = true;
        }
    }

    // Метод вызывается когда фонарь погас по таймеру
    private void BurnOut()
    {
        isOn = false;
        canTurnOnFlashlight = false; // блокируем кнопку включения
        isBurnedOut = true;          // фонарь погас, нельзя влиять на таймер смерти
    }
}
























