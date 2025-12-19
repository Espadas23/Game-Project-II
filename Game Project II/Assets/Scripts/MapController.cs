using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class MapController : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup mapGroup;      // CanvasGroup карты
    public RawImage mapImage;         // Изображение карты
    public RectTransform playerMarker;

    [Header("References")]
    public Transform player;
    public float mapFadeSpeed = 5f;
    public float markerMoveSpeed = 10f;

    [Header("Fog Settings")]
    public int fogTextureSize = 512;
    public float revealRadius = 5f;
    public string shaderFogTexProp = "_FogTex";
    public string shaderAlphaProp = "_Alpha";

    private bool mapVisible = false;
    private float targetAlpha = 0f;
    private Material mapMaterial;
    private Texture2D fogTexture;
    private Color32[] fogPixels;
    private float worldToTex;

    void Awake()
    {
        // гарантируем наличие CanvasGroup
        if (mapGroup == null)
            mapGroup = GetComponent<CanvasGroup>();

        // карта невидима и неактивна для кликов
        mapGroup.alpha = 0f;
        mapGroup.interactable = false;
        mapGroup.blocksRaycasts = false;

        if (playerMarker != null)
            playerMarker.gameObject.SetActive(false);
    }

    void Start()
    {
        if (mapImage != null)
        {
            mapMaterial = mapImage.material;

            fogTexture = new Texture2D(fogTextureSize, fogTextureSize, TextureFormat.RGBA32, false);
            fogTexture.wrapMode = TextureWrapMode.Clamp;

            fogPixels = new Color32[fogTextureSize * fogTextureSize];
            for (int i = 0; i < fogPixels.Length; i++)
                fogPixels[i] = Color.white;

            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply();

            mapMaterial.SetTexture(shaderFogTexProp, fogTexture);
            SetMapAlpha(0f);

            worldToTex = fogTextureSize / (mapImage.rectTransform.rect.width);
        }
    }

    void Update()
    {
        HandleMapToggle();
        UpdateMapAlpha();
        UpdatePlayerMarker();
        UpdateFogTexture();
    }

    private void HandleMapToggle()
    {
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            mapVisible = !mapVisible;
            targetAlpha = mapVisible ? 1f : 0f;

            mapGroup.interactable = mapVisible;
            mapGroup.blocksRaycasts = mapVisible;

            if (playerMarker != null)
                playerMarker.gameObject.SetActive(mapVisible);
        }
    }

    private void UpdateMapAlpha()
    {
        if (mapGroup != null)
            mapGroup.alpha = Mathf.MoveTowards(mapGroup.alpha, targetAlpha, Time.deltaTime * mapFadeSpeed);

        if (mapMaterial != null)
        {
            float currentAlpha = mapMaterial.GetFloat(shaderAlphaProp);
            float newAlpha = Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * mapFadeSpeed);
            mapMaterial.SetFloat(shaderAlphaProp, newAlpha);
        }
    }

    private void UpdatePlayerMarker()
    {
        if (playerMarker != null && player != null && mapVisible)
        {
            Vector3 worldPos = player.position;
            Vector3 markerPos = new Vector3(worldPos.x, worldPos.y, 0f);
            playerMarker.localPosition = Vector3.Lerp(playerMarker.localPosition, markerPos, Time.deltaTime * markerMoveSpeed);
        }
    }

    private void UpdateFogTexture()
    {
        if (fogTexture == null || player == null || !mapVisible) return;

        int px = Mathf.RoundToInt(player.position.x * worldToTex + fogTextureSize / 2f);
        int py = Mathf.RoundToInt(player.position.y * worldToTex + fogTextureSize / 2f);
        int radius = Mathf.RoundToInt(revealRadius * worldToTex);

        for (int y = -radius; y <= radius; y++)
        {
            int ty = py + y;
            if (ty < 0 || ty >= fogTextureSize) continue;

            for (int x = -radius; x <= radius; x++)
            {
                int tx = px + x;
                if (tx < 0 || tx >= fogTextureSize) continue;

                if (x * x + y * y <= radius * radius)
                {
                    int index = ty * fogTextureSize + tx;
                    fogPixels[index] = Color.clear;
                }
            }
        }

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }

    private void SetMapAlpha(float alpha)
    {
        if (mapMaterial != null)
            mapMaterial.SetFloat(shaderAlphaProp, alpha);
    }
}


/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CanvasGroup))]
public class MapController : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup mapGroup;      // CanvasGroup карты (контролирует прозрачность всей карты)
    public RawImage mapImage;         // Изображение карты с Fog
    public RectTransform playerMarker;

    [Header("References")]
    public Transform player;
    public float mapFadeSpeed = 5f;
    public float markerMoveSpeed = 10f;

    [Header("Fog Settings")]
    public int fogTextureSize = 512;
    public float revealRadius = 5f;

    private bool mapVisible = false;
    private float targetAlpha = 0f;
    private Texture2D fogTexture;
    private Color32[] fogPixels;
    private float worldToTex;

    void Awake()
    {
        if (mapGroup == null)
            mapGroup = GetComponent<CanvasGroup>();

        // изначально карта полностью скрыта
        mapGroup.alpha = 0f;
        mapGroup.interactable = false;
        mapGroup.blocksRaycasts = false;

        // RawImage тоже прозрачный
        if (mapImage != null)
        {
            Color c = mapImage.color;
            c.a = 0f;
            mapImage.color = c;
            mapImage.gameObject.SetActive(false); // полностью скрываем до открытия
        }

        if (playerMarker != null)
            playerMarker.gameObject.SetActive(false);
    }

    void Start()
    {
        if (mapImage != null)
        {
            // создаём текстуру тумана
            fogTexture = new Texture2D(fogTextureSize, fogTextureSize, TextureFormat.RGBA32, false);
            fogTexture.wrapMode = TextureWrapMode.Clamp;

            fogPixels = new Color32[fogTextureSize * fogTextureSize];
            for (int i = 0; i < fogPixels.Length; i++)
                fogPixels[i] = Color.white;

            fogTexture.SetPixels32(fogPixels);
            fogTexture.Apply();

            mapImage.texture = fogTexture;

            worldToTex = fogTextureSize / (mapImage.rectTransform.rect.width);
        }
    }

    void Update()
    {
        HandleMapToggle();
        UpdateMapAlpha();
        UpdatePlayerMarker();
        UpdateFogTexture();
    }

    private void HandleMapToggle()
    {
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            mapVisible = !mapVisible;
            targetAlpha = mapVisible ? 1f : 0f;

            mapGroup.interactable = mapVisible;
            mapGroup.blocksRaycasts = mapVisible;

            // включаем RawImage только при открытии карты
            if (mapImage != null)
                mapImage.gameObject.SetActive(mapVisible);

            if (playerMarker != null)
                playerMarker.gameObject.SetActive(mapVisible);
        }
    }

    private void UpdateMapAlpha()
    {
        if (mapGroup != null)
            mapGroup.alpha = Mathf.MoveTowards(mapGroup.alpha, targetAlpha, Time.deltaTime * mapFadeSpeed);

        if (mapImage != null)
        {
            Color c = mapImage.color;
            c.a = Mathf.MoveTowards(c.a, targetAlpha, Time.deltaTime * mapFadeSpeed);
            mapImage.color = c;
        }
    }

    private void UpdatePlayerMarker()
    {
        if (playerMarker != null && player != null && mapVisible)
        {
            Vector3 worldPos = player.position;
            Vector3 markerPos = new Vector3(worldPos.x, worldPos.y, 0f);
            playerMarker.localPosition = Vector3.Lerp(playerMarker.localPosition, markerPos, Time.deltaTime * markerMoveSpeed);
        }
    }

    private void UpdateFogTexture()
    {
        if (fogTexture == null || player == null || !mapVisible) return;

        int px = Mathf.RoundToInt(player.position.x * worldToTex + fogTextureSize / 2f);
        int py = Mathf.RoundToInt(player.position.y * worldToTex + fogTextureSize / 2f);
        int radius = Mathf.RoundToInt(revealRadius * worldToTex);

        for (int y = -radius; y <= radius; y++)
        {
            int ty = py + y;
            if (ty < 0 || ty >= fogTextureSize) continue;

            for (int x = -radius; x <= radius; x++)
            {
                int tx = px + x;
                if (tx < 0 || tx >= fogTextureSize) continue;

                if (x * x + y * y <= radius * radius)
                {
                    int index = ty * fogTextureSize + tx;
                    fogPixels[index] = Color.clear;
                }
            }
        }

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }
}*/











