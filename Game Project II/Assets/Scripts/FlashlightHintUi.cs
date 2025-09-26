/*using UnityEngine;

public class FlashlightHintUI : MonoBehaviour
{
    public FlashlightHP flashlightHP; // перетащить сюда объект с FlashlightHP
    public GameObject hintPanel;      // перетащить сюда UI-панель или текст
    public float fadeDuration = 1f;   // время исчезания

    private CanvasGroup canvasGroup;
    private bool isFading = false;

    void Start()
    {
        if (hintPanel != null)
        {
            canvasGroup = hintPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = hintPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 1f;
            hintPanel.SetActive(true);
        }
    }

    void Update()
    {
        if (flashlightHP != null && flashlightHP.hasTurnedOnOnce && !isFading)
        {
            StartCoroutine(FadeOutHint());
        }
    }

    private System.Collections.IEnumerator FadeOutHint()
    {
        isFading = true;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        hintPanel.SetActive(false);
    }
}*/

using UnityEngine;

public class FlashlightHintUI : MonoBehaviour
{
    public FlashlightHP flashlightHP; // перетащить сюда объект с FlashlightHP
    public GameObject hintPanel;      // перетащить сюда UI-панель или текст
    public float fadeDuration = 1f;   // время исчезания

    private CanvasGroup canvasGroup;
    private bool isFading = false;
    private bool hasGameStarted = false; // флаг начала игры

    void Start()
    {
        if (hintPanel != null)
        {
            canvasGroup = hintPanel.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
                canvasGroup = hintPanel.AddComponent<CanvasGroup>();

            canvasGroup.alpha = 0f;
            hintPanel.SetActive(false); // скрываем подсказку до старта игры
        }
    }

    void Update()
    {
        if (!hasGameStarted) return;

        if (flashlightHP != null && flashlightHP.hasTurnedOnOnce && !isFading)
        {
            StartCoroutine(FadeOutHint());
        }
    }

    // Вызывать этот метод из UIMenu после нажатия Start
    public void ShowHint()
    {
        if (hintPanel != null)
        {
            hintPanel.SetActive(true);
            canvasGroup.alpha = 1f;
            hasGameStarted = true;
        }
    }

    private System.Collections.IEnumerator FadeOutHint()
    {
        isFading = true;
        float startAlpha = canvasGroup.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, time / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
        hintPanel.SetActive(false);
    }
}
