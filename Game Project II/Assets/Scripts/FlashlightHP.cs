using UnityEngine;
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

        Debug.Log("Кристалл собран — радиус фонарика восстановлен!");
    }

    private void Die()
    {
        Debug.Log("Игрок умер! Игра окончена.");
        Time.timeScale = 0f;
        Destroy(gameObject);
    }
}