using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class ShaderGraph2 : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup mapCanvasGroup;
    public RectTransform fogParent;

    [Header("Fog Settings")]
    public GameObject fogTilePrefab;
    public int tilesX = 20;
    public int tilesY = 20;

    [Header("World Map Bounds")]
    public Vector2 worldMapMin;
    public Vector2 worldMapMax;

    [Header("Player Reveal Settings")]
    public CircleCollider2D revealCollider;  // Collider на дочернем объекте игрока

    private FogTile[,] fogTiles;
    private bool fogVisible = false;

    void Start()
    {
        if (mapCanvasGroup != null)
            mapCanvasGroup.alpha = 0f;

        CreateFogTiles();
    }

    void Update()
    {
        // Открытие/закрытие карты кнопкой M
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            fogVisible = !fogVisible;
            if (mapCanvasGroup != null)
                mapCanvasGroup.alpha = fogVisible ? 1f : 0f;

            SetFogVisibility(fogVisible);
        }

        if (revealCollider != null)
            UpdateFogTiles();
    }

    void CreateFogTiles()
    {
        fogTiles = new FogTile[tilesX, tilesY];

        float width = fogParent.rect.width;
        float height = fogParent.rect.height;

        float tileWidth = width / tilesX;
        float tileHeight = height / tilesY;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                GameObject tileGO = Instantiate(fogTilePrefab, fogParent);
                RectTransform rt = tileGO.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(tileWidth, tileHeight);
                rt.anchoredPosition = new Vector2(x * tileWidth, y * tileHeight);

                // Выключаем RaycastTarget, чтобы кнопки кликабельны
                Image img = tileGO.GetComponent<Image>();
                if (img != null) img.raycastTarget = false;

                FogTile fogTile = tileGO.AddComponent<FogTile>();
                fogTile.fogTileRect = rt;

                fogTiles[x, y] = fogTile;
            }
        }
    }

    void SetFogVisibility(bool visible)
    {
        foreach (var tile in fogTiles)
        {
            if (tile != null && !tile.revealed)
                tile.gameObject.SetActive(visible);
        }
    }

    void UpdateFogTiles()
    {
        Vector2 colliderPos = revealCollider.transform.position;
        float colliderRadius = revealCollider.radius * revealCollider.transform.lossyScale.x;

        // Преобразуем в координаты UI
        Vector2 normalizedPos = new Vector2(
            (colliderPos.x - worldMapMin.x) / (worldMapMax.x - worldMapMin.x),
            (colliderPos.y - worldMapMin.y) / (worldMapMax.y - worldMapMin.y)
        );

        Vector2 playerUIPos = new Vector2(
            normalizedPos.x * fogParent.rect.width,
            normalizedPos.y * fogParent.rect.height
        );

        Vector2 normalizedRadius = new Vector2(
            colliderRadius / (worldMapMax.x - worldMapMin.x) * fogParent.rect.width,
            colliderRadius / (worldMapMax.y - worldMapMin.y) * fogParent.rect.height
        );

        float avgRadius = (normalizedRadius.x + normalizedRadius.y) * 0.5f;

        // Проверяем каждый квадратик
        foreach (var tile in fogTiles)
        {
            if (tile != null && !tile.revealed)
            {
                Vector2 tileCenter = tile.fogTileRect.anchoredPosition + tile.fogTileRect.sizeDelta / 2f;
                float dist = Vector2.Distance(tileCenter, playerUIPos);

                if (dist <= avgRadius)
                    tile.Reveal();
            }
        }
    }
}




/*using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FogOfWar : MonoBehaviour
{
    [Header("UI Elements")]
    public CanvasGroup mapCanvasGroup;
    public RectTransform fogParent;

    [Header("Fog Settings")]
    public GameObject fogTilePrefab;
    public int tilesX = 20;
    public int tilesY = 20;

    [Header("World Map Bounds")]
    public Vector2 worldMapMin;
    public Vector2 worldMapMax;

    [Header("Player Reveal Settings")]
    public CircleCollider2D revealCollider;  // Collider на дочернем объекте игрока
    public float maxAlpha = 0.8f;            // максимальная непрозрачность тумана

    private FogTile[,] fogTiles;
    private bool fogVisible = false;

    void Start()
    {
        if (mapCanvasGroup != null)
            mapCanvasGroup.alpha = 0f;

        CreateFogTiles();
    }

    void Update()
    {
        // Открытие/закрытие карты кнопкой M
        if (Keyboard.current != null && Keyboard.current.mKey.wasPressedThisFrame)
        {
            fogVisible = !fogVisible;
            if (mapCanvasGroup != null)
                mapCanvasGroup.alpha = fogVisible ? 1f : 0f;

            SetFogVisibility(fogVisible);
        }

        if (revealCollider != null)
            UpdateFogTiles();
    }

    void CreateFogTiles()
    {
        fogTiles = new FogTile[tilesX, tilesY];

        float width = fogParent.rect.width;
        float height = fogParent.rect.height;

        float tileWidth = width / tilesX;
        float tileHeight = height / tilesY;

        for (int x = 0; x < tilesX; x++)
        {
            for (int y = 0; y < tilesY; y++)
            {
                GameObject tileGO = Instantiate(fogTilePrefab, fogParent);
                tileGO.name = $"FogTile_{x}_{y}";

                RectTransform rt = tileGO.GetComponent<RectTransform>();
                rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0, 0);
                rt.sizeDelta = new Vector2(tileWidth, tileHeight);
                rt.anchoredPosition = new Vector2(x * tileWidth, y * tileHeight);

                // Image префаба чёрный с alpha, Raycast выключен
                Image img = tileGO.GetComponent<Image>();
                if (img != null)
                {
                    img.color = new Color(0f, 0f, 0f, maxAlpha);
                    img.raycastTarget = false;
                }

                FogTile fogTile = tileGO.AddComponent<FogTile>();
                fogTile.fogTileRect = rt;

                fogTiles[x, y] = fogTile;
            }
        }
    }

    void SetFogVisibility(bool visible)
    {
        foreach (var tile in fogTiles)
        {
            if (tile != null)
                tile.gameObject.SetActive(visible);
        }
    }

    void UpdateFogTiles()
    {
        Vector2 colliderPos = revealCollider.transform.position;
        float colliderRadius = revealCollider.radius * revealCollider.transform.lossyScale.x;

        // Преобразуем в координаты UI
        Vector2 normalizedPos = new Vector2(
            (colliderPos.x - worldMapMin.x) / (worldMapMax.x - worldMapMin.x),
            (colliderPos.y - worldMapMin.y) / (worldMapMax.y - worldMapMin.y)
        );

        Vector2 playerUIPos = new Vector2(
            normalizedPos.x * fogParent.rect.width,
            normalizedPos.y * fogParent.rect.height
        );

        Vector2 normalizedRadius = new Vector2(
            colliderRadius / (worldMapMax.x - worldMapMin.x) * fogParent.rect.width,
            colliderRadius / (worldMapMax.y - worldMapMin.y) * fogParent.rect.height
        );

        float avgRadius = (normalizedRadius.x + normalizedRadius.y) * 0.5f;

        foreach (var tile in fogTiles)
        {
            if (tile == null) continue;

            Vector2 tileCenter = tile.fogTileRect.anchoredPosition + tile.fogTileRect.sizeDelta / 2f;
            float dist = Vector2.Distance(tileCenter, playerUIPos);

            // alpha: 0 в центре, maxAlpha на краю круга
            float targetAlpha = Mathf.Clamp01((dist - avgRadius) / avgRadius) * maxAlpha;
            tile.UpdateAlpha(targetAlpha);
        }
    }
}*/

