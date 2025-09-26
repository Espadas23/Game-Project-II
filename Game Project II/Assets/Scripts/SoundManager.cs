
/*using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;      // для эффектов: шаги, прыжки, кристаллы, фонарь
    public AudioSource ambientSource;  // для амбиента/фоновой музыки

    [Header("Audio Clips")]
    public AudioClip jumpClip;
    public AudioClip[] footstepClips;
    public AudioClip crystalPickupClip;
    public AudioClip flashlightToggleClip;
    public AudioClip ambientClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Включаем амбиент
        if (ambientSource != null && ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    // --- Звуки эффектов
    public void PlayJump()
    {
        if (sfxSource != null && jumpClip != null)
            sfxSource.PlayOneShot(jumpClip);
    }

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

    public void PlayCrystalPickup()
    {
        if (sfxSource != null && crystalPickupClip != null)
            sfxSource.PlayOneShot(crystalPickupClip);
    }

    public void PlayFlashlightToggle()
    {
        if (sfxSource != null && flashlightToggleClip != null)
            sfxSource.PlayOneShot(flashlightToggleClip);
    }

    // --- Регулировка громкости
    public void SetSFXVolume(float value)
    {
        if (sfxSource != null)
            sfxSource.volume = value;
    }

    public void SetAmbientVolume(float value)
    {
        if (ambientSource != null)
            ambientSource.volume = value;
    }
}*/

using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;      
    public AudioSource ambientSource;  

    [Header("Audio Clips")]
    public AudioClip jumpClip;
    public AudioClip[] footstepClips;
    public AudioClip crystalPickupClip;
    public AudioClip flashlightToggleClip;
    public AudioClip ambientClip;

    [Header("Audio Mixer")]
    public AudioMixer audioMixer;  // сюда перетащить MainMixer

    [Header("Smooth Volume Settings")]
    [Tooltip("Скорость плавного изменения громкости")]
    public float smoothSpeed = 5f;

    // внутренние цели громкости
    private float targetSFX = 1f;
    private float targetAmbient = 1f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (ambientSource != null && ambientClip != null)
        {
            ambientSource.clip = ambientClip;
            ambientSource.loop = true;
            ambientSource.Play();
        }
    }

    private void Update()
    {
        // Плавное применение громкости
        if (audioMixer != null)
        {
            float currentSFX, currentAmbient;

            audioMixer.GetFloat("SFXVolume", out currentSFX);
            audioMixer.GetFloat("AmbientVolume", out currentAmbient);

            // Максимальное значение теперь 5× обычного
            float newSFX = Mathf.Lerp(currentSFX, Mathf.Log10(Mathf.Clamp(targetSFX, 0.0001f, 5f)) * 20f, smoothSpeed * Time.unscaledDeltaTime);
            float newAmbient = Mathf.Lerp(currentAmbient, Mathf.Log10(Mathf.Clamp(targetAmbient, 0.0001f, 5f)) * 20f, smoothSpeed * Time.unscaledDeltaTime);

            audioMixer.SetFloat("SFXVolume", newSFX);
            audioMixer.SetFloat("AmbientVolume", newAmbient);
        }
    }

    // --- Звуки эффектов
    public void PlayJump()
    {
        if (sfxSource != null && jumpClip != null)
            sfxSource.PlayOneShot(jumpClip);
    }

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

    public void PlayCrystalPickup()
    {
        if (sfxSource != null && crystalPickupClip != null)
            sfxSource.PlayOneShot(crystalPickupClip);
    }

    public void PlayFlashlightToggle()
    {
        if (sfxSource != null && flashlightToggleClip != null)
            sfxSource.PlayOneShot(flashlightToggleClip);
    }

    // --- Публичные методы для ползунков
    public void SetSFXVolume(float value)
    {
        targetSFX = value; // value теперь может быть от 0 до 5
    }

    public void SetAmbientVolume(float value)
    {
        targetAmbient = value;
    }
}





