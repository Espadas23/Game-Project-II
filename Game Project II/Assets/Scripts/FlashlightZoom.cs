/*using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FlashlightZoom : MonoBehaviour
{
    public FlashlightController flashlight;
    public CinemachineCamera cineCam;
    public Transform player;

    [Header("Zoom Settings")]
    public float normalSize = 5f;         // обычный зум
    public float zoomedSize = 3f;         // минимальный зум при зуме
    public float maxZoomInExtra = 1f;     // насколько можно приблизить сильнее
    public float zoomSpeed = 8f;          
    public float maxOffset = 2f;          
    public float zoomWheelSpeed = 0.5f;   

    private float currentSize;
    private float wheelOffset = 0f; // накопленный эффект колесика

    void Start()
    {
        if (cineCam == null || player == null || flashlight == null)
        {
            Debug.LogError("Не назначены cineCam, player или flashlight!");
            return;
        }

        if (cineCam.Follow != null)
            cineCam.Follow = null;

        currentSize = cineCam.Lens.OrthographicSize;
    }

    void Update()
    {
        float targetSize = normalSize;
        Vector3 targetPosition = player.position;
        targetPosition.z = cineCam.transform.position.z;

        if (flashlight.isOn && Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            // Реакция на колесико
            float scroll = Mouse.current.scroll.ReadValue().y;
            wheelOffset -= scroll * zoomWheelSpeed;

            // Ограничиваем диапазон wheelOffset
            wheelOffset = Mathf.Clamp(wheelOffset, -maxZoomInExtra, normalSize - zoomedSize);

            // Целевой зум = минимальный базовый зум + wheelOffset
            targetSize = zoomedSize + wheelOffset;

            // Позиция мыши в мире
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );

            Vector2 dir = mouseWorld - (Vector3)player.position;
            if (dir.magnitude > maxOffset)
                dir = dir.normalized * maxOffset;

            targetPosition = player.position + new Vector3(dir.x, dir.y, 0f);
            targetPosition.z = cineCam.transform.position.z;
        }
        else
        {
            // ПКМ отпущена → возвращаем wheelOffset в 0
            wheelOffset = 0f;
        }

        // Плавный переход размера и позиции
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * zoomSpeed);
        cineCam.Lens.OrthographicSize = currentSize;
        cineCam.transform.position = Vector3.Lerp(cineCam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}*/


/*using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FlashlightZoom : MonoBehaviour
{
    public FlashlightController flashlight;
    public CinemachineCamera cineCam;
    public Transform player;

    [Header("Zoom Settings")]
    public float normalSize = 5f;         // обычный зум
    public float zoomedSize = 3f;         // минимальный зум при зуме
    public float maxZoomInExtra = 1f;     // насколько можно приблизить сильнее
    public float zoomSpeed = 8f;          
    public float maxOffset = 2f;          
    public float zoomWheelSpeed = 0.5f;   

    private float currentSize;
    private float wheelOffset = 0f; // накопленный эффект колесика

    [HideInInspector] public bool isZoomed = false;       // ПКМ зажата
    [HideInInspector] public bool isExtraZoomed = false;  // колесиком ушли ниже zoomedSize

    void Start()
    {
        if (cineCam == null || player == null || flashlight == null)
        {
            Debug.LogError("Не назначены cineCam, player или flashlight!");
            return;
        }

        if (cineCam.Follow != null)
            cineCam.Follow = null;

        currentSize = cineCam.Lens.OrthographicSize;
    }

    void Update()
    {
        float targetSize = normalSize;
        Vector3 targetPosition = player.position;
        targetPosition.z = cineCam.transform.position.z;

        // --- Проверяем состояние зума ---
        isZoomed = false;
        isExtraZoomed = false;

        if (flashlight.isOn && Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            isZoomed = true;

            // Реакция на колесико
            float scroll = Mouse.current.scroll.ReadValue().y;
            wheelOffset -= scroll * zoomWheelSpeed;

            // Ограничиваем диапазон wheelOffset
            wheelOffset = Mathf.Clamp(wheelOffset, -maxZoomInExtra, normalSize - zoomedSize);

            // Целевой зум = минимальный базовый зум + wheelOffset
            targetSize = zoomedSize + wheelOffset;

            // Проверка экстра-зума
            if (targetSize < zoomedSize + 0.01f) // ушли на самый минимум
                isExtraZoomed = true;

            // Позиция мыши в мире
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );

            Vector2 dir = mouseWorld - (Vector3)player.position;
            if (dir.magnitude > maxOffset)
                dir = dir.normalized * maxOffset;

            targetPosition = player.position + new Vector3(dir.x, dir.y, 0f);
            targetPosition.z = cineCam.transform.position.z;
        }
        else
        {
            // ПКМ отпущена → возвращаем wheelOffset в 0
            wheelOffset = 0f;
        }

        // Плавный переход размера и позиции
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * zoomSpeed);
        cineCam.Lens.OrthographicSize = currentSize;
        cineCam.transform.position = Vector3.Lerp(cineCam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}*/

using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class FlashlightZoom : MonoBehaviour
{
    public FlashlightController flashlight;
    public CinemachineCamera cineCam;
    public Transform player;

    [Header("Zoom Settings")]
    public float normalSize = 5f;         // обычный зум
    public float zoomedSize = 3f;         // минимальный зум при зуме
    public float maxZoomInExtra = 1f;     // насколько можно приблизить сильнее
    public float zoomSpeed = 8f;          
    public float maxOffset = 2f;          
    public float zoomWheelSpeed = 0.5f;   

    private float currentSize;
    private float wheelOffset = 0f; // накопленный эффект колесика

    [HideInInspector] public bool isZoomed = false;       // ПКМ зажата
    [HideInInspector] public bool isExtraZoomed = false;  // активен экстра-зум

    // Публичное свойство для безопасного доступа извне
    public float WheelOffset => wheelOffset;

    void Start()
    {
        if (cineCam == null || player == null || flashlight == null)
        {
            Debug.LogError("Не назначены cineCam, player или flashlight!");
            return;
        }

        if (cineCam.Follow != null)
            cineCam.Follow = null;

        currentSize = cineCam.Lens.OrthographicSize;
    }

    void Update()
    {
        float targetSize = normalSize;
        Vector3 targetPosition = player.position;
        targetPosition.z = cineCam.transform.position.z;

        // --- Сброс флагов ---
        isZoomed = false;
        isExtraZoomed = false;

        // --- Зум активен ---
        if (flashlight.isOn && Mouse.current != null && Mouse.current.rightButton.isPressed)
        {
            isZoomed = true;

            // Реакция на колесико
            float scroll = Mouse.current.scroll.ReadValue().y;
            wheelOffset -= scroll * zoomWheelSpeed;

            // Ограничиваем диапазон wheelOffset
            wheelOffset = Mathf.Clamp(wheelOffset, -maxZoomInExtra, normalSize - zoomedSize);

            // Целевой зум = минимальный базовый зум + wheelOffset
            targetSize = zoomedSize + wheelOffset;

            // Экстра-зум активен только если колесико прокручено
            if (wheelOffset < -0.01f)
                isExtraZoomed = true;

            // Позиция мыши в мире
            Vector2 mouseScreen = Mouse.current.position.ReadValue();
            Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(
                new Vector3(mouseScreen.x, mouseScreen.y, -Camera.main.transform.position.z)
            );

            Vector2 dir = mouseWorld - (Vector3)player.position;
            if (dir.magnitude > maxOffset)
                dir = dir.normalized * maxOffset;

            targetPosition = player.position + new Vector3(dir.x, dir.y, 0f);
            targetPosition.z = cineCam.transform.position.z;
        }
        else
        {
            // ПКМ отпущена → сбрасываем колесико
            wheelOffset = 0f;
        }

        // Плавное обновление размера и позиции камеры
        currentSize = Mathf.Lerp(currentSize, targetSize, Time.deltaTime * zoomSpeed);
        cineCam.Lens.OrthographicSize = currentSize;
        cineCam.transform.position = Vector3.Lerp(cineCam.transform.position, targetPosition, Time.deltaTime * zoomSpeed);
    }
}


