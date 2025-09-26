/*using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightHP : MonoBehaviour
{
    [Header("Свет")]
    public Light2D flashlight;
    public float maxRadius = 5f;
    public float minRadius = 1f;
    public float shrinkDuration = 10f;

    [Header("Таймер смерти")]
    public float deathDelay = 15f;

    private float timer = 0f;
    private bool isShrinking = true;
    private bool isDeadCountdown = false;

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light2D>();

        flashlight.pointLightOuterRadius = maxRadius;
    }

    void Update()
    {
        if (isShrinking)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / shrinkDuration);
            flashlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, t);

            if (timer >= shrinkDuration)
            {
                isShrinking = false;
                isDeadCountdown = true;
                timer = 0f;
            }
        }
        else if (isDeadCountdown)
        {
            timer += Time.deltaTime;
            if (timer >= deathDelay)
            {
                Die();
            }
        }
    }

    // Метод, который вызывается **кристаллом** при сборе
    public void OnCrystalCollected()
    {
        flashlight.pointLightOuterRadius = maxRadius; // полностью восстанавливаем радиус
        timer = 0f;
        isShrinking = true;
        isDeadCountdown = false;

        Debug.Log("CrystalCollected");
    }

    private void Die()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}*/

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class FlashlightHP : MonoBehaviour
{
    [Header("Свет")]
    public Light2D flashlight;
    public Flashlight flashlightScript; // ссылка на скрипт фонарика
    public float maxRadius = 5f;
    public float minRadius = 1f;
    public float shrinkDuration = 10f;

    [Header("Таймер смерти")]
    public float deathDelay = 15f;

    private float timer = 0f;
    private bool isShrinking = true;
    private bool isDeadCountdown = false;

    [HideInInspector] public bool hasTurnedOnOnce = false; // проверка, включал ли игрок фонарик хотя бы раз

    void Start()
    {
        if (flashlight == null)
            flashlight = GetComponent<Light2D>();

        if (flashlightScript == null)
            flashlightScript = GetComponent<Flashlight>();

        flashlight.pointLightOuterRadius = maxRadius;
    }

    void Update()
    {
        if (flashlightScript != null && flashlightScript.isOn)
            hasTurnedOnOnce = true; // игрок включил фонарик хотя бы раз

        // --- Таймер сужения идёт только если фонарик включен
        if (flashlightScript != null && flashlightScript.isOn && isShrinking)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / shrinkDuration);
            flashlight.pointLightOuterRadius = Mathf.Lerp(maxRadius, minRadius, t);

            if (timer >= shrinkDuration)
            {
                isShrinking = false;
                isDeadCountdown = true;
                timer = 0f;
            }
        }
        else if (isDeadCountdown)
        {
            timer += Time.deltaTime;
            if (timer >= deathDelay)
            {
                Die();
            }
        }
    }

    // Метод, который вызывается кристаллом при сборе
    public void OnCrystalCollected()
    {
        flashlight.pointLightOuterRadius = maxRadius; // полностью восстанавливаем радиус
        timer = 0f;
        isShrinking = true;
        isDeadCountdown = false;

        Debug.Log("CrystalCollected");
    }

    private void Die()
    {
        Debug.Log("GameOver");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}
