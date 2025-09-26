using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;   // Главное меню с кнопками Start/Settings
    public GameObject settingsPanel;   // Панель настроек с ползунками и Back

    [Header("Settings UI")]
    public Slider sfxSlider;
    public Slider ambientSlider;

    private void Start()
    {
        // Загружаем сохраненные значения громкости
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

        // Главное меню включено
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);

        // Панель Settings скрыта
        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    // --- Кнопка Start ---
    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f; // запуск игры
    }

    // --- Кнопка Settings ---
    public void OpenSettings()
    {
        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(false); // скрываем главное меню

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);

            // Обновляем ползунки на актуальные значения
            if (sfxSlider != null)
                sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);

            if (ambientSlider != null)
                ambientSlider.value = PlayerPrefs.GetFloat("AmbientVolume", 1f);
        }
    }

    // --- Кнопка Back ---
    public void BackToMenu()
    {
        if (settingsPanel != null)
            settingsPanel.SetActive(false); // скрываем панель настроек

        if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true); // возвращаем главное меню
    }

    // --- Установка громкости ---
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
