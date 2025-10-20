using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SafeZone : MonoBehaviour
{
    [Header("Ссылки на фонарик игрока")]
    public FlashlightHP flashlightHP;           // Скрипт HP фонарика
    public Flashlight flashlightScript;         // Скрипт фонарика
    public Light2D playerLight;                 // Сам источник света у игрока

    [Header("Свет сейв-зоны (лампа)")]
    public Light2D zoneLight;                   // Свет лампы сейв-зоны
    public float flickerSpeed = 1.5f;           // Скорость пульсации
    public float flickerIntensity = 0.15f;      // Амплитуда колебаний интенсивности
    private float baseIntensity;                // Базовая интенсивность лампы

    [Header("Внутренние состояния")]
    private bool playerInside = false;
    private float originalShrinkDuration;
    private float originalNoFlashlightDuration;

    private void Start()
    {
        if (flashlightHP != null)
        {
            originalShrinkDuration = flashlightHP.shrinkDuration;
            originalNoFlashlightDuration = flashlightHP.noFlashlightDuration;
        }

        if (zoneLight != null)
            baseIntensity = zoneLight.intensity;
    }

    private void Update()
    {
        // 🔦 Эффект мягкого пульсирующего света
        if (zoneLight != null)
        {
            float flicker = Mathf.Sin(Time.time * flickerSpeed) * flickerIntensity;
            zoneLight.intensity = baseIntensity + flicker;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && flashlightHP != null)
        {
            playerInside = true;

            // 🔋 Полностью восстанавливаем фонарик через готовую функцию
            flashlightHP.OnCrystalCollected();

            // 🧊 Замораживаем процессы
            flashlightHP.shrinkDuration = Mathf.Infinity;        // Фонарик не тратится
            flashlightHP.noFlashlightDuration = Mathf.Infinity;  // Без света не идёт таймер

            // Убедимся, что фонарь можно включить, если игрок захочет
            if (flashlightScript != null)
            {
                flashlightScript.canTurnOnFlashlight = true;
                flashlightScript.isBurnedOut = false;
            }

            Debug.Log("✅ Player entered Safe Zone");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && flashlightHP != null)
        {
            playerInside = false;

            // 🔁 Возвращаем исходные параметры
            flashlightHP.shrinkDuration = originalShrinkDuration;
            flashlightHP.noFlashlightDuration = originalNoFlashlightDuration;

            Debug.Log("🚪 Player left Safe Zone");
        }
    }
}
