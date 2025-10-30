using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Portal : MonoBehaviour
{
    [Header("Связанный портал")]
    public Transform linkedPortal;

    [Header("Настройки взаимодействия")]
    public float teleportDelay = 0.3f; // небольшая задержка
    public float cooldown = 1f;        // время, пока портал нельзя активировать снова

    [Header("Эффекты (опционально)")]
    public AudioClip teleportSound;

    private bool playerInRange = false;
    private bool isOnCooldown = false;
    private Transform playerTransform;
    private AudioSource audioSource;

    private void Start()
    {
        if (teleportSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.playOnAwake = false;
        }
    }

    private void Update()
    {
        // Проверяем нажатие E через новую Input System (аналог твоего NotePickup)
        if (playerInRange && !isOnCooldown && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            StartCoroutine(TeleportPlayer());
        }
    }

    private IEnumerator TeleportPlayer()
    {
        if (linkedPortal == null || playerTransform == null) yield break;

        isOnCooldown = true;

        // Проигрываем звук (если есть)
        if (audioSource != null && teleportSound != null)
            audioSource.PlayOneShot(teleportSound);

        // Небольшая задержка
        yield return new WaitForSeconds(teleportDelay);

        // Перемещаем игрока
        playerTransform.position = linkedPortal.position;

        // Ждём перед повторной активацией
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            playerTransform = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            playerTransform = null;
        }
    }
}
