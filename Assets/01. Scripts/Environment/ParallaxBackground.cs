using System.Collections;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    private float scrollSpeed = 2f;

    [SerializeField]
    private Transform[] backgrounds;

    private float backgroundWidth;
    private bool isBattlePaused = false;
    private bool isKnockback = false;

    private void Start()
    {
        if (backgrounds.Length > 0)
        {
            SpriteRenderer spriteRenderer = backgrounds[0].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                backgroundWidth = Mathf.Round(spriteRenderer.bounds.size.x);
            }

            // 배경을 초기 위치에 정확히 배치
            for (int i = 1; i < backgrounds.Length; i++)
            {
                backgrounds[i].position = new Vector3(
                    Mathf.Round(backgrounds[i - 1].position.x + backgroundWidth),
                    backgrounds[i - 1].position.y,
                    backgrounds[i - 1].position.z
                );
            }
        }

        BattleManager.Instance.OnBattleStart += PauseScroll;
        BattleManager.Instance.OnBattleEnd += ResumeScroll;
    }

    private void OnDestroy()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStart -= PauseScroll;
            BattleManager.Instance.OnBattleEnd -= ResumeScroll;
        }
    }

    private void Update()
    {
        if (isBattlePaused) return;


        if (isKnockback)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].position += Vector3.right * (scrollSpeed * 4) * Time.deltaTime;
            }
        }
        else
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                backgrounds[i].position += Vector3.left * scrollSpeed * Time.deltaTime;

                if (backgrounds[i].position.x <= -backgroundWidth)
                {
                    float rightmostPosition = GetRightmostBackgroundPosition();
                    backgrounds[i].position = new Vector3(
                        Mathf.Round(rightmostPosition + backgroundWidth),
                        backgrounds[i].position.y,
                        backgrounds[i].position.z
                    );
                }
            }
        }
    }

    public void StartScrollingRight(float KnockbackTime)
    {
        if (!isKnockback)
        {
            isKnockback = true;
            StartCoroutine(StopScrollingRightAfterDuration(KnockbackTime));
        }
    }

    private IEnumerator StopScrollingRightAfterDuration(float KnockbackTime)
    {
        yield return new WaitForSeconds(KnockbackTime);
        isKnockback = false;
    }

    private float GetRightmostBackgroundPosition()
    {
        float maxX = backgrounds[0].position.x;

        for (int i = 1; i < backgrounds.Length; i++)
        {
            if (backgrounds[i].position.x > maxX)
            {
                maxX = backgrounds[i].position.x;
            }
        }

        return maxX;
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