/*using UnityEngine;

public class EnemyTriggerSpawn : MonoBehaviour
{
    [Header("Монстр для активации")]
    public GameObject enemyToActivate;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && enemyToActivate != null)
        {
            enemyToActivate.SetActive(true);
        }
    }
}*/


using UnityEngine;

public class EnemyTriggerSpawn : MonoBehaviour
{
    public GameObject monsterToActivate;
    public bool spawnOnce = true;
    public bool destroyAfterSpawn = false;

    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        if (spawnOnce && hasSpawned) return;
        if (monsterToActivate == null) return;

        // Просто включаем объект
        monsterToActivate.SetActive(true);

        hasSpawned = true;

        if (destroyAfterSpawn)
            Destroy(gameObject);
    }
}



