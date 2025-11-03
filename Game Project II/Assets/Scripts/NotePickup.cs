/*using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour
{
    public NoteData noteData; // ссылка на ScriptableObject с текстом
    private bool playerInRange = false;
    private bool noteOpened = false;

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!noteOpened)
            {
                // открываем записку
                NoteManager.Instance.ShowNotePopup(noteData);
                noteOpened = true;

                // деактивируем объект, чтобы нельзя было открыть снова
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}*/




using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour
{
    [SerializeField] private NoteData noteData; // Ссылка на ScriptableObject с данными записки

    private bool playerInRange = false; // Игрок находится в зоне взаимодействия
    private bool noteOpened = false;    // Записка уже была открыта

    private void Update()
    {
        if (playerInRange && Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!noteOpened)
            {
                // Открываем записку через NoteManager
                NoteManager.Instance.ShowNotePopup(noteData);
                noteOpened = true;

                // Деактивируем объект, чтобы нельзя было открыть снова
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}



