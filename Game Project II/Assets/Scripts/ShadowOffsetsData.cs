using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShadowOffsetsData", menuName = "Tilemap/Shadow Offsets Data")]
public class ShadowOffsetsData : ScriptableObject
{
    [System.Serializable]
    public struct TileOffset
    {
        public Vector3Int tilePos;
        public Vector3 offset;
    }

    public List<TileOffset> offsets = new List<TileOffset>();

    public Vector3 GetOffset(Vector3Int pos)
    {
        foreach (var t in offsets)
            if (t.tilePos == pos) return t.offset;
        return Vector3.zero;
    }

    public void SetOffset(Vector3Int pos, Vector3 offset)
    {
        for (int i = 0; i < offsets.Count; i++)
        {
            if (offsets[i].tilePos == pos)
            {
                var t = offsets[i];
                t.offset = offset;
                offsets[i] = t;
                return;
            }
        }
        offsets.Add(new TileOffset { tilePos = pos, offset = offset });
    }
}