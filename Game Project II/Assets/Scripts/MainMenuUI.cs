using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;

    [Header("Settings UI")]
    public Slider sfxSlider;
    public Slider ambientSlider;

    private void Start()
    {
        // Загружаем громкость из PlayerPrefs
        float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);

        if (sfxSlider != null)
        {
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (ambientSlider != null)
        {
            ambientSlider.value = ambientVolume;
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
        }

        SetSFXVolume(sfxVolume);
        SetAmbientVolume(ambientVolume);

        // Останавливаем игру до старта
        Time.timeScale = 0f;

        // Показываем главное меню
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        if (settingsPanel != null) settingsPanel.SetActive(true);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    public void BackToMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    private void SetSFXVolume(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetSFXVolume(value);

        PlayerPrefs.SetFloat("SFXVolume", value);
        PlayerPrefs.Save();
    }

    private void SetAmbientVolume(float value)
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.SetAmbientVolume(value);

        PlayerPrefs.SetFloat("AmbientVolume", value);
        PlayerPrefs.Save();
    }
}
