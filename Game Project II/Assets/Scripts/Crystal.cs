using UnityEngine;

public class Crystal : MonoBehaviour
{
    [Header("Сбор")]
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Находим компонент фонарика у игрока и сообщаем о сборе
            FlashlightHP hp = other.GetComponentInChildren<FlashlightHP>();
            if (hp != null)
            {
                hp.OnCrystalCollected();
            }

            // 🎵 звук сбора кристалла
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayCrystalPickup();

            // Удаляем кристалл из сцены
            Destroy(gameObject);
        }
    }
}