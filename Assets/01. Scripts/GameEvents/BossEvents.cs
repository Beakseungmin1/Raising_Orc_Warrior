using System;
using System.Numerics;
using UnityEngine;

public class BossEvents
{
    //���� ü�� ���϶����� ���� HP�� �޾ƿ��� ��������Ʈ
    public event Action<BigInteger> onBossHpChanged;

    public void BossHPChanged(BigInteger currentHealth)
    {
        if (onBossHpChanged != null)
        {
            onBossHpChanged(currentHealth);
        }
    }

    //���� ��ȯ�Ǹ鼭 �ƽ�HP�� �޾ƿ��� ��������Ʈ
    public event Action<BigInteger> onSetBossHp;

    public void BossHPSet(BigInteger maxHealth)
    {
        if (onBossHpChanged != null)
        {
            onBossHpChanged(maxHealth);
        }
    }
}