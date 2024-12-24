using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private float stopDistance = 1.5f;
    private bool canMove = true;
    private Transform frontEnemy;

    private void OnEnable()
    {
        BattleManager.Instance.OnBattleStart += StopMovement;
        BattleManager.Instance.OnBattleEnd += ResumeMovement;
    }

    private void OnDisable()
    {
        if (BattleManager.Instance != null)
        {
            BattleManager.Instance.OnBattleStart -= StopMovement;
            BattleManager.Instance.OnBattleEnd -= ResumeMovement;
        }
    }

    private void Start()
    {
        canMove = !BattleManager.Instance.IsBattleActive;
    }

    private void Update()
    {
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

    private void MoveLeft()
    {
        transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
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
        canMove = true;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }
}