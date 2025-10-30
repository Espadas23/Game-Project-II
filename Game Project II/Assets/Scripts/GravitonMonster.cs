/*using UnityEngine;

public class GravitonMonster : MonoBehaviour
{
    [Header("Параметры гравитации")]
    public float pullForce = 3f;
    public float lightKillTime = 3f;
    public float lightAngle = 30f;
    public float lightRange = 5f;

    [Header("Ссылки")]
    public Transform player;
    public Transform flashlight;
    public Vector3 flashlightDirection = Vector3.up;
    public GameOverUI gameOverUI;

    private bool isLit = false;
    private bool isDead = false;
    private bool playerInField = false;
    private float lightTimer = 0f;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (isDead || player == null) return;

        // гарантируем, что стоит на месте
        transform.position = startPosition;

        CheckLight();

        if (isLit)
        {
            lightTimer += Time.deltaTime;
            if (lightTimer >= lightKillTime)
                Die();
            return;
        }

        PullPlayer();
    }

    private void PullPlayer()
    {
        if (!playerInField) return;

        Vector2 direction = (transform.position - player.position).normalized;
        float distance = Vector2.Distance(player.position, transform.position);
        float force = Mathf.Lerp(0, pullForce, 1 - Mathf.Clamp01(distance / 3f));

        player.position += (Vector3)(direction * force * Time.deltaTime);
    }

    private void CheckLight()
    {
        if (flashlight == null) return;

        Vector2 dirToEnemy = transform.position - flashlight.position;
        float distance = dirToEnemy.magnitude;

        Vector2 lightDir = flashlight.TransformDirection(flashlightDirection);
        float angle = Vector2.Angle(lightDir, dirToEnemy);

        if (angle < lightAngle / 2f && distance < lightRange)
        {
            isLit = true;
        }
        else
        {
            isLit = false;
            lightTimer = 0f;
        }
    }

    private void Die()
    {
        isDead = true;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Player"))
            playerInField = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInField = false;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead || !other.CompareTag("Player")) return;

        if (!isLit)
        {
            // Просто вызываем GameOverUI при нахождении игрока внутри триггера
            if (gameOverUI != null)
                gameOverUI.Show();
        }
    }

}*/


using UnityEngine;

public class GravitonMonster : MonoBehaviour
{
    [Header("Параметры гравитации")]
    public float pullRadius = 5f;       // радиус втягивания
    public float maxPullForce = 3f;     // максимальная сила притяжения
    public float killDistance = 0.5f;   // дистанция до центра, при которой игрок умирает

    [Header("Параметры света")]
    public float lightKillTime = 3f;
    public float lightAngle = 30f;
    public float lightRange = 5f;

    [Header("Ссылки")]
    public Transform player;
    public Transform flashlight;
    public Vector3 flashlightDirection = Vector3.up;
    public GameOverUI gameOverUI;

    private bool isLit = false;
    private bool isDead = false;

    private void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void Update()
    {
        if (isDead || player == null) return;

        // Остаёмся на месте
        transform.position = transform.position;

        CheckLight();
        if (isLit)
        {
            return;
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (isDead || !other.CompareTag("Player")) return;

        Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
        if (rb == null) return;

        Vector2 dir = (transform.position - other.transform.position).normalized;
        float distance = Vector2.Distance(transform.position, other.transform.position);

        // Сила пропорциональна близости к центру
        float force = Mathf.Lerp(0, maxPullForce, 1f - Mathf.Clamp01(distance / pullRadius));
        rb.AddForce(dir * force, ForceMode2D.Force);

        // Смерть игрока, если близко к центру
        if (distance < killDistance)
        {
            if (gameOverUI != null)
                gameOverUI.Show();
        }
    }

    private void CheckLight()
    {
        if (flashlight == null) return;

        Vector2 dirToMonster = transform.position - flashlight.position;
        float distance = dirToMonster.magnitude;
        Vector2 lightDir = flashlight.TransformDirection(flashlightDirection);
        float angle = Vector2.Angle(lightDir, dirToMonster);

        if (angle < lightAngle / 2f && distance < lightRange)
        {
            isLit = true;
            // Можно добавить логику для исчезновения через свет
            Destroy(gameObject, lightKillTime);
        }
        else
        {
            isLit = false;
        }
    }
}

