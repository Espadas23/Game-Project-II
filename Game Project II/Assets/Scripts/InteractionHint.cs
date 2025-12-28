using UnityEngine;

public class InteractionHint : MonoBehaviour
{
    [SerializeField] private SpriteRenderer eIcon;

    private void Start()
    {
        eIcon.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eIcon.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            eIcon.enabled = false;
        }
    }
}