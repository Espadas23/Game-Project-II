using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // ✅ Новый Input System

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance;

    [Header("UI для Popup (одна на сцене)")]
    public CanvasGroup notePopupCanvas;
    public TextMeshProUGUI noteTitleText;
    public TextMeshProUGUI noteBodyText;
    public ScrollRect noteScroll;
    public float fadeSpeed = 5f;

    [Header("Данные")]
    public List<NoteData> allNotes = new List<NoteData>();
    private HashSet<int> discoveredNotes = new HashSet<int>();

    private bool isNoteOpen = false;
    private NoteData currentNote;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        HideNotePopupInstant();
    }

    private void Update()
    {
        // ✅ Новый способ проверки нажатия клавиши E
        if (Keyboard.current != null && isNoteOpen && Keyboard.current.eKey.wasPressedThisFrame)
        {
            CloseNotePopup();
        }
    }

    // --- Показать Popup ---
    public void ShowNotePopup(NoteData data)
    {
        if (isNoteOpen) return;

        currentNote = data;
        isNoteOpen = true;
        discoveredNotes.Add(data.noteID);
        PlayerPrefs.SetInt("Note_" + data.noteID, 1);

        Time.timeScale = 0f;

        noteTitleText.text = data.noteTitle;
        noteBodyText.text = data.noteText;

        StartCoroutine(FadeCanvas(notePopupCanvas, 0f, 1f));
    }

    // --- Закрыть Popup ---
    public void CloseNotePopup()
    {
        if (!isNoteOpen) return;

        isNoteOpen = false;
        Time.timeScale = 1f;
        StartCoroutine(FadeCanvas(notePopupCanvas, 1f, 0f));
    }

    private void HideNotePopupInstant()
    {
        notePopupCanvas.alpha = 0f;
        notePopupCanvas.blocksRaycasts = false;
        notePopupCanvas.interactable = false;
    }

    private System.Collections.IEnumerator FadeCanvas(CanvasGroup cg, float from, float to)
    {
        float t = 0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        if (to == 0f)
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    // --- Проверка наличия записки ---
    public bool IsNoteDiscovered(int id)
    {
        if (PlayerPrefs.GetInt("Note_" + id, 0) == 1) return true;
        return discoveredNotes.Contains(id);
    }

    public HashSet<int> GetDiscoveredNotes() => discoveredNotes;
}
