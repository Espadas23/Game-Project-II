using UnityEngine;
using UnityEngine.UI;

public class FogTile : MonoBehaviour
{
    public RectTransform fogTileRect;
    public float fadeSpeed = 2f;        // Скорость исчезновения квадратика

    [HideInInspector]
    public bool revealed = false;

    private CanvasGroup canvasGroup;
    private bool fading = false;

    void Awake()
    {
        // Добавляем CanvasGroup, если нет
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 1f; // Изначально полностью видимый
    }

    void Update()
    {
        if (fading && canvasGroup.alpha > 0f)
        {
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;
            if (canvasGroup.alpha <= 0f)
            {
                canvasGroup.alpha = 0f;
                revealed = true;
                fading = false;
                // Можно отключить объект для оптимизации
                // gameObject.SetActive(false);
            }
        }
    }

    // Запуск плавного исчезновения
    public void Reveal()
    {
        if (revealed || fading) return;
        fading = true;
    }
}



/*using UnityEngine;
using UnityEngine.UI;

public class FogTile : MonoBehaviour
{
    public RectTransform fogTileRect;
    public float fadeSpeed = 2f; // скорость плавного изменения

    private CanvasGroup canvasGroup;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        canvasGroup.alpha = 1f; // изначально полностью закрыт
    }

    public void UpdateAlpha(float targetAlpha)
    {
        // Плавно изменяем alpha
        canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
    }
}*/
