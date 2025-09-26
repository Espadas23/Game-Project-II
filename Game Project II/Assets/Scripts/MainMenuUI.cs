using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem; // для новой Input System

public class UIMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;    // Главное меню с кнопками Start/Settings
    public GameObject settingsPanel;    // Настройки (ползунки + Back)
    public GameObject pausePanel;       // Пауза (Resume + Settings)

    [Header("Settings UI")]
    public Slider sfxSlider;
    public Slider ambientSlider;

    private bool isPaused = false;

    private void Start()
    {
        // Панели выключены на старте, кроме главного меню
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        // Загружаем сохранённые значения громкости
        if (sfxSlider != null)
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
            sfxSlider.value = sfxVolume;
            sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        if (ambientSlider != null)
        {
            float ambientVolume = PlayerPrefs.GetFloat("AmbientVolume", 1f);
            ambientSlider.value = ambientVolume;
            ambientSlider.onValueChanged.AddListener(SetAmbientVolume);
        }

        Time.timeScale = 0f; // игра на паузе до нажатия Start
    }

    private void Update()
    {
        // Нажатие Esc для паузы
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // --- Кнопка Start ---
    public void StartGame()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);

        Time.timeScale = 1f; // запуск игры
    }

    // --- Кнопка Settings в главном меню ---
    public void OpenSettings()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);

        if (settingsPanel != null) settingsPanel.SetActive(true);

        // Обновляем значения ползунков
        if (sfxSlider != null)
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume", 1f);
        if (ambientSlider != null)
            ambientSlider.value = PlayerPrefs.GetFloat("AmbientVolume", 1f);
    }

    // --- Кнопка Back в настройках ---
    public void BackToMenu()
    {
        if (settingsPanel != null) settingsPanel.SetActive(false);

        if (pausePanel != null && isPaused)
            pausePanel.SetActive(true);
        else if (mainMenuPanel != null)
            mainMenuPanel.SetActive(true);
    }

    // --- Пауза ---
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
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
