using System;
using System.Numerics;
using UnityEngine;

public class BossEvents
{
    //보스 체력 깎일때마다 현재 HP값 받아오는 델리게이트
    public event Action<BigInteger> onBossHpChanged;

    public void BossHPChanged(BigInteger currentHealth)
    {
        if (onBossHpChanged != null)
        {
            onBossHpChanged(currentHealth);
        }
    }

    //보스 소환되면서 맥스HP값 받아오는 델리게이트
    public event Action<BigInteger> onSetBossHp;

    public void BossHPSet(BigInteger maxHealth)
    {
        if (onBossHpChanged != null)
        {
            onBossHpChanged(maxHealth);
        }
    }
}