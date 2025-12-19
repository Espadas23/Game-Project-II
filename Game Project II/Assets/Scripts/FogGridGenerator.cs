using UnityEngine;

public class FogGridGenerator : MonoBehaviour
{
    [Header("Настройки сетки")]
    public GameObject fogTilePrefab; // Prefab FogTile с SpriteRenderer
    public int width = 10;           // Кол-во тайлов по X
    public int height = 10;          // Кол-во тайлов по Y
    public float tileSize = 1f;      // Размер тайла в юнитах мира

    void Start()
    {
        if (fogTilePrefab == null)
        {
            Debug.LogError("FogGridGenerator: fogTilePrefab не назначен!");
            return;
        }

        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Создаём тайл и делаем дочерним FogRoot
                GameObject tile = Instantiate(fogTilePrefab, transform);
                tile.name = $"FogTile_{x}_{y}";

                // Позиция в мире
                tile.transform.position = new Vector3(x * tileSize, y * tileSize, 0);

                // Убедимся, что тайл на правильном слое
                tile.layer = LayerMask.NameToLayer("Fog");

                // Убедимся, что SpriteRenderer есть (защита)
                SpriteRenderer sr = tile.GetComponent<SpriteRenderer>();
                if (sr == null)
                {
                    Debug.LogError($"FogTile {tile.name} не имеет SpriteRenderer!");
                }
            }
        }
    }
}