using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class EnemyBase : MonoBehaviour, IDamageable
{
    [SerializeField] private BigInteger hp;
    [SerializeField] private BigInteger maxHp;

    public virtual void TakeDamage(BigInteger damage)
    {
        DamageUISystem.Instance.ShowDamage(damage, transform.position, transform);
    }
}