using System.Numerics;
using TMPro;
using UnityEngine;
using System.Collections;

public class DamageUISystem : Singleton<DamageUISystem>
{
    [SerializeField] private TextMeshPro damageTextPrefab;
    private PlayerDamageCalculator playerDamageCalculator;
    private bool isCriticalHit = false;
    private bool isMissHit = false;

    private Enemy enemyComponent;
    private EnemyBoss enemyBossComponent;
    private EnemyDungeonBoss enemyDungeonBossComponent;

    private void Start()
    {
        playerDamageCalculator = PlayerObjManager.Instance.Player.DamageCalculator;
        playerDamageCalculator.OnCriticalHit += HandleCriticalHit;
        playerDamageCalculator.OnMissHit += HandleMissHit;
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

    private void HandleMissHit()
    {
        isMissHit = true;
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
            heightMultiplier = 1.2f;
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
        damageText.text = damage.ToString("#,0");

        if (isMissHit)
        {
            damageText.color = Color.gray;
            damageText.text = "Miss";
            isMissHit = false;
        }
        else if (isCriticalHit)
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

        StartCoroutine(AnimateDamageText(damageText, adjustedPosition));
    }

    private IEnumerator AnimateDamageText(TextMeshPro damageText, UnityEngine.Vector3 initialWorldPosition)
    {
        float growDuration = 0.05f;
        float shrinkDuration = 0.2f;
        float holdDuration = 0.3f;
        float fadeDuration = 0.3f;
        float fastMoveDistance = 1.0f;

        float timeElapsed = 0f;

        Color initialColor = damageText.color;
        UnityEngine.Vector3 startScale = damageText.transform.localScale;
        UnityEngine.Vector3 maxScale = startScale * 2f;

        while (timeElapsed < growDuration)
        {
            if (damageText == null)
                yield break;

            float progress = timeElapsed / growDuration;
            damageText.transform.localScale = UnityEngine.Vector3.Lerp(startScale, maxScale, progress);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        while (timeElapsed < shrinkDuration)
        {
            if (damageText == null)
                yield break;

            float progress = timeElapsed / shrinkDuration;
            damageText.transform.localScale = UnityEngine.Vector3.Lerp(maxScale, startScale, progress);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        while (timeElapsed < holdDuration)
        {
            if (damageText == null)
                yield break;

            damageText.transform.localScale = startScale;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        UnityEngine.Vector3 targetPosition = initialWorldPosition + UnityEngine.Vector3.up * fastMoveDistance;
        while (timeElapsed < fadeDuration)
        {
            if (damageText == null)
                yield break;

            float progress = timeElapsed / fadeDuration;

            damageText.transform.position = UnityEngine.Vector3.Lerp(initialWorldPosition, targetPosition, progress);

            Color fadedColor = initialColor;
            fadedColor.a = Mathf.Lerp(1.0f, 0.0f, progress);
            damageText.color = fadedColor;

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        if (damageText != null)
        {
            Destroy(damageText.gameObject);
        }
    }
}