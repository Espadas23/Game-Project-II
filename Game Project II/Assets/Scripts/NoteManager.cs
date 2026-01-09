
/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }

    [Header("UI для Popup (одна на сцене)")]
    [SerializeField] private CanvasGroup notePopupCanvas;
    [SerializeField] private TextMeshProUGUI noteTitleText;
    [SerializeField] private TextMeshProUGUI noteBodyText;
    [SerializeField] private ScrollRect noteScroll;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Данные")]
    [SerializeField] private List<NoteData> allNotes = new List<NoteData>();
    public List<NoteData> AllNotes => allNotes;

    private HashSet<int> discoveredNotes = new HashSet<int>();
    private bool isNoteOpen = false;
    private NoteData currentNote;

    private PlayerControls controls;

    [Header("Счётчик собранных записок")]
    public int CollectedNotesCount { get; private set; } = 0;
    public event System.Action<int> OnNotesCountChanged;

    private void Awake()
    {
        Instance = this;

        if (controls == null)
            controls = new PlayerControls();

        HideNotePopupInstant();
        discoveredNotes.Clear();
        CollectedNotesCount = 0;
    }

    private void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();

        controls.Player.Interact.performed += OnInteract;
        controls.Enable();
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (controls != null)
        {
            controls.Player.Interact.performed -= OnInteract;
            controls.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
            CloseNotePopup();
    }

    public void ShowNotePopup(NoteData data)
    {
        if (isNoteOpen || data == null || notePopupCanvas == null)
            return;

        currentNote = data;
        isNoteOpen = true;

        if (!discoveredNotes.Contains(data.NoteID))
        {
            discoveredNotes.Add(data.NoteID);
            CollectedNotesCount++;
            OnNotesCountChanged?.Invoke(CollectedNotesCount);
        }

        Time.timeScale = 0f;

        noteTitleText.text = data.NoteTitle;
        noteBodyText.text = data.NoteText;

        StartCoroutine(FadeCanvas(notePopupCanvas, 0f, 1f));
    }

    public void CloseNotePopup()
    {
        if (!isNoteOpen || notePopupCanvas == null)
            return;

        isNoteOpen = false;
        Time.timeScale = 1f;

        StartCoroutine(FadeCanvas(notePopupCanvas, 1f, 0f));
    }

    private void HideNotePopupInstant()
    {
        if (notePopupCanvas == null)
            return;

        notePopupCanvas.alpha = 0f;
        notePopupCanvas.blocksRaycasts = false;
        notePopupCanvas.interactable = false;
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float from, float to)
    {
        if (cg == null)
            yield break;

        float t = 0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        while (t < 1f)
        {
            if (cg == null)
                yield break;

            t += Time.unscaledDeltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        if (cg != null && to == 0f)
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    public void ResetProgress()
    {
        discoveredNotes.Clear();
        CollectedNotesCount = 0;
        OnNotesCountChanged?.Invoke(CollectedNotesCount);

        isNoteOpen = false;
        currentNote = null;

        StopAllCoroutines();

        Time.timeScale = 1f;

        HideNotePopupInstant();

        Debug.Log("🗑️ NoteManager: прогресс записок сброшен");
    }

    public bool IsNoteDiscovered(int id)
    {
        return discoveredNotes.Contains(id);
    }

    public HashSet<int> GetDiscoveredNotes() => discoveredNotes;
}*/



using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class NoteManager : MonoBehaviour
{
    public static NoteManager Instance { get; private set; }

    [Header("UI для Popup (одна на сцене)")]
    [SerializeField] private CanvasGroup notePopupCanvas;
    [SerializeField] private TextMeshProUGUI noteTitleText;
    [SerializeField] private TextMeshProUGUI noteBodyText;
    [SerializeField] private ScrollRect noteScroll;
    [SerializeField] private float fadeSpeed = 5f;

    [Header("Данные")]
    [SerializeField] private List<NoteData> allNotes = new List<NoteData>();
    public List<NoteData> AllNotes => allNotes;

    private HashSet<int> discoveredNotes = new HashSet<int>();
    private bool isNoteOpen = false;
    private NoteData currentNote;

    private PlayerControls controls;

    [Header("Счётчик собранных записок")]
    public int CollectedNotesCount { get; private set; } = 0;
    public event System.Action<int> OnNotesCountChanged;

    // ✅ ФИКС: запоминаем кадр, в который записка была открыта,
    // чтобы то же нажатие Interact не могло сразу её закрыть.
    private int lastOpenFrame = -1;

    private void Awake()
    {
        Instance = this;

        if (controls == null)
            controls = new PlayerControls();

        HideNotePopupInstant();
        discoveredNotes.Clear();
        CollectedNotesCount = 0;
    }

    private void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();

        controls.Player.Interact.performed += OnInteract;
        controls.Enable();
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        if (controls != null)
        {
            controls.Player.Interact.performed -= OnInteract;
            controls.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        // ✅ ФИКС: не даём тому же нажатию, которое открыло записку, сразу закрыть её
        if (isNoteOpen && Time.frameCount != lastOpenFrame)
            CloseNotePopup();
    }

    public void ShowNotePopup(NoteData data)
    {
        if (isNoteOpen || data == null || notePopupCanvas == null)
            return;

        currentNote = data;
        isNoteOpen = true;

        // ✅ ФИКС: запоминаем текущий кадр открытия
        lastOpenFrame = Time.frameCount;

        if (!discoveredNotes.Contains(data.NoteID))
        {
            discoveredNotes.Add(data.NoteID);
            CollectedNotesCount++;
            OnNotesCountChanged?.Invoke(CollectedNotesCount);
        }

        Time.timeScale = 0f;

        noteTitleText.text = data.NoteTitle;
        noteBodyText.text = data.NoteText;

        StartCoroutine(FadeCanvas(notePopupCanvas, 0f, 1f));
    }

    public void CloseNotePopup()
    {
        if (!isNoteOpen || notePopupCanvas == null)
            return;

        isNoteOpen = false;
        Time.timeScale = 1f;

        StartCoroutine(FadeCanvas(notePopupCanvas, 1f, 0f));
    }

    private void HideNotePopupInstant()
    {
        if (notePopupCanvas == null)
            return;

        notePopupCanvas.alpha = 0f;
        notePopupCanvas.blocksRaycasts = false;
        notePopupCanvas.interactable = false;
    }

    private IEnumerator FadeCanvas(CanvasGroup cg, float from, float to)
    {
        if (cg == null)
            yield break;

        float t = 0f;
        cg.blocksRaycasts = true;
        cg.interactable = true;

        while (t < 1f)
        {
            if (cg == null)
                yield break;

            t += Time.unscaledDeltaTime * fadeSpeed;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        if (cg != null && to == 0f)
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }
    }

    public void ResetProgress()
    {
        discoveredNotes.Clear();
        CollectedNotesCount = 0;
        OnNotesCountChanged?.Invoke(CollectedNotesCount);

        isNoteOpen = false;
        currentNote = null;

        StopAllCoroutines();

        Time.timeScale = 1f;

        HideNotePopupInstant();

        Debug.Log("🗑️ NoteManager: прогресс записок сброшен");
    }

    public bool IsNoteDiscovered(int id)
    {
        return discoveredNotes.Contains(id);
    }

    public HashSet<int> GetDiscoveredNotes() => discoveredNotes;
}















