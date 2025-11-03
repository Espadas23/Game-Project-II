
/*using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesBookUI : MonoBehaviour
{
    [Header("UI элементы")]
    public Transform notesListContainer;
    public GameObject noteButtonPrefab;
    public TextMeshProUGUI noteContentText;

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        // Удаляем старые кнопки
        foreach (Transform child in notesListContainer)
            Destroy(child.gameObject);

        // Создаем кнопки для всех записок
        foreach (var note in NoteManager.Instance.allNotes)
        {
            GameObject btnObj = Instantiate(noteButtonPrefab, notesListContainer);
            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = note.noteTitle;

            // Проверяем, была ли записка подобрана в текущей сессии
            bool unlocked = NoteManager.Instance.IsNoteDiscovered(note.noteID);
            btn.interactable = unlocked;
            txt.alpha = unlocked ? 1f : 0.4f;

            // Правильное замыкание
            NoteData capturedNote = note;
            btn.onClick.AddListener(() => ShowNote(capturedNote));
        }

        // Очищаем окно справа при открытии книжки
        noteContentText.text = "";
    }

    private void ShowNote(NoteData note)
    {
        if (note == null) return;
        noteContentText.text = note.noteText;
    }
}*/



using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesBookUI : MonoBehaviour
{
    [Header("UI элементы")]
    [SerializeField] private Transform notesListContainer;
    [SerializeField] private GameObject noteButtonPrefab;
    [SerializeField] private TextMeshProUGUI noteContentText;

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        // Удаляем старые кнопки
        foreach (Transform child in notesListContainer)
            Destroy(child.gameObject);

        // Создаем кнопки для всех записок
        foreach (var note in NoteManager.Instance.AllNotes)
        {
            GameObject btnObj = Instantiate(noteButtonPrefab, notesListContainer);
            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            // <-- заменено на свойство
            txt.text = note.NoteTitle;

            // Проверяем, была ли записка подобрана в текущей сессии
            bool unlocked = NoteManager.Instance.IsNoteDiscovered(note.NoteID);
            btn.interactable = unlocked;
            txt.alpha = unlocked ? 1f : 0.4f;

            // Правильное замыкание
            NoteData capturedNote = note;
            btn.onClick.AddListener(() => ShowNote(capturedNote));
        }

        // Очищаем окно справа при открытии книжки
        noteContentText.text = "";
    }

    private void ShowNote(NoteData note)
    {
        if (note == null) return;
        // <-- заменено на свойство
        noteContentText.text = note.NoteText;
    }
}








