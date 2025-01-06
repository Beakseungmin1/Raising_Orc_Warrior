using UnityEngine;
using System.Collections;
using UnityEngine.UIElements;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float stopDistance = 1.5f;
    private bool canMove = true;
    private Transform frontEnemy;
    private bool isKnockback = false;

    private void OnEnable()
    {
        ResetState();

        BattleManager.Instance.OnBattleStart += StopMovement;
        BattleManager.Instance.OnBattleEnd += ResumeMovement;

        ParallaxBackground.Instance.OnKnockback += TriggerKnockback;

        canMove = !BattleManager.Instance.IsBattleActive;
    }

    private void OnDisable()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStart -= StopMovement;
            BattleManager.Instance.OnBattleEnd -= ResumeMovement;
        }

        if (ParallaxBackground.Instance != null)
        {
            ParallaxBackground.Instance.OnKnockback -= TriggerKnockback;
        }
    }

    private void Update()
    {
        if (isKnockback)
        {
            KnockbackMovement();
            return;
        }

        if (canMove)
        {
            MoveLeft();
        }

        if (frontEnemy != null)
        {
            float distanceToFront = Vector3.Distance(transform.position, frontEnemy.position);
            if (distanceToFront < stopDistance)
            {
                canMove = false;
            }
            else if (!BattleManager.Instance.IsBattleActive)
            {
                canMove = true;
            }
        }
    }

    private void ResetState()
    {
        canMove = false;
        isKnockback = false;
        frontEnemy = null;
    }

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
    }

    private void KnockbackMovement()
    {
        // 넉백 중일 때의 움직임 (우측 이동)
        transform.Translate(Vector3.right * (moveSpeed * 4) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            frontEnemy = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (frontEnemy == collision.transform)
            {
                frontEnemy = null;
                canMove = true;
            }
        }
    }

    private void StopMovement()
    {
        canMove = false;
    }

    private void ResumeMovement()
    {
        if (!isKnockback)
        {
            canMove = true;
        }
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void TriggerKnockback(float knockbackTime)
    {
        if (!isKnockback)
        {
            isKnockback = true;
            StartCoroutine(StopKnockbackAfterDuration(knockbackTime));
        }
    }

    private IEnumerator StopKnockbackAfterDuration(float knockbackTime)
    {
        yield return new WaitForSeconds(knockbackTime);
        isKnockback = false;

        if (!BattleManager.Instance.IsBattleActive)
        {
            ResumeMovement();
        }
    }
}