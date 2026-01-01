using UnityEngine;
using UnityEngine.InputSystem;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialManager tutorialManager;
    private bool playerInside;

    void Update()
    {
        if (playerInside && Keyboard.current.tKey.wasPressedThisFrame)
        {
            tutorialManager.StartTutorial();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInside = false;
    }
}