using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FogTile : MonoBehaviour
{
    public float fadeSpeed = 2f;
    private Image image;
    private bool revealed = false;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void Reveal()
    {
        if (revealed) return;
        revealed = true;
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Color c = image.color;

        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeSpeed;
            image.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}