using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;


public class ArmController : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Точка плеча, относительно которой позиционируется рука")]
    public Transform shoulderPoint;
    [Tooltip("Основная камера")]
    public Camera mainCamera;
    [Tooltip("Спрайт руки")]
    public SpriteRenderer spriteRenderer;

    [Header("Flashlight Settings")]
    [Tooltip("Объект фонаря, прикреплённый к руке")]
    public GameObject flashlightObject;
    private Transform flashlightTransform;
    private Light2D flashlightLight;
    private Vector3 flashlightLocalOffset;

    [Header("Arm Settings")]
    [Tooltip("Смещение руки от плеча")]
    public Vector3 offset = Vector3.zero;
    [Tooltip("Ограничивать ли угол вращения руки")]
    public bool clampAngle = true;
    [Tooltip("Минимальный угол вращения")]
    public float minAngle = -45f;
    [Tooltip("Максимальный угол вращения")]
    public float maxAngle = 90f;
    [Tooltip("Сглаживание поворота руки (0 = мгновенно)")]
    public float rotationSmoothing = 0f;
    [Tooltip("Привязка позиции к пиксельной сетке")]
    public bool pixelSnap = true;
    public int pixelsPerUnit = 32;

    [Header("Facing")]
    [Tooltip("Определяет, куда смотрит персонаж")]
    public bool isFacingRight = true;

    [Header("Flashlight Alignment")]
    [Tooltip("Локальный поворот фонаря относительно руки (например, -90 если свет должен идти вбок)")]
    public Vector3 flashlightRotationOffset = new Vector3(0f, 0f, -90f);

    void Reset()
    {
        mainCamera = Camera.main;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (flashlightObject != null)
        {
            flashlightTransform = flashlightObject.transform;
            flashlightLight = flashlightObject.GetComponentInChildren<Light2D>();
            flashlightLocalOffset = flashlightTransform.localPosition;
        }
    }

    void LateUpdate()
    {
        PlayerController player = GetComponentInParent<PlayerController>();
        if (player != null && player.flashlight != null && !player.flashlight.hasActivatedOnce)
        {
            // ставим руку в нейтральное положение (например, прямо вправо)
            transform.rotation = Quaternion.Euler(0f, 0f, player.armController.isFacingRight ? 0f : 180f);
            return;
        }

        if (!shoulderPoint || !mainCamera)
            return;
        
        if (!shoulderPoint || !mainCamera)
            return;

        // --- Позиция руки ---
        transform.position = shoulderPoint.position + offset;

        // --- Позиция мыши в мире ---
        Vector3 mouseScreen = Mouse.current.position.ReadValue();
        float z = Mathf.Abs(mainCamera.transform.position.z - transform.position.z);
        Vector3 mouseWorld = mainCamera.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, z));

        // --- Направление и угол ---
        Vector2 dir = (mouseWorld - transform.position).normalized;
        float targetAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        // 🔧 Исправление инверсии мыши: зеркалим угол, если персонаж смотрит влево
        if (!isFacingRight)
            targetAngle = 180f + targetAngle;

        // --- Ограничение углов ---
        if (clampAngle)
            targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        // --- Плавное вращение ---
        float newAngle = (rotationSmoothing <= 0f)
            ? targetAngle
            : Mathf.LerpAngle(transform.eulerAngles.z, targetAngle, Time.deltaTime * rotationSmoothing);

        transform.rotation = Quaternion.Euler(0f, 0f, newAngle);

        // --- Фонарь ---
        if (flashlightTransform != null)
        {
            // сохраняем локальное смещение фонаря
            Vector3 localOffset = flashlightLocalOffset;
            if (!isFacingRight)
                localOffset.x = -flashlightLocalOffset.x;

            flashlightTransform.position = transform.TransformPoint(localOffset);

            // вычисляем поворот фонаря относительно руки
            Vector3 offsetEuler = flashlightRotationOffset;
            if (!isFacingRight)
                offsetEuler.z = -flashlightRotationOffset.z;

            flashlightTransform.rotation = transform.rotation * Quaternion.Euler(offsetEuler);
        }

        // --- Pixel Snap ---
        if (pixelSnap && pixelsPerUnit > 0)
        {
            Vector3 p = transform.position;
            p.x = Mathf.Round(p.x * pixelsPerUnit) / pixelsPerUnit;
            p.y = Mathf.Round(p.y * pixelsPerUnit) / pixelsPerUnit;
            transform.position = p;
        }
    }
}