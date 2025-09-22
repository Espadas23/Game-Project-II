using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FlashlightZoom : MonoBehaviour
{
    [Header("References")]
    public FlashlightController flashlight;   // скрипт фонарика
    public CinemachineCamera cineCam;         // объект CinemachineCamera
    public Transform player;                  // игрок

    [Header("Zoom Settings")]
    public float normalSize = 5f;             // размер камеры без зума
    public float zoomedSize = 3f;             // размер камеры при зуме
    public float zoomSpeed = 8f;              // скорость перехода
    public float maxOffset = 2f;              // радиус смещения к курсору

    private float currentSize;

    void Start()
    {
        if (cineCam == null || player == null || flashlight == null)
        {
            Debug.LogError("⚠️ Не назначены cineCam, player или flashlight!");
            return;
        }

        // Проверяем, чтобы Follow был пустым
        if (cineCam.Follow != null)
        {
            Debug.LogWarning("Follow у CinemachineCamera должен быть None для ручного управления.");
            cineCam.Follow = null;
        }

        currentSize = cineCam.Lens.OrthographicSize;
    }

    void Update()
    {
        float targetSize = normalSize;
        Vector3 targetPosition = player.position;
        targetPosition.z = cineCam.transform.position.z;

        if (flashlight.isOn && Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            targetSize = zoomedSize;

            // Позиция мыши в мире
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );

            // Вектор от игрока к мыши
            Vector2 dir = mouseWorld - (Vector3)player.position;

            // Ограничиваем радиус
            if (dir.magnitude > maxOffset)
                dir = dir.normalized * maxOffset;

            // Смещение камеры к курсору
            targetPosition = player.position + new Vector3(dir.x, dir.y, 0f);
            targetPosition.z = cineCam.transform.position.z;
        }

        // Плавный переход размера камеры
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * zoomSpeed);
        cineCam.Lens.OrthographicSize = currentSize;

        // Плавный переход позиции камеры
        cineCam.transform.position = Vector3.Lerp(cineCam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}
