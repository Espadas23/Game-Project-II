using UnityEngine;

public class VictoryUI : MonoBehaviour
{
    [Header("Ссылки")]
    public CodeDoor finalDoor;          // ссылка на Final Door
    public GameObject victoryPanel;     // панель победы

    private bool victoryShown = false;

    private void Start()
    {
        if (victoryPanel != null)
            victoryPanel.SetActive(false);
    }

    private void Update()
    {
        if (finalDoor == null || victoryPanel == null)
            return;

        if (!victoryShown && finalDoor.IsGameWon)
        {
            ShowVictory();
        }
    }

    private void ShowVictory()
    {
        victoryShown = true;
        victoryPanel.SetActive(true);

        Debug.Log("🏆 Victory UI показан");
    }
}