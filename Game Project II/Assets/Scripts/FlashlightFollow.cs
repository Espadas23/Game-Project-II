using UnityEngine;
public class FlashlightFollow : MonoBehaviour
{
    public Transform player;  // перетащи персонажа сюда
    private Vector3 offset;   // смещение фонарика относительно персонажа
    private Vector3 initialScale;

    void Start()
    {
        offset = transform.localPosition;  // сохраняем позицию относительно персонажа
        initialScale = transform.localScale;
    }

    void LateUpdate()
    {
        // двигаем фонарик вместе с персонажем
        transform.position = player.position + offset;

        // флип по X, чтобы фонарик смотрел в сторону персонажа
        if (player.localScale.x > 0)
            transform.localScale = new Vector3(Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
        else if (player.localScale.x < 0)
            transform.localScale = new Vector3(-Mathf.Abs(initialScale.x), initialScale.y, initialScale.z);
    }
}






