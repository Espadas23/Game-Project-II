
/*using System.Collections.Generic;
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

    // --- Input через auto-generated PlayerControls
    private PlayerControls controls;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        discoveredNotes.Clear();
        HideNotePopupInstant();

        // Создаём PlayerControls
        controls = new PlayerControls();
    }

    //private void OnEnable()
    //{
        // Включаем Input
       // controls.Enable();

        // Подписка на клавишу Interact
       // controls.Player.Interact.performed += OnInteract;
    //}
    
    private void OnEnable()
    {
        if (controls == null)
            controls = new PlayerControls();

        controls.Player.Interact.performed += OnInteract;
        controls.Enable();
    }

    //private void OnDisable()
    //{
        //controls.Player.Interact.performed -= OnInteract;
        //controls.Disable();
    //}
    
    private void OnDisable()
    {
        if (controls != null)
        {
            controls.Player.Interact.performed -= OnInteract;
            controls.Disable();
        }
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (isNoteOpen)
        {
            CloseNotePopup();
        }
    }

    public void ShowNotePopup(NoteData data)
    {
        if (isNoteOpen || data == null) return;

        currentNote = data;
        isNoteOpen = true;

        discoveredNotes.Add(data.NoteID);

        Time.timeScale = 0f;

        noteTitleText.text = data.NoteTitle;
        noteBodyText.text = data.NoteText;

        StartCoroutine(FadeCanvas(notePopupCanvas, 0f, 1f));
    }

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

    private void Awake()
    {
        // Просто создаём новый экземпляр при каждой загрузке сцены
        Instance = this;

        // 🔹 Не используем DontDestroyOnLoad, чтобы при Restart создавался новый менеджер
        // DontDestroyOnLoad(gameObject);

        // Создаём контроллер Input
        if (controls == null)
            controls = new PlayerControls();

        // Сразу скрываем UI записки
        HideNotePopupInstant();

        // Очищаем собранные записки, на всякий случай
        discoveredNotes.Clear();
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

        discoveredNotes.Add(data.NoteID);

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

    // 🔥 ВОТ ГЛАВНОЕ — СБРОС ПРОГРЕССА
    public void ResetProgress()
    {
        discoveredNotes.Clear();
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













