
/*using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour
{
    [Header("Данные записки")]
    [SerializeField] private NoteData noteData; // ScriptableObject с данными записки

    private bool playerInRange = false; // Игрок в зоне взаимодействия
    private bool noteOpened = false;    // Записка уже была открыта

    // Auto-generated класс из твоего InputAction Asset
    private PlayerControls controls;

    private void Awake()
    {
        // Создаём экземпляр PlayerControls
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // Если игрок рядом и записка ещё не открыта
        if (playerInRange && !noteOpened)
        {
            // Просто вызываем ShowNotePopup без if, так как метод void
            NoteManager.Instance.ShowNotePopup(noteData);

            // Ставим флаг, что записка открыта, и деактивируем объект
            noteOpened = true;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}*/



using UnityEngine;
using UnityEngine.InputSystem;

public class NotePickup : MonoBehaviour
{
    [SerializeField] private NoteData noteData;

    private bool playerInRange = false;
    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void Start()
    {
        if (NoteManager.Instance != null && NoteManager.Instance.IsNoteDiscovered(noteData.NoteID))
            gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Interact.performed += OnInteract;
    }

    private void OnDisable()
    {
        controls.Player.Interact.performed -= OnInteract;
        controls.Disable();
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (!playerInRange || NoteManager.Instance == null)
            return;

        // 🔹 Показываем UI через NoteManager
        NoteManager.Instance.ShowNotePopup(noteData);

        // 🔹 Сразу помечаем в NoteManager как собранную
        //     и деактивируем объект
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}










