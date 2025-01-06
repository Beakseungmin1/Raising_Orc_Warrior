using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PassiveStatManager : Singleton<PassiveStatManager>
{
    public float PassiveAtkIncreaseRate { get; private set; }
    public float PassiveCriticalDamageBonus { get; private set; }
    public BigInteger PassiveGoldGainRate { get; private set; }
    public float PassiveHpAndHpRecoveryIncreaseRate { get; private set; }
    public float PassiveMpAndMpRecoveryIncreaseRate { get; private set; }
    public float PassiveAddEXPRate { get; private set; }

    private PlayerStat stat;

    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;


        PassiveGoldGainRate = stat.GetGoldGainRate();
        //PassiveMpAndMpRecoveryIncreaseRate = stat.mana
    }

    public void ResetPassiveStats()
    {
        PassiveAtkIncreaseRate = 0;
        PassiveCriticalDamageBonus = 0;
        PassiveGoldGainRate = 0;
        PassiveHpAndHpRecoveryIncreaseRate = 0;
        PassiveMpAndMpRecoveryIncreaseRate = 0;
        PassiveAddEXPRate = 0;
    }

    public void UpdatePassiveStats(List<Weapon> weapons, List<Accessory> accessories)
    {
        ResetPassiveStats();

        HashSet<string> processedWeapons = new HashSet<string>();
        HashSet<string> processedAccessories = new HashSet<string>();

        foreach (var weapon in weapons)
        {
            if (!processedWeapons.Contains(weapon.BaseData.name))
            {
                PassiveAtkIncreaseRate += weapon.PassiveEquipAtkIncreaseRate;
                PassiveCriticalDamageBonus += weapon.PassiveCriticalDamageBonus;
                PassiveGoldGainRate += (BigInteger)weapon.PassiveGoldGainRate;
                processedWeapons.Add(weapon.BaseData.name);
            }
        }

        foreach (var accessory in accessories)
        {
            if (!processedAccessories.Contains(accessory.BaseData.name))
            {
                PassiveHpAndHpRecoveryIncreaseRate += accessory.PassiveHpAndHpRecoveryIncreaseRate;
                PassiveMpAndMpRecoveryIncreaseRate += accessory.PassiveMpAndMpRecoveryIncreaseRate;
                PassiveAddEXPRate += accessory.PassiveAddEXPRate;
                processedAccessories.Add(accessory.BaseData.name);
            }
        }

        PlayerObjManager.Instance.Player.stat.ApplyPassiveStats();
    }
}