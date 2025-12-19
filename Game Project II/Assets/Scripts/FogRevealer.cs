using UnityEngine;

public class FogRevealer : MonoBehaviour
{
    public float revealRadius = 50f;
    public LayerMask fogLayer;

    void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(
            transform.position,
            revealRadius,
            fogLayer
        );

        foreach (var hit in hits)
        {
            FogTile tile = hit.GetComponent<FogTile>();
            if (tile != null)
            {
                tile.Reveal();
            }
        }
    }
}