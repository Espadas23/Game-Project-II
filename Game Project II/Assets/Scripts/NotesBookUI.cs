using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class NotesBookUI : MonoBehaviour
{
    [Header("UI элементы")]
    public Transform notesListContainer;
    public GameObject noteButtonPrefab;
    public TextMeshProUGUI noteContentText;
    public ScrollRect noteScroll;

    private void OnEnable()
    {
        RefreshList();
    }

    public void RefreshList()
    {
        foreach (Transform child in notesListContainer)
            Destroy(child.gameObject);

        foreach (var note in NoteManager.Instance.allNotes)
        {
            GameObject btnObj = Instantiate(noteButtonPrefab, notesListContainer);
            Button btn = btnObj.GetComponent<Button>();
            TextMeshProUGUI txt = btnObj.GetComponentInChildren<TextMeshProUGUI>();

            txt.text = note.noteTitle;

            bool unlocked = NoteManager.Instance.IsNoteDiscovered(note.noteID);
            btn.interactable = unlocked;
            txt.alpha = unlocked ? 1f : 0.4f;

            btn.onClick.AddListener(() => ShowNote(note));
        }
    }

    private void ShowNote(NoteData note)
    {
        noteContentText.text = note.noteText;
    }
}