/*using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CodeDoor : MonoBehaviour
{
    [Header("Связанные элементы")]
    public GameObject codePanel;           // UI панель с картинами и полями
    public TMP_InputField[] codeInputs;    // массив TMP Input Field
    public string[] correctCode;           // правильный код

    [Header("Флаги поведения")]
    public bool isFinalDoor = false;       // true → финальная дверь
    public bool isOpenableDoor = true;     // true → обычная дверь

    [Header("Скрипт управления игроком")]
    public MonoBehaviour playerMovement;

    [Header("Дверь")]
    public GameObject doorObject;          // объект двери с Collider2D

    [Header("Звук при успешном открытии двери")]
    public AudioClip successSound;
    private AudioSource audioSource;

    private bool playerInRange = false;    // игрок в триггере
    private bool isPanelOpen = false;      // панель открыта?
    private int currentInputIndex = 0;     
    private bool isDoorOpen = false;       // дверь открыта?

    private void Start()
    {
        if (codePanel != null)
            codePanel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        for (int i = 0; i < codeInputs.Length; i++)
        {
            int index = i;
            codeInputs[i].onValueChanged.AddListener((string value) => OnInputChanged(index, value));
        }
    }

    private void Update()
    {
        if (!playerInRange || Keyboard.current == null) return;

        // E открывает/закрывает панель
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TogglePanel();
        }

        // Enter проверяет код
        if (isPanelOpen && Keyboard.current.enterKey.wasPressedThisFrame)
        {
            CheckCode();
        }
    }

    private void TogglePanel()
    {
        if (codePanel == null) return;

        isPanelOpen = !isPanelOpen;
        codePanel.SetActive(isPanelOpen);

        if (playerMovement != null)
            playerMovement.enabled = !isPanelOpen;

        if (isPanelOpen)
        {
            ResetInputs();
            currentInputIndex = 0;
            EventSystem.current.SetSelectedGameObject(codeInputs[currentInputIndex].gameObject);
            codeInputs[currentInputIndex].ActivateInputField();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnInputChanged(int index, string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        codeInputs[index].text = value.Substring(0, 1);

        if (index + 1 < codeInputs.Length)
        {
            currentInputIndex = index + 1;
            EventSystem.current.SetSelectedGameObject(codeInputs[currentInputIndex].gameObject);
            codeInputs[currentInputIndex].ActivateInputField();
        }
    }

    public void CheckCode()
    {
        bool correct = true;

        for (int i = 0; i < codeInputs.Length; i++)
        {
            if (i >= correctCode.Length) break;

            if (codeInputs[i].text.Trim() != correctCode[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            if (successSound != null)
                audioSource.PlayOneShot(successSound);

            Debug.Log("Код верный! Дверь открыта.");
            codePanel.SetActive(false);
            isPanelOpen = false;
            if (playerMovement != null) playerMovement.enabled = true;

            if (isOpenableDoor && doorObject != null)
            {
                Collider2D col = doorObject.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
                isDoorOpen = true;
            }

            if (isFinalDoor)
            {
                Debug.Log("🎉 Победа! Конец игры.");
                // GameManager.EndGame() или сцена победы
            }
        }
        else
        {
            Debug.Log("Неверный код!");
            ResetInputs();
            currentInputIndex = 0;
            EventSystem.current.SetSelectedGameObject(codeInputs[currentInputIndex].gameObject);
            codeInputs[currentInputIndex].ActivateInputField();
        }
    }

    private void ResetInputs()
    {
        foreach (var input in codeInputs)
            input.text = "";
    }

    // Скрипт вешается на триггер (IsTrigger = true)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (codePanel != null)
                codePanel.SetActive(false);
            isPanelOpen = false;
            if (playerMovement != null) playerMovement.enabled = true;
            ResetInputs();
        }
    }
}*/




using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CodeDoor : MonoBehaviour
{
    [Header("Связанные элементы")]
    public GameObject codePanel;
    public TMP_InputField[] codeInputs;
    public string[] correctCode;

    [Header("Флаги поведения")]
    public bool isFinalDoor = false;
    public bool isOpenableDoor = true;

    [Header("Дверь")]
    public GameObject doorObject;

    [Header("Звук при успешном открытии двери")]
    public AudioClip successSound;
    private AudioSource audioSource;

    private bool playerInRange = false;
    private bool isPanelOpen = false;
    private int currentInputIndex = 0;
    private bool isDoorOpen = false;
    public bool IsGameWon { get; private set; }

    private void Start()
    {
        if (codePanel != null)
            codePanel.SetActive(false);

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        for (int i = 0; i < codeInputs.Length; i++)
        {
            int index = i;
            codeInputs[i].onValueChanged.AddListener((string value) =>
                OnInputChanged(index, value));
        }
    }

    private void Update()
    {
        if (!playerInRange || Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
            TogglePanel();

        if (isPanelOpen && Keyboard.current.enterKey.wasPressedThisFrame)
            CheckCode();
    }

    private void TogglePanel()
    {
        if (codePanel == null) return;

        isPanelOpen = !isPanelOpen;
        codePanel.SetActive(isPanelOpen);

        if (isPanelOpen)
        {
            ResetInputs();
            currentInputIndex = 0;
            EventSystem.current.SetSelectedGameObject(codeInputs[0].gameObject);
            codeInputs[0].ActivateInputField();
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(null);
        }
    }

    private void OnInputChanged(int index, string value)
    {
        if (string.IsNullOrEmpty(value)) return;

        codeInputs[index].text = value.Substring(0, 1);

        if (index + 1 < codeInputs.Length)
        {
            currentInputIndex = index + 1;
            EventSystem.current.SetSelectedGameObject(codeInputs[currentInputIndex].gameObject);
            codeInputs[currentInputIndex].ActivateInputField();
        }
    }

    public void CheckCode()
    {
        bool correct = true;

        for (int i = 0; i < codeInputs.Length; i++)
        {
            if (i >= correctCode.Length) break;

            if (codeInputs[i].text.Trim() != correctCode[i])
            {
                correct = false;
                break;
            }
        }

        if (correct)
        {
            if (successSound != null)
                audioSource.PlayOneShot(successSound);

            Debug.Log("Код верный! Дверь открыта.");
            codePanel.SetActive(false);
            isPanelOpen = false;

            if (isOpenableDoor && doorObject != null)
            {
                Collider2D col = doorObject.GetComponent<Collider2D>();
                if (col != null) col.enabled = false;
                isDoorOpen = true;
            }

            if (isFinalDoor)
            {
                EndGame();
            }
        }
        else
        {
            Debug.Log("Неверный код!");
            ResetInputs();
            currentInputIndex = 0;
            EventSystem.current.SetSelectedGameObject(codeInputs[0].gameObject);
            codeInputs[0].ActivateInputField();
        }
    }

    private void EndGame()
    {
        Debug.Log("🎉 Победа! Игра окончена.");

        IsGameWon = true;

        Time.timeScale = 0f;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }


    private void ResetInputs()
    {
        foreach (var input in codeInputs)
            input.text = "";
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (codePanel != null)
                codePanel.SetActive(false);

            isPanelOpen = false;
            ResetInputs();
        }
    }
}

