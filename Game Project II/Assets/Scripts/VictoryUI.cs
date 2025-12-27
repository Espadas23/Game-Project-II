
/*using UnityEngine;
using TMPro;
using System.Collections;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance { get; private set; }

    [Header("Fade экраны")]
    [SerializeField] private CanvasGroup blackFade;
    [SerializeField] private CanvasGroup whiteFade;

    [Header("UI концовки")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI endingText;

    [Header("Тексты")]
    [SerializeField] private string badEndingText = "Вас не успели спасти";
    [SerializeField] private string goodEndingText = "Вас успели спасти";

    [Header("Анимация")]
    [SerializeField] private float fadeDuration = 1f;

    private bool isShown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        HideInstant(blackFade);
        HideInstant(whiteFade);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowEnding(EndingType ending)
    {
        if (isShown) return;
        isShown = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        switch (ending)
        {
            case EndingType.Bad:
                endingText.text = badEndingText;
                StartCoroutine(FadeThenShowUI(blackFade));
                break;

            case EndingType.Good:
                endingText.text = goodEndingText;
                StartCoroutine(FadeThenShowUI(whiteFade));
                break;
        }
    }

    private IEnumerator FadeThenShowUI(CanvasGroup fadeGroup)
    {
        // 1️⃣ Плавно затемняем / засветляем
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // 2️⃣ После fade показываем UI
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    private IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        cg.blocksRaycasts = true;
        cg.interactable = false;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        cg.alpha = to;
    }

    private void HideInstant(CanvasGroup cg)
    {
        if (cg == null) return;

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }
}*/



using UnityEngine;
using TMPro;
using System.Collections;

public class VictoryUI : MonoBehaviour
{
    public static VictoryUI Instance { get; private set; }

    [Header("Fade экраны")]
    [SerializeField] private CanvasGroup blackFade;
    [SerializeField] private CanvasGroup whiteFade;

    [Header("UI концовки")]
    [SerializeField] private GameObject victoryPanel;
    [SerializeField] private TextMeshProUGUI endingText;

    [Header("Тексты")]
    [SerializeField] private string badEndingText = "Вас не успели спасти";
    [SerializeField] private string goodEndingText = "Вас успели спасти";

    [Header("Анимация")]
    [SerializeField] private float fadeDuration = 1f;
    [SerializeField] private float delayBeforeFade = 0.2f;

    private bool isShown;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        HideInstant(blackFade);
        HideInstant(whiteFade);

        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    public void ShowEnding(EndingType ending)
    {
        if (isShown) return;
        isShown = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        switch (ending)
        {
            case EndingType.Bad:
                endingText.text = badEndingText;
                endingText.color = Color.white;          // белый текст на черном фоне
                StartCoroutine(FadeThenShowUI(blackFade));
                break;

            case EndingType.Good:
                endingText.text = goodEndingText;
                endingText.color = Color.black;          // черный текст на белом фоне
                StartCoroutine(FadeThenShowUI(whiteFade));
                break;
        }
    }

    private IEnumerator FadeThenShowUI(CanvasGroup fadeGroup)
    {
        if (fadeGroup == null) yield break;

        // пауза перед началом fade
        yield return new WaitForSecondsRealtime(delayBeforeFade);

        // плавное затемнение / засветление
        yield return Fade(fadeGroup, 0f, 1f, fadeDuration);

        // показываем UI после анимации
        if (victoryPanel != null)
            victoryPanel.SetActive(true);
    }

    private IEnumerator Fade(CanvasGroup cg, float from, float to, float duration)
    {
        if (cg == null) yield break;

        cg.blocksRaycasts = true;
        cg.interactable = false;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime / duration;
            cg.alpha = Mathf.Lerp(from, to, t);
            yield return null;
        }

        cg.alpha = to;
    }

    private void HideInstant(CanvasGroup cg)
    {
        if (cg == null) return;

        cg.alpha = 0f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }
}







