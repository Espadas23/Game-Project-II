using UnityEngine;

public class CrystalForcePlay : MonoBehaviour
{
    [Header("Перелив")]
    public SpriteRenderer sr;         // спрайт кристалла
    public Color[] colors;            // цвета для перелива
    public float speed = 2f;          // скорость перелива

    private int current = 0;
    private float t = 0f;

    [Header("Вращение")]
    public Vector3 rotationAxis = new Vector3(1, 1, 0); // ось вращения
    public float rotationSpeed = 90f; // градусов в секунду

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
        // --- Перелив ---
        if (colors.Length >= 2)
        {
            t += Time.deltaTime * speed;
            sr.color = Color.Lerp(colors[current], colors[(current + 1) % colors.Length], t);

            if (t >= 1f)
            {
                t = 0f;
                current = (current + 1) % colors.Length;
            }
        }

        // --- Вращение ---
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.World);
    }
}