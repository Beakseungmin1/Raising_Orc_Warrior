using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public interface IEnemy
{
    BigInteger GiveExp();

    BigInteger GiveMoney();

    void Die();
    bool GetActive();

    void TakeDamage(BigInteger Damage);
}
