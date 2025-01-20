using System;
using System.Collections;
using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    [SerializeField]
    public float scrollSpeed = 2f;

    [SerializeField]
    private Transform[] backgrounds;

    private float backgroundWidth;
    private bool isScrolling = true;
    private bool isBattlePaused = false;
    private bool isKnockback = false;
    private float defaultSpeed;

    public event Action<float> OnKnockback;


    private void Start()
    {
        if (backgrounds.Length > 0)
        {
            defaultSpeed = scrollSpeed;

            SpriteRenderer spriteRenderer = backgrounds[0].GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                // 배경 가로 길이 계산
                backgroundWidth = spriteRenderer.bounds.size.x;
            }

            // 두 번째 배경 배치
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

    public float GetdefaultSpeedSpeed()
    {
        return defaultSpeed;
    }

    public void ChangeScrollSpeed()
    {
        scrollSpeed = defaultSpeed;
    }

    private void ScrollBackground(Vector3 direction)
    {
        foreach (var background in backgrounds)
        {
            background.position += direction * Time.deltaTime;
        }

        // 두 번째 그림의 중앙쯤 지났을 때 첫 번째 그림을 뒤로 이동
        if (backgrounds[1].position.x <= 0)
        {
            RepositionBackgrounds();
        }
    }

    private void RepositionBackgrounds()
    {
        // 첫 번째 그림을 두 번째 그림 뒤로 이동
        backgrounds[0].position = new Vector3(
            backgrounds[1].position.x + backgroundWidth,
            backgrounds[0].position.y,
            backgrounds[0].position.z
        );

        // 첫 번째와 두 번째 그림 순서 스왑
        Transform temp = backgrounds[0];
        backgrounds[0] = backgrounds[1];
        backgrounds[1] = temp;

    }

    public void StartScrollingRight(float knockbackTime)
    {
        isKnockback = false;
        if (!isKnockback)
        {
            isKnockback = true;
            OnKnockback?.Invoke(knockbackTime);
            Debug.Log(isKnockback);
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
