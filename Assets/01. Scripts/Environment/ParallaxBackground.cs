using System.Collections;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 2f;

    [SerializeField]
    private Transform[] backgrounds;

    private float backgroundWidth;
    private bool isScrolling = true;
    private bool isBattlePaused = false;
    private bool isKnockback = false;

    private void Start()
    {
        if (backgrounds.Length > 0)
        {
            SpriteRenderer spriteRenderer = backgrounds[0].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // ��� ���� ���� ���
                backgroundWidth = spriteRenderer.bounds.size.x;

                // ����� �α�
                Debug.Log($"Sprite Bounds Size X: {spriteRenderer.bounds.size.x}");
                Debug.Log($"Calculated Background Width: {backgroundWidth}");
            }

            // �� ��° ��� ��ġ
            if (backgrounds.Length > 1)
            {
                backgrounds[1].position = new Vector3(
                    backgrounds[0].position.x + backgroundWidth,
                    backgrounds[0].position.y,
                    backgrounds[0].position.z
                );
            }
        }

        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStart += PauseScroll;
            BattleManager.Instance.OnBattleEnd += ResumeScroll;
        }
    }

    private void Update()
    {
        if (isBattlePaused) return;

        if (isKnockback)
        {
            ScrollBackground(Vector3.right * (scrollSpeed * 4));
        }
        else
        {
            ScrollBackground(Vector3.left * scrollSpeed);
        }
    }

    private void ScrollBackground(Vector3 direction)
    {
        foreach (var background in backgrounds)
        {
            background.position += direction * Time.deltaTime;
        }

        // �� ��° �׸��� �߾��� ������ �� ù ��° �׸��� �ڷ� �̵�
        if (backgrounds[1].position.x <= 0)
        {
            RepositionBackgrounds();
        }
    }

    private void RepositionBackgrounds()
    {
        // ù ��° �׸��� �� ��° �׸� �ڷ� �̵�
        backgrounds[0].position = new Vector3(
            backgrounds[1].position.x + backgroundWidth,
            backgrounds[0].position.y,
            backgrounds[0].position.z
        );

        // ù ��°�� �� ��° �׸� ���� ����
        Transform temp = backgrounds[0];
        backgrounds[0] = backgrounds[1];
        backgrounds[1] = temp;

    }

    public void StartScrollingRight(float knockbackTime)
    {
        if (!isKnockback)
        {
            isKnockback = true;
            StartCoroutine(StopScrollingRightAfterDuration(knockbackTime));
        }
    }

    private IEnumerator StopScrollingRightAfterDuration(float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        isKnockback = false;

        if (!isBattlePaused)
        {
            ResumeScroll();
        }
    }

    private void PauseScroll()
    {
        if (!isKnockback)
        {
            isBattlePaused = true;
        }
    }

    private void ResumeScroll()
    {
        isBattlePaused = false;
    }
}
