/*using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera tutorialCamera;

    [Header("Tutorial Steps")]
    public Transform[] tutorialPoints;
    public GameObject[] tutorialPopups;

    private int currentStep = 0;
    private bool isActive = false;
    private Vector3 tutorialCameraStartPos;

    void Update()
    {
        if (!isActive) return;

        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame)
        {
            NextStep();
        }
    }

    public void StartTutorial()
    {
        if (isActive) return;

        isActive = true;
        currentStep = 0;

        tutorialCameraStartPos = tutorialCamera.transform.position;

        // Переключаем камеры
        mainCamera.enabled = false;
        tutorialCamera.enabled = true;

        // Фризим игру
        Time.timeScale = 0f;

        ShowStep();
    }

    void NextStep()
    {
        if (currentStep < tutorialPopups.Length)
            tutorialPopups[currentStep].SetActive(false);

        currentStep++;

        if (currentStep >= tutorialPoints.Length)
        {
            EndTutorial();
            return;
        }

        ShowStep();
    }

    void ShowStep()
    {
        tutorialCamera.transform.position = new Vector3(
            tutorialPoints[currentStep].position.x,
            tutorialPoints[currentStep].position.y,
            tutorialCamera.transform.position.z
        );

        tutorialPopups[currentStep].SetActive(true);
    }

    void EndTutorial()
    {
        tutorialCamera.transform.position = tutorialCameraStartPos;

        tutorialCamera.enabled = false;
        mainCamera.enabled = true;

        Time.timeScale = 1f;
        isActive = false;
    }
}*/



using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialManager : MonoBehaviour
{
    [Header("Cameras")]
    public Camera mainCamera;
    public Camera tutorialCamera;

    [Header("Tutorial Steps")]
    public Transform[] tutorialPoints;
    public GameObject[] tutorialPopups;

    [Header("Camera Movement")]
    public float moveSpeed = 5f; // скорость перелёта

    private int currentStep = 0;
    private bool isActive = false;
    private Vector3 tutorialCameraStartPos;
    private bool isMoving = false;

    void Update()
    {
        if (!isActive) return;

        // Плавное движение камеры
        if (isMoving)
        {
            Vector3 targetPos = new Vector3(
                tutorialPoints[currentStep].position.x,
                tutorialPoints[currentStep].position.y,
                tutorialCamera.transform.position.z
            );

            tutorialCamera.transform.position = Vector3.MoveTowards(
                tutorialCamera.transform.position,
                targetPos,
                moveSpeed * Time.unscaledDeltaTime
            );

            // Когда дошли до цели
            if (Vector3.Distance(tutorialCamera.transform.position, targetPos) < 0.01f)
            {
                isMoving = false;
                tutorialPopups[currentStep].SetActive(true);
            }
        }

        // Обработка кнопки T
        if (Keyboard.current != null && Keyboard.current.tKey.wasPressedThisFrame && !isMoving)
        {
            NextStep();
        }
    }

    public void StartTutorial()
    {
        if (isActive) return;

        isActive = true;
        currentStep = 0;

        tutorialCameraStartPos = tutorialCamera.transform.position;

        tutorialCamera.enabled = true;
        Time.timeScale = 0f;

        // Отображаем первый шаг сразу
        tutorialCamera.transform.position = new Vector3(
            tutorialPoints[currentStep].position.x,
            tutorialPoints[currentStep].position.y,
            tutorialCamera.transform.position.z
        );
        tutorialPopups[currentStep].SetActive(true);

        // Подготовка для следующего шага
        isMoving = false;
    }

    void NextStep()
    {
        // Скрываем текущий попап
        if (currentStep < tutorialPopups.Length)
            tutorialPopups[currentStep].SetActive(false);

        currentStep++;

        if (currentStep >= tutorialPoints.Length)
        {
            EndTutorial();
            return;
        }

        // Начинаем плавное движение к следующему шагу
        isMoving = true;
    }

    void EndTutorial()
    {
        tutorialCamera.transform.position = tutorialCameraStartPos;

        tutorialCamera.enabled = false;
        Time.timeScale = 1f;
        isActive = false;
        isMoving = false;
    }
}

