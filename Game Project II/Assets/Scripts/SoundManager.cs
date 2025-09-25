using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;      // для эффектов (шаги, прыжки, сбор кристаллов)
    public AudioSource ambientSource;  // для амбиента (фон)

    [Header("Clips")]
    public AudioClip jumpClip;
    public AudioClip[] footstepClips;
    public AudioClip ambientClip;

    [Space(10)]
    public AudioClip crystalPickupClip;
    public AudioClip flashlightToggleClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Включаем амбиент при старте
        if (ambientSource != null && ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    // --- Прыжок
    public void PlayJump()
    {
        if (sfxSource != null && jumpClip != null)
            sfxSource.PlayOneShot(jumpClip);
    }

    // --- Шаги
    public void PlayFootsteps()
    {
        if (sfxSource != null && footstepClips.Length > 0 && !sfxSource.isPlaying)
        {
            int index = Random.Range(0, footstepClips.Length);
            sfxSource.clip = footstepClips[index];
            sfxSource.loop = true;
            sfxSource.Play();
        }
    }

    public void StopFootsteps()
    {
        if (sfxSource != null && sfxSource.loop)
        {
            sfxSource.loop = false;
            sfxSource.Stop();
        }
    }

    // --- Сбор кристалла
    public void PlayCrystalPickup()
    {
        if (sfxSource != null && crystalPickupClip != null)
            sfxSource.PlayOneShot(crystalPickupClip);
    }

    // --- Включение/выключение фонарика
    public void PlayFlashlightToggle()
    {
        if (sfxSource != null && flashlightToggleClip != null)
            sfxSource.PlayOneShot(flashlightToggleClip);
    }
}
