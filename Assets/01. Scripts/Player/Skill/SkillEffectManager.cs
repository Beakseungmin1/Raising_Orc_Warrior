using UnityEngine;
using System.Collections;
using System.Numerics;

public class SkillEffectManager : Singleton<SkillEffectManager>
{
    public Transform playerWeaponPosition;
    public Transform mapCenter;
    private PlayerDamageCalculator playerDamageCalculator;

    private void Start()
    {
        if (playerWeaponPosition == null || mapCenter == null)
        {
            Debug.LogError("SkillEffectManager: 필수 Transform이 설정되지 않았습니다.");
        }

        playerDamageCalculator = PlayerObjManager.Instance?.Player?.DamageCalculator;
    }

    public void TriggerEffect(BaseSkill skill, UnityEngine.Vector3 targetPosition)
    {
        if (skill == null)
        {
            return;
        }

        SkillEffect effect = skill.GetSkillEffect(targetPosition);

        playerDamageCalculator.ApplySkillEffect(effect);

        HandleEffect(effect);
    }

    private void HandleEffect(SkillEffect effect)
    {
        switch (effect.EffectType)
        {
            case EffectType.OnPlayer:
                HandleWeaponEffect(effect);
                break;

            case EffectType.OnMapCenter:
                HandleAreaEffect(effect);
                break;

            case EffectType.Projectile:
                HandleProjectileEffect(effect);
                break;

            default:
                break;
        }
    }

    private void HandleWeaponEffect(SkillEffect effect)
    {
        if (playerWeaponPosition == null)
        {
            return;
        }

        GameObject effectObj = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);

        StartCoroutine(PerformWeaponEffect(effect, effectObj));
    }

    private IEnumerator PerformWeaponEffect(SkillEffect effect, GameObject effectObj)
    {
        yield return new WaitForSeconds(0.35f);

        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        if (targets.Length == 0)
        {
            Destroy(effectObj, effect.EffectDuration);
            yield break;
        }

        Collider2D closestTarget = null;
        float closestDistance = float.MaxValue;

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                float distance = UnityEngine.Vector2.Distance(playerWeaponPosition.position, target.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        if (closestTarget != null)
        {
            IEnemy enemy = closestTarget.GetComponent<IEnemy>();
            if (enemy != null)
            {
                BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                enemy.TakeDamage(skillDamage);
            }
        }

        Destroy(effectObj, effect.EffectDuration);
    }

    private void HandleAreaEffect(SkillEffect effect)
    {
        if (mapCenter == null)
        {
            return;
        }

        StartCoroutine(PerformAreaEffect(effect));
    }

    private IEnumerator PerformAreaEffect(SkillEffect effect)
    {
        yield return new WaitForSeconds(0.3f);

        Collider2D[] targets = Physics2D.OverlapCircleAll(playerWeaponPosition.position, effect.EffectRange);

        foreach (var target in targets)
        {
            if (target.CompareTag("Monster"))
            {
                GameObject effectOnTarget = Instantiate(effect.SkillPrefab, target.transform.position, UnityEngine.Quaternion.identity);
                effectOnTarget.transform.SetParent(target.transform);

                IEnemy enemy = target.GetComponent<IEnemy>();
                if (enemy != null)
                {
                    BigInteger skillDamage = playerDamageCalculator.CalculateSkillDamage(effect.DamagePercent);
                    enemy.TakeDamage(skillDamage);
                }

                Destroy(effectOnTarget, effect.EffectDuration);
            }
        }
    }

    private void HandleProjectileEffect(SkillEffect effect)
    {
        if (effect.SkillPrefab == null)
        {
            return;
        }

        GameObject projectile = Instantiate(effect.SkillPrefab, playerWeaponPosition.position, UnityEngine.Quaternion.identity);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new UnityEngine.Vector2(effect.EffectRange, 0);
        }

        ProjectileHandler projectileHandler = projectile.GetComponent<ProjectileHandler>();
        if (projectileHandler != null)
        {
            projectileHandler.Initialize(effect);
        }

        Destroy(projectile, effect.EffectDuration);
    }
}