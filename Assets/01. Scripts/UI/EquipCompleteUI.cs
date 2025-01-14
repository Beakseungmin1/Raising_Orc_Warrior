using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EquipCompleteUI : UIBase
{
    [SerializeField] private GameObject equipCompleteMessage;
    [SerializeField] private float displayDuration = 0.5f;
    [SerializeField] private float fadeInDuration = 0.25f;
    [SerializeField] private float fadeOutDuration = 0.25f;

    private Coroutine displayCoroutine;

    private void Awake()
    {
        SuggetionGroupUI.OnEquipComplete += ShowEquipCompleteMessage;
    }

    private void Start()
    {
        equipCompleteMessage.SetActive(false);
    }

    public void ShowEquipCompleteMessage()
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        if (displayCoroutine != null)
        {
            StopCoroutine(displayCoroutine);
        }
        displayCoroutine = StartCoroutine(DisplayMessageCoroutine());
    }

    private IEnumerator DisplayMessageCoroutine()
    {
        equipCompleteMessage.SetActive(true);
        SetMessageAlpha(0f);
        yield return FadeInMessage();
        yield return new WaitForSeconds(displayDuration);
        yield return FadeOutMessage();
        equipCompleteMessage.SetActive(false);
        displayCoroutine = null;
    }

    private void SetMessageAlpha(float alpha)
    {
        SpriteRenderer spriteRenderer = equipCompleteMessage.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, alpha);
        }
        else
        {
            Image image = equipCompleteMessage.GetComponent<Image>();
            if (image != null)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alpha);
            }
        }
    }

    private IEnumerator FadeInMessage()
    {
        float elapsedTime = 0f;
        float targetAlpha = 0.6f;
        while (elapsedTime < fadeInDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, targetAlpha, elapsedTime / fadeInDuration);
            SetMessageAlpha(alpha);
            yield return null;
        }
        SetMessageAlpha(targetAlpha);
    }

    private IEnumerator FadeOutMessage()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeOutDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0.6f, 0f, elapsedTime / fadeOutDuration);
            SetMessageAlpha(alpha);
            yield return null;
        }
        SetMessageAlpha(0f);
    }
}