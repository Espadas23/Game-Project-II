using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource sfxSource;      // для эффектов (шаги, прыжки и т.д.)
    public AudioSource ambientSource;  // для амбиента (фон)

    [Header("Clips")]
    public AudioClip jumpClip;
    public AudioClip[] footstepClips;
    public AudioClip ambientClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // включаем амбиент при старте
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
}