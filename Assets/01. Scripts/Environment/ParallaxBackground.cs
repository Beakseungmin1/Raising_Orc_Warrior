using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 2f;

    [SerializeField]
    private Transform[] backgrounds;

    private float backgroundWidth;
    private bool isPaused = false;

    private void Start()
    {
        if (backgrounds.Length > 0)
        {
            SpriteRenderer spriteRenderer = backgrounds[0].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                Sprite sprite = spriteRenderer.sprite;
                Texture2D texture = sprite.texture;
                backgroundWidth = texture.width / sprite.pixelsPerUnit;
            }
        }
    }

    private void Update()
    {
        if (isPaused)
            return;

        for (int i = 0; i < backgrounds.Length; i++)
        {
            backgrounds[i].position += Vector3.left * scrollSpeed * Time.deltaTime;

            if (backgrounds[i].position.x <= -backgroundWidth)
            {
                int nextIndex = (i + 1) % backgrounds.Length;
                backgrounds[i].position = new Vector3(backgrounds[nextIndex].position.x + backgroundWidth, backgrounds[i].position.y);
            }
        }
    }

    public void PauseParallax()
    {
        isPaused = true;
    }

    public void ResumeParallax()
    {
        isPaused = false;
    }
}