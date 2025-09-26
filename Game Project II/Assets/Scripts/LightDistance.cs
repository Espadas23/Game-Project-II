/*using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class LightDistance : MonoBehaviour
{
    public Transform flashlight;       
    public Transform player;           
    public LayerMask obstacleLayer;    
    public float maxDistance = 5f;     
    public float spotlightAngle = 45f; 
    public float lerpSpeed = 5f;       
    [Range(1f, 5f)] public float intensityMultiplier = 3f;

    private SpriteRenderer sr;
    private Color normalColor;
    private Color highlightColor = Color.white;
    private float target = 0f;
    private float current = 0f;

    private FlashlightController flashlightController;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;

        if (flashlight != null)
            flashlightController = flashlight.GetComponent<FlashlightController>();
    }

    void Update()
    {
        if (flashlight == null || player == null || flashlightController == null) return;

        // Если фонарик выключен — эффекта нет
        if (!flashlightController.isOn)
        {
            target = 0f;
        }
        else
        {
            Vector2 dir = (Vector2)transform.position - (Vector2)flashlight.position;
            float distance = dir.magnitude;

            if (distance > maxDistance)
            {
                target = 0f;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(flashlight.position, dir, distance, obstacleLayer);
                if (hit.collider != null)
                    target = 0f;
                else
                {
                    Vector2 flashlightDir = flashlight.up;
                    float angle = Vector2.Angle(flashlightDir, dir);
                    target = (angle <= spotlightAngle / 2f) ? 1f : 0f;
                }
            }
        }

        current = Mathf.Lerp(current, target, Time.deltaTime * lerpSpeed);
        sr.color = normalColor + (highlightColor * intensityMultiplier - normalColor) * current;
    }
}*/

using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(SpriteRenderer))]
public class LightDistance : MonoBehaviour
{
    public Transform flashlight;       
    public Transform player;           
    public LayerMask obstacleLayer;    
    public float maxDistance = 5f;     
    public float spotlightAngle = 45f; 
    public float lerpSpeed = 5f;       
    [Range(1f, 5f)] public float intensityMultiplier = 3f;

    private SpriteRenderer sr;
    private Color normalColor;
    private Color highlightColor = Color.white;
    private float target = 0f;
    private float current = 0f;

    private FlashlightController flashlightController;
    private Light2D flashlightLight; // доступ к радиусу фонаря
    private float maxRadius;         // запоминаем максимальный радиус фонаря

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        normalColor = sr.color;

        if (flashlight != null)
        {
            flashlightController = flashlight.GetComponent<FlashlightController>();
            flashlightLight = flashlight.GetComponent<Light2D>();

            if (flashlightLight != null)
                maxRadius = flashlightLight.pointLightOuterRadius;
        }
    }

    void Update()
    {
        if (flashlight == null || player == null || flashlightController == null || flashlightLight == null) return;

        // Если фонарик выключен или радиус = 0 → эффекта нет
        if (!flashlightController.isOn || flashlightLight.pointLightOuterRadius <= 0.01f)
        {
            target = 0f;
        }
        else
        {
            Vector2 dir = (Vector2)transform.position - (Vector2)flashlight.position;
            float distance = dir.magnitude;

            if (distance > maxDistance)
            {
                target = 0f;
            }
            else
            {
                RaycastHit2D hit = Physics2D.Raycast(flashlight.position, dir, distance, obstacleLayer);
                if (hit.collider != null)
                {
                    target = 0f;
                }
                else
                {
                    Vector2 flashlightDir = flashlight.up;
                    float angle = Vector2.Angle(flashlightDir, dir);

                    if (angle <= spotlightAngle / 2f)
                    {
                        // фактор яркости от радиуса фонаря (по кривой)
                        float radiusFactor = Mathf.Clamp01(flashlightLight.pointLightOuterRadius / maxRadius);
                        radiusFactor = Mathf.SmoothStep(0f, 1f, radiusFactor); 
                        target = radiusFactor;
                    }
                    else
                    {
                        target = 0f;
                    }
                }
            }
        }

        current = Mathf.Lerp(current, target, Time.deltaTime * lerpSpeed);
        sr.color = normalColor + (highlightColor * intensityMultiplier - normalColor) * current;
    }
}

