using UnityEngine;

public class CrystalForcePlay : MonoBehaviour
{
    [Header("Перелив")]
    public SpriteRenderer sr;         // спрайт кристалла
    public Color[] colors;            // цвета для перелива
    public float glowSpeed = 2f;      // скорость перелива

    private int currentColor = 0;
    private float t = 0f;

    [Header("Вращение")]
    public Vector3 rotationAxis = new Vector3(1, 1, 0); // ось вращения
    public float rotationSpeed = 90f;                   // градусов в секунду

    [Header("Пульсация")]
    public float pulseAmplitude = 0.1f;  // насколько сильно увеличивается/уменьшается
    public float pulseSpeed = 2f;        // скорость пульсации
    private Vector3 initialScale;

    [Header("Сбор")]
    public string playerTag = "Player"; // тег персонажа

    void Start()
    {
        if (sr == null)
            sr = GetComponent<SpriteRenderer>();

        if (colors == null || colors.Length == 0)
            colors = new Color[] { Color.white, Color.cyan, Color.magenta, Color.white };

        sr.color = colors[0];
        initialScale = transform.localScale;
    }

    void Update()
    {
        // --- Перелив ---
        if (colors.Length >= 2)
        {
            t += Time.deltaTime * glowSpeed;
            sr.color = Color.Lerp(colors[currentColor], colors[(currentColor + 1) % colors.Length], t);

            if (t >= 1f)
            {
                t = 0f;
                currentColor = (currentColor + 1) % colors.Length;
            }
        }

        // --- Вращение ---
        transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime, Space.World);

        // --- Пульсация ---
        float scaleFactor = 1 + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
        transform.localScale = initialScale * scaleFactor;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            Collect();
        }
    }

    void Collect()
    {
        // Здесь можно добавить эффект сбора: звук, частицы и т.д.
        Debug.Log("Crystall Collected");
        Destroy(gameObject);
    }
}
