/*using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class TilemapShadowChunker : MonoBehaviour
{
    public Tilemap tilemap;
    public int chunkSize = 32; // размер чанка
    public CompositeShadowCaster2D composite;

    [ContextMenu("Generate Shadow Chunks")]
    public void GenerateChunks()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found!");
            return;
        }

        ClearChunks();

        BoundsInt bounds = tilemap.cellBounds;
        Vector3 cellSize = tilemap.cellSize;

        for (int cx = bounds.xMin; cx <= bounds.xMax; cx += chunkSize)
        {
            for (int cy = bounds.yMin; cy <= bounds.yMax; cy += chunkSize)
            {
                bool hasTile = false;
                for (int x = 0; x < chunkSize && cx + x <= bounds.xMax; x++)
                    for (int y = 0; y < chunkSize && cy + y <= bounds.yMax; y++)
                        if (tilemap.GetTile(new Vector3Int(cx + x, cy + y, 0)) != null)
                        {
                            hasTile = true;
                            break;
                        }

                if (!hasTile) continue;

                Vector3 chunkWorldPos = tilemap.CellToWorld(new Vector3Int(cx, cy, 0));
                chunkWorldPos += new Vector3(cellSize.x * chunkSize / 2f, cellSize.y * chunkSize / 2f, 0f);

                GameObject chunkGO = new GameObject($"ShadowChunk_{cx}_{cy}");
                chunkGO.transform.SetParent(transform, false);
                chunkGO.transform.position = chunkWorldPos;

                // PolygonCollider2D, пустой, isTrigger = true
                var poly = chunkGO.AddComponent<PolygonCollider2D>();
                poly.isTrigger = true;

                // ShadowCaster2D
                var sc = chunkGO.AddComponent<ShadowCaster2D>();
                sc.castsShadows = true;
                sc.selfShadows = false;
#if UNITY_2022_1_OR_NEWER
                sc.alphaCutoff = 1f;
#endif
            }
        }

        if (composite == null)
            composite = GetComponent<CompositeShadowCaster2D>() ?? gameObject.AddComponent<CompositeShadowCaster2D>();
    }

    [ContextMenu("Clear Shadow Chunks")]
    public void ClearChunks()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var c = transform.GetChild(i);
            if (c.name.StartsWith("ShadowChunk"))
                DestroyImmediate(c.gameObject);
        }
    }
}*/

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

[ExecuteInEditMode]
public class TilemapShadowChunker : MonoBehaviour
{
    public Tilemap tilemap;         // Исходный Tilemap с визуальными тайлами
    public int chunkSize = 32;      // Размер чанка в тайлах
    public CompositeShadowCaster2D composite;

    [ContextMenu("Generate Shadow Chunks")]
    public void GenerateChunks()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        if (tilemap == null)
        {
            Debug.LogError("Tilemap not found!");
            return;
        }

        ClearChunks();

        BoundsInt bounds = tilemap.cellBounds;
        Vector3Int min = bounds.min;
        Vector3Int max = bounds.max;

        for (int cx = min.x; cx < max.x; cx += chunkSize)
        {
            for (int cy = min.y; cy < max.y; cy += chunkSize)
            {
                // Проверяем, есть ли тайлы в чанке
                bool hasTile = false;
                for (int x = 0; x < chunkSize && cx + x < max.x; x++)
                {
                    for (int y = 0; y < chunkSize && cy + y < max.y; y++)
                    {
                        if (tilemap.GetTile(new Vector3Int(cx + x, cy + y, 0)) != null)
                        {
                            hasTile = true;
                            break;
                        }
                    }
                    if (hasTile) break;
                }
                if (!hasTile) continue;

                // Создаём дочерний Tilemap для чанка
                GameObject chunkGO = new GameObject($"ShadowChunk_{cx}_{cy}", typeof(Grid));
                chunkGO.transform.SetParent(transform, false);

                // Копируем позицию и родителя
                chunkGO.transform.localPosition = Vector3.zero;

                Grid grid = chunkGO.GetComponent<Grid>();
                grid.cellSize = tilemap.layoutGrid.cellSize;

                Tilemap chunkTilemap = chunkGO.AddComponent<Tilemap>();
                TilemapRenderer renderer = chunkGO.AddComponent<TilemapRenderer>();
                renderer.sortOrder = tilemap.GetComponent<TilemapRenderer>().sortOrder;

                // Копируем тайлы из исходного тайлмапа
                for (int x = 0; x < chunkSize && cx + x < max.x; x++)
                {
                    for (int y = 0; y < chunkSize && cy + y < max.y; y++)
                    {
                        Vector3Int pos = new Vector3Int(cx + x, cy + y, 0);
                        var tile = tilemap.GetTile(pos);
                        if (tile != null)
                        {
                            chunkTilemap.SetTile(pos, tile);
                        }
                    }
                }

                // TilemapCollider2D и ShadowCaster2D
                TilemapCollider2D tmc = chunkGO.AddComponent<TilemapCollider2D>();
                tmc.isTrigger = true;
                // Merge можно включить вручную в инспекторе

                Rigidbody2D rb = chunkGO.AddComponent<Rigidbody2D>();
                rb.bodyType = RigidbodyType2D.Static;
                rb.simulated = false;

                ShadowCaster2D sc = chunkGO.AddComponent<ShadowCaster2D>();
                sc.castsShadows = true;
                sc.selfShadows = false;
#if UNITY_2022_1_OR_NEWER
                sc.alphaCutoff = 1f;
#endif
            }
        }

        if (composite == null)
            composite = GetComponent<CompositeShadowCaster2D>() ?? gameObject.AddComponent<CompositeShadowCaster2D>();
    }

    [ContextMenu("Clear Shadow Chunks")]
    public void ClearChunks()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            var c = transform.GetChild(i);
            if (c.name.StartsWith("ShadowChunk"))
                DestroyImmediate(c.gameObject);
        }
    }
}






