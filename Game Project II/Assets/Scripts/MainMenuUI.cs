
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class MainMenuUI : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainMenuPanel;    // Главное меню
    public GameObject settingsPanel;    // Панель с кнопками Sound/HotKeys/Back
    public GameObject soundPanel;       // Подменю Sound (ползунки)
    public GameObject hotkeysPanel;     // Подменю Hot Keys
    public GameObject pausePanel;       // Пауза (Resume + Settings)
    public GameObject notesMenuPanel;   // Новая панель «Книжка записок»

    [Header("Settings UI")]
    public Slider sfxSlider;
    public Slider ambientSlider;

    private bool isPaused = false;

    private void Start()
    {
        ShowMainMenu();

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

        Time.timeScale = 0f;
    }

    private void Update()
    {
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }

    // --- Главное меню ---
    public void StartGame()
    {
        HideAllPanels();
        Time.timeScale = 1f;
    }

    public void OpenSettings()
    {
        HideAllPanels();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSound()
    {
        HideAllPanels();
        if (soundPanel != null) soundPanel.SetActive(true);
    }

    public void OpenHotKeys()
    {
        HideAllPanels();
        if (hotkeysPanel != null) hotkeysPanel.SetActive(true);
    }

    public void BackToSettings()
    {
        HideAllPanels();
        if (settingsPanel != null) settingsPanel.SetActive(true);
    }

    public void BackToMenu()
    {
        HideAllPanels();
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
    }

    // --- Пауза ---
    private void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        HideAllPanels();
        if (pausePanel != null) pausePanel.SetActive(true);
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        HideAllPanels();
    }

    // --- Новые методы для книжки записок ---
    public void OpenNotesMenu()
    {
        if (notesMenuPanel != null)
            notesMenuPanel.SetActive(true);
    }

    public void CloseNotesMenu()
    {
        if (notesMenuPanel != null)
            notesMenuPanel.SetActive(false);
    }

    // --- Сохранение громкости ---
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

    // --- Вспомогательные методы ---
    private void ShowMainMenu()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(true);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hotkeysPanel != null) hotkeysPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (notesMenuPanel != null) notesMenuPanel.SetActive(false);
    }

    private void HideAllPanels()
    {
        if (mainMenuPanel != null) mainMenuPanel.SetActive(false);
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (soundPanel != null) soundPanel.SetActive(false);
        if (hotkeysPanel != null) hotkeysPanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (notesMenuPanel != null) notesMenuPanel.SetActive(false);
    }
}


