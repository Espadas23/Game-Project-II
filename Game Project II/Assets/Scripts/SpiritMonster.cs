using UnityEngine;

public class SpiritMonster : MonoBehaviour
{
    [Header("Параметры поведения")]
    public float speed = 2f;
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
    private float lightTimer = 0f;

    private void Update()
    {
        if (isDead || player == null) return;

        CheckLight();

        if (isLit)
        {
            lightTimer += Time.deltaTime;
            if (lightTimer >= lightKillTime)
                Die();
            return;
        }

        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
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
        {
            if (gameOverUI != null)
                gameOverUI.Show();
        }
    }

}