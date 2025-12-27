using UnityEngine;
using UnityEngine.InputSystem;

public class FinalDoor : MonoBehaviour
{
    [SerializeField] private int badEndingThreshold = 7;

    private bool playerInRange;
    private bool triggered;

    private void Update()
    {
        if (!playerInRange || triggered || Keyboard.current == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            TryFinishGame();
        }
    }

    private void TryFinishGame()
    {
        triggered = true;

        int notes = NoteManager.Instance.GetDiscoveredNotes().Count;

        if (notes <= badEndingThreshold)
            VictoryUI.Instance.ShowEnding(EndingType.Bad);
        else
            VictoryUI.Instance.ShowEnding(EndingType.Good);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}