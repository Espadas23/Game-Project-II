using TMPro;
using UnityEngine;

public class NotesCounterUI : MonoBehaviour
{
    [SerializeField] private TMP_Text notesCountText;

    private void Start()
    {
        if (NoteManager.Instance != null)
        {
            UpdateText(NoteManager.Instance.CollectedNotesCount);
            NoteManager.Instance.OnNotesCountChanged += UpdateText;
        }
    }

    private void OnDestroy()
    {
        if (NoteManager.Instance != null)
            NoteManager.Instance.OnNotesCountChanged -= UpdateText;
    }

    private void UpdateText(int count)
    {
        if (notesCountText != null)
            notesCountText.text = count.ToString();
    }
}