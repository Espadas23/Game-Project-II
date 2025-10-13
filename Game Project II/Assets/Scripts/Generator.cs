using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteAlways]
[RequireComponent(typeof(Tilemap))]
public class Generator : MonoBehaviour
{
    [Tooltip("Форма тени (локальные координаты вершин). По часовой стрелке.")]
    public Vector2[] shapePoints = new Vector2[]
    {
        new Vector2(-0.25f, -0.5f),
        new Vector2(0.25f, -0.5f),
        new Vector2(0f, 0.5f)
    };

    public ShadowOffsetsData offsetsData;

    private Tilemap tilemap;
    private Dictionary<Vector3Int, GameObject> shadowObjects = new Dictionary<Vector3Int, GameObject>();

#if UNITY_EDITOR
    [ContextMenu("Generate Shadows")]
    public void GenerateShadows()
    {
        if (tilemap == null) tilemap = GetComponent<Tilemap>();
        if (offsetsData == null)
        {
            Debug.LogError("Assign ShadowOffsetsData ScriptableObject!");
            return;
        }

        // Удаляем старые ShadowCaster
        foreach (Transform child in transform)
            if (child.name.StartsWith("Shadow_"))
                DestroyImmediate(child.gameObject);

        shadowObjects.Clear();

        BoundsInt bounds = tilemap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int pos = new Vector3Int(x, y, 0);
                TileBase tile = tilemap.GetTile(pos);
                if (tile == null) continue;

                Vector3 offset = offsetsData.GetOffset(pos);

                GameObject go = new GameObject($"Shadow_{x}_{y}");
                go.transform.SetParent(transform, false);
                go.transform.position = tilemap.GetCellCenterWorld(pos) + offset;

                ShadowCaster2D sc = go.AddComponent<ShadowCaster2D>();
                sc.castsShadows = true;
                sc.selfShadows = false;
#if UNITY_2022_1_OR_NEWER
                sc.alphaCutoff = 1f;
#endif

                // Применяем форму треугольника
                ApplyTriangleShape(sc, shapePoints);

                shadowObjects[pos] = go;
            }
        }

        EditorUtility.SetDirty(this);
        EditorUtility.SetDirty(offsetsData);
    }

    private void ApplyTriangleShape(ShadowCaster2D caster, Vector2[] pts)
    {
        if (caster == null || pts == null || pts.Length < 3) return;

        SerializedObject so = new SerializedObject(caster);
        SerializedProperty shapeProp = so.FindProperty("m_ShapePath");
        if (shapeProp == null)
        {
            Debug.LogWarning("Не удалось найти m_ShapePath в ShadowCaster2D");
            return;
        }

        shapeProp.ClearArray();
        shapeProp.arraySize = pts.Length;

        for (int i = 0; i < pts.Length; i++)
        {
            var element = shapeProp.GetArrayElementAtIndex(i);

            if (element.propertyType == SerializedPropertyType.Vector2)
            {
                element.vector2Value = pts[i];
            }
            else if (element.propertyType == SerializedPropertyType.Vector3)
            {
                element.vector3Value = new Vector3(pts[i].x, pts[i].y, 0f);
            }
            else
            {
                Debug.LogWarning("Unknown element type in m_ShapePath");
            }
        }

        so.ApplyModifiedProperties();
        EditorUtility.SetDirty(caster);
    }

    private void OnDrawGizmos()
    {
        if (shadowObjects == null || offsetsData == null) return;
        Gizmos.color = Color.cyan;

        foreach (var kv in shadowObjects)
        {
            var go = kv.Value;
            if (go == null) continue;

            EditorGUI.BeginChangeCheck();
            Vector3 newPos = Handles.PositionHandle(go.transform.position, Quaternion.identity);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(go.transform, "Move ShadowCaster");
                go.transform.position = newPos;

                Vector3Int tilePos = kv.Key;
                Vector3 center = tilemap.GetCellCenterWorld(tilePos);
                Vector3 localOffset = newPos - center;

                offsetsData.SetOffset(tilePos, localOffset);
                EditorUtility.SetDirty(offsetsData);
            }
        }
    }
#endif
}
