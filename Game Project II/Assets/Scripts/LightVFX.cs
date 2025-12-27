using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class LightVFX : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Color lightColor = Color.green;
    [SerializeField] private float maxIntensity = 1.5f;
    [SerializeField] private float fadeSpeed = 2f;

    [Header("Optional")]
    [SerializeField] private float maxRadius = 3f;

    private Light2D light2D;
    private float targetIntensity = 0f;
    private float targetRadius = 0f;

    private void Awake()
    {
        light2D = GetComponent<Light2D>();

        light2D.color = lightColor;
        light2D.intensity = 0f;
        light2D.pointLightOuterRadius = 0f;
    }

    private void Update()
    {
        // Плавное изменение интенсивности
        light2D.intensity = Mathf.Lerp(
            light2D.intensity,
            targetIntensity,
            Time.deltaTime * fadeSpeed
        );

        // Плавное изменение радиуса
        light2D.pointLightOuterRadius = Mathf.Lerp(
            light2D.pointLightOuterRadius,
            targetRadius,
            Time.deltaTime * fadeSpeed
        );
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            TurnOff();
        }
    }

    public void TurnOn()
    {
        targetIntensity = maxIntensity;
        targetRadius = maxRadius;
    }

    public void TurnOff()
    {
        targetIntensity = 0f;
        targetRadius = 0f;
    }
}