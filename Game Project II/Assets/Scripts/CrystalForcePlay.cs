using UnityEngine;

public class CrystalForcePlay : MonoBehaviour
{
    public SpriteRenderer sr;         // спрайт кристалла
    public Color[] colors;            // цвета для перелива
    public float speed = 2f;          // скорость перелива

    private int current = 0;
    private float t = 0f;

    void Start()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (colors == null || colors.Length == 0)
            colors = new Color[] { Color.white, Color.cyan, Color.magenta, Color.white };

        sr.color = colors[0];
    }

    void Update()
    {
        if (colors.Length < 2) return;

        t += Time.deltaTime * speed;
        sr.color = Color.Lerp(colors[current], colors[(current + 1) % colors.Length], t);

        if (t >= 1f)
        {
            t = 0f;
            current = (current + 1) % colors.Length;
        }
    }
}