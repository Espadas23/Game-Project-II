
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;


[RequireComponent(typeof(CanvasGroup))]
public class ShaderGraph1 : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup mapGroup;
    public RawImage mapImage;
    public RectTransform mapRect;

    [Header("World")]
    public Transform player;          // Игрок, вокруг которого раскрывается туман
    public Tilemap tilemap;           // Тайлмап карты для границ

    [Header("Settings")]
    public float mapFadeSpeed = 5f;

    [Header("Fog Settings")]
    public int fogTextureWidth = 739; // Совпадает с шириной карты
    public int fogTextureHeight = 556; // Совпадает с высотой карты
    public float revealRadius = 5f;   // В единицах мира
    public string shaderFogTexProp = "_FogTex";
    public string shaderAlphaProp = "_Alpha";

    private bool mapVisible;
    private float targetAlpha;
    private Material mapMaterial;
    private Texture2D fogTexture;
    private Color32[] fogPixels;

    private Bounds mapBounds;

    void Awake()
    {
        if (mapGroup == null)
            mapGroup = GetComponent<CanvasGroup>();

        mapGroup.alpha = 0f;
        mapGroup.interactable = false;
        mapGroup.blocksRaycasts = false;

        if (player == null)
            Debug.LogWarning("Player Transform is not assigned!");
        if (tilemap == null)
            Debug.LogWarning("Tilemap is not assigned!");
    }

    void Start()
    {
        if (mapImage == null || player == null || tilemap == null) return;

        mapMaterial = mapImage.material;

        fogTexture = new Texture2D(fogTextureWidth, fogTextureHeight, TextureFormat.RGBA32, false);
        fogTexture.wrapMode = TextureWrapMode.Clamp;
        fogTexture.filterMode = FilterMode.Point;

        fogPixels = new Color32[fogTextureWidth * fogTextureHeight];
        ClearFog(); // старт с черного

        mapMaterial.SetTexture(shaderFogTexProp, fogTexture);
        mapMaterial.SetFloat(shaderAlphaProp, 0f);

        mapBounds = tilemap.localBounds; // используем тайлмап для границ
    }

    void Update()
    {
        HandleMapToggle();
        UpdateMapAlpha();
        UpdateFogTexture();
    }

    void HandleMapToggle()
    {
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            mapVisible = !mapVisible;
            targetAlpha = mapVisible ? 1f : 0f;

            mapGroup.interactable = mapVisible;
            mapGroup.blocksRaycasts = mapVisible;

            if (mapVisible)
                ClearFog(); // очищаем при открытии карты
        }
    }

    void UpdateMapAlpha()
    {
        mapGroup.alpha = Mathf.MoveTowards(mapGroup.alpha, targetAlpha, Time.deltaTime * mapFadeSpeed);

        if (mapMaterial != null)
        {
            float currentAlpha = mapMaterial.GetFloat(shaderAlphaProp);
            mapMaterial.SetFloat(shaderAlphaProp,
                Mathf.MoveTowards(currentAlpha, targetAlpha, Time.deltaTime * mapFadeSpeed));
        }
    }

    void UpdateFogTexture()
    {
        if (!mapVisible || fogTexture == null || player == null) return;

        // Преобразуем мировые координаты игрока в координаты текстуры
        float u = Mathf.InverseLerp(mapBounds.min.x, mapBounds.max.x, player.position.x);
        float v = Mathf.InverseLerp(mapBounds.min.y, mapBounds.max.y, player.position.y);

        int px = Mathf.RoundToInt(u * fogTextureWidth);
        int py = Mathf.RoundToInt(v * fogTextureHeight);

        // Рассчитываем radius в пикселях текстуры
        float worldWidth = mapBounds.size.x;
        float worldHeight = mapBounds.size.y;

        int radiusX = Mathf.RoundToInt((revealRadius / worldWidth) * fogTextureWidth);
        int radiusY = Mathf.RoundToInt((revealRadius / worldHeight) * fogTextureHeight);

        for (int y = -radiusY; y <= radiusY; y++)
        {
            int ty = py + y;
            if (ty < 0 || ty >= fogTextureHeight) continue;

            for (int x = -radiusX; x <= radiusX; x++)
            {
                int tx = px + x;
                if (tx < 0 || tx >= fogTextureWidth) continue;

                // Эллиптическая форма (подгонка под прямоугольную карту)
                if ((float)x / radiusX * (float)x / radiusX + (float)y / radiusY * (float)y / radiusY <= 1f)
                    fogPixels[ty * fogTextureWidth + tx] = Color.white;
            }
        }

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }

    void ClearFog()
    {
        for (int i = 0; i < fogPixels.Length; i++)
            fogPixels[i] = Color.black;

        fogTexture.SetPixels32(fogPixels);
        fogTexture.Apply();
    }
}





