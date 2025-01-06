using System.Numerics;
using TMPro;
using UnityEngine;
using System.Collections;

public class DamageUISystem : Singleton<DamageUISystem>
{
    [SerializeField] private TextMeshPro damageTextPrefab;
    private PlayerDamageCalculator playerDamageCalculator;
    private bool isCriticalHit = false;

    private Enemy enemyComponent;
    private EnemyBoss enemyBossComponent;
    private EnemyDungeonBoss enemyDungeonBossComponent;

    private void Start()
    {
        playerDamageCalculator = PlayerObjManager.Instance.Player.DamageCalculator;
        playerDamageCalculator.OnCriticalHit += HandleCriticalHit;
    }

    private void CacheEnemyComponents(Transform enemyTransform)
    {
        if (enemyComponent == null)
            enemyComponent = enemyTransform.GetComponent<Enemy>();

        if (enemyBossComponent == null)
            enemyBossComponent = enemyTransform.GetComponent<EnemyBoss>();

        if (enemyDungeonBossComponent == null)
            enemyDungeonBossComponent = enemyTransform.GetComponent<EnemyDungeonBoss>();
    }

    private void HandleCriticalHit()
    {
        isCriticalHit = true;
    }

    public void ShowDamage(BigInteger damage, UnityEngine.Vector3 position, Transform enemyTransform)
    {
        EnemyBase enemyBaseComponent = enemyTransform.GetComponent<EnemyBase>();

        if (enemyBaseComponent == null)
        {
            return;
        }

        float heightMultiplier = 1.0f;

        if (enemyBaseComponent is Enemy)
        {
            heightMultiplier = 1.0f;
        }
        else if (enemyBaseComponent is EnemyBoss)
        {
            heightMultiplier = 2.0f;
        }
        else if (enemyBaseComponent is EnemyDungeonBoss)
        {
            heightMultiplier = 1.0f;
        }

        TextMeshPro damageText = Instantiate(damageTextPrefab, enemyTransform);
        damageText.text = damage.ToString();

        if (isCriticalHit)
        {
            damageText.color = Color.red;
            isCriticalHit = false;
        }
        else
        {
            damageText.color = new Color(255f / 255f, 165f / 255f, 0f / 255f);
        }

        float heightAdjustment = enemyTransform.localScale.y * heightMultiplier;
        UnityEngine.Vector3 adjustedPosition = position + UnityEngine.Vector3.up * heightAdjustment;

        damageText.transform.position = adjustedPosition;

        StartCoroutine(FollowEnemy(damageText, adjustedPosition));

        Destroy(damageText.gameObject, 0.5f);
    }

    private IEnumerator FollowEnemy(TextMeshPro damageText, UnityEngine.Vector3 initialWorldPosition)
    {
        float timeElapsed = 0f;

        while (timeElapsed < 0.5f)
        {
            if (damageText == null)
                yield break;

            damageText.transform.position = initialWorldPosition;

            timeElapsed += Time.deltaTime;
            yield return null;
        }
    }
}