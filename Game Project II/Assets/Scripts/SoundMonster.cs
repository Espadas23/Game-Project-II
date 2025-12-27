/*using System.Collections;
using UnityEngine;

public class SoundMonster : MonoBehaviour
{
    [Header("Player & Movement")]
    public Transform player;               // Ссылка на игрока
    public float speed = 3f;               // Скорость преследования
    public float detectionRadius = 10f;    // Радиус, на котором монстр начинает преследовать
    public float attackDistance = 0.5f;    // Расстояние для срабатывания скримера

    [Header("Spawn Settings")]
    public float appearDistanceMin = 3f;   // Минимальное расстояние появления рядом с игроком
    public float appearDistanceMax = 5f;   // Максимальное расстояние появления рядом с игроком
    public float respawnTime = 60f;        // Время до следующего появления (в секундах)

    [Header("Audio Settings")]
    public AudioSource audioSource;        // Основной звук
    public float maxVolume = 1f;           // Максимальная громкость
    public float minVolume = 0f;           // Минимальная громкость

    [Header("Screamer")]
    public GameObject screamerPrefab;      // Префаб скримера
    public AudioClip screamerSound;        // Звук скримера
    public float screamerScaleTime = 0.2f; // Время масштабирования скримера
    public float screamerDuration = 1f;    // Время показа скримера

    private bool isActive = false;         // Монстр активен на сцене
    private bool isChasing = false;        // Началось преследование

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f; // Начальная громкость
        }

        // Запускаем корутину спавна
        StartCoroutine(SpawnMonster());
    }

    private void Update()
    {
        if (!isActive || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            isChasing = true;

            // Двигаемся к игроку
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Громкость AudioSource в зависимости от расстояния
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp(maxVolume * (1 - distance / detectionRadius), minVolume, maxVolume);
            }

            // Проверка на скример
            if (distance <= attackDistance)
            {
                StartCoroutine(TriggerScreamer());
            }
        }
        else
        {
            // Игрок ушел за пределы радиуса
            if (isChasing)
            {
                StartCoroutine(DeactivateMonster());
            }
        }
    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);

            if (player == null)
                continue;

            // Случайная позиция рядом с игроком
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(appearDistanceMin, appearDistanceMax);
            transform.position = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            isActive = true;
            isChasing = false;

            if (audioSource != null)
            {
                audioSource.volume = minVolume;
                audioSource.Play();
            }
        }
    }

    private IEnumerator DeactivateMonster()
    {
        isActive = false;
        isChasing = false;

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        yield return null;
    }

    private IEnumerator TriggerScreamer()
    {
        isChasing = false;
        isActive = false;

        // Максимальная громкость
        if (audioSource != null)
        {
            audioSource.volume = maxVolume;
        }

        // Скример появляется по центру камеры
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 screamerPosition = new Vector3(cameraPos.x, cameraPos.y, 0); // Z = 0 для 2D
        GameObject screamer = Instantiate(screamerPrefab, screamerPosition, Quaternion.identity);
        screamer.transform.SetParent(null);
        screamer.transform.localScale = Vector3.zero;

        // Проигрываем звук скримера
        if (screamerSound != null)
        {
            AudioSource.PlayClipAtPoint(screamerSound, Camera.main.transform.position);
        }

        // Быстро масштабируем скример
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one;
        while (elapsed < screamerScaleTime)
        {
            screamer.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / screamerScaleTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        screamer.transform.localScale = targetScale;

        // Ждем секунду
        yield return new WaitForSeconds(screamerDuration);

        Destroy(screamer);

        // Останавливаем звук
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}*/



using System.Collections;
using UnityEngine;

public class SoundMonster : MonoBehaviour
{
    [Header("Player & Movement")]
    public Transform player;               // Ссылка на игрока
    public float speed = 3f;               // Скорость преследования
    public float detectionRadius = 10f;    // Радиус, на котором монстр начинает преследовать
    public float attackDistance = 0.5f;    // Расстояние для срабатывания скримера

    [Header("Spawn Settings")]
    public float appearDistanceMin = 3f;   // Минимальное расстояние появления рядом с игроком
    public float appearDistanceMax = 5f;   // Максимальное расстояние появления рядом с игроком
    public float respawnTime = 60f;        // Время до следующего появления (в секундах)

    [Header("Audio Settings")]
    public AudioSource audioSource;        // Основной звук
    public float maxVolume = 1f;           // Максимальная громкость
    public float minVolume = 0f;           // Минимальная громкость

    [Header("Screamer")]
    public GameObject screamerPrefab;      // Префаб скримера
    public AudioClip screamerSound;        // Звук скримера
    public float screamerScaleTime = 0.2f; // Время масштабирования скримера
    public float screamerDuration = 1f;    // Время показа скримера

    private bool isActive = false;         // Монстр активен на сцене
    private bool isChasing = false;        // Началось преследование

    private void Start()
    {
        if (audioSource != null)
        {
            audioSource.volume = 0f; // Начальная громкость
        }

        // Запускаем корутину спавна
        StartCoroutine(SpawnMonster());
    }

    private void Update()
    {
        if (!isActive || player == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRadius)
        {
            isChasing = true;

            // Двигаемся к игроку
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * speed * Time.deltaTime;

            // Громкость AudioSource в зависимости от расстояния
            if (audioSource != null)
            {
                audioSource.volume = Mathf.Clamp(maxVolume * (1 - distance / detectionRadius), minVolume, maxVolume);
            }

            // Проверка на скример
            if (distance <= attackDistance)
            {
                StartCoroutine(TriggerScreamer());
            }
        }
        else
        {
            // Игрок ушел за пределы радиуса
            if (isChasing)
            {
                StartCoroutine(DeactivateMonster());
            }
        }
    }

    private IEnumerator SpawnMonster()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnTime);

            if (player == null)
                continue;

            // Случайная позиция рядом с игроком
            Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(appearDistanceMin, appearDistanceMax);
            transform.position = player.position + new Vector3(randomOffset.x, randomOffset.y, 0);

            isActive = true;
            isChasing = false;

            if (audioSource != null)
            {
                audioSource.volume = minVolume;
                audioSource.Play();
            }
        }
    }

    private IEnumerator DeactivateMonster()
    {
        isActive = false;
        isChasing = false;

        if (audioSource != null)
        {
            audioSource.Stop();
        }

        yield return null;
    }

    private IEnumerator TriggerScreamer()
    {
        isChasing = false;
        isActive = false;

        // Максимальная громкость
        if (audioSource != null)
        {
            audioSource.volume = maxVolume;
        }

        // Применяем штраф к фонарику, ищем компонент даже на дочерних объектах
        if (player != null)
        {
            FlashlightHP flashlightHP = player.GetComponentInChildren<FlashlightHP>();
            if (flashlightHP != null)
            {
                flashlightHP.ApplyScreamerPenalty(flashlightHP.flashlightPenaltyOnScreamer);
                Debug.Log("Screamer applied! Timer now: " + flashlightHP.flashlightPenaltyOnScreamer);
            }
            else
            {
                Debug.LogWarning("FlashlightHP not found on player or its children!");
            }
        }

        // Скример появляется по центру камеры
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 screamerPosition = new Vector3(cameraPos.x, cameraPos.y, 0); // Z = 0 для 2D
        GameObject screamer = Instantiate(screamerPrefab, screamerPosition, Quaternion.identity);
        screamer.transform.SetParent(null);
        screamer.transform.localScale = Vector3.zero;

        // Проигрываем звук скримера
        if (screamerSound != null)
        {
            AudioSource.PlayClipAtPoint(screamerSound, Camera.main.transform.position);
        }

        // Быстро масштабируем скример
        float elapsed = 0f;
        Vector3 targetScale = Vector3.one;
        while (elapsed < screamerScaleTime)
        {
            screamer.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, elapsed / screamerScaleTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        screamer.transform.localScale = targetScale;

        // Ждем секунду
        yield return new WaitForSeconds(screamerDuration);

        Destroy(screamer);

        // Останавливаем звук
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }
}


