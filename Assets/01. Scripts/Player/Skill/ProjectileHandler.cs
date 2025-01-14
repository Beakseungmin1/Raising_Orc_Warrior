using System.Numerics;
using UnityEngine;

public class ProjectileHandler : MonoBehaviour
{
    private SkillEffect effect;

    public void Initialize(SkillEffect effectData)
    {
        effect = effectData;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            Debug.Log($"ProjectileHandler: 몬스터에 명중 - 데미지 전달 시작");

            ApplyDamageToMonster(collision.gameObject);
        }
    }

    private void ApplyDamageToMonster(GameObject monster)
    {
        IEnemy enemy = monster.GetComponent<IEnemy>();

        if (enemy != null)
        {
            BigInteger baseDamage = PlayerObjManager.Instance?.Player?.DamageCalculator.GetTotalDamage(false, true) ?? 0;
            float damageMultiplier = effect.DamagePercent / 100f;
            double adjustedDamage = (double)baseDamage * damageMultiplier;
            BigInteger totalDamage = (BigInteger)adjustedDamage;

            enemy.TakeDamage(totalDamage);
        }
    }
}