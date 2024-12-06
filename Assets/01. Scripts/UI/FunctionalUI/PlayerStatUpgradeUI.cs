using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatUpgradeUI : Singleton<PlayerStatUpgradeUI>
{
    public UpgradeTabUI attackUi;
    public UpgradeTabUI healthUi;
    public UpgradeTabUI healthRegenUi;
    public UpgradeTabUI criticalDamageUi;
    public UpgradeTabUI criticalProbabilityUi;
    public UpgradeTabUI blueCriticalDamageUi;
    public UpgradeTabUI blueCriticalProbabilityUi;


    public void OnAttackLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.AttackLevelUp();
    }
    public void OnHealthLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.HealthLevelUp();
    }
    public void OnHealthRegenLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.HealthRegenerationLevelUp();
    }
    public void OnCriticalDamageLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.CriticalIncreaseDamageLevelUp();
    }
    public void OnCriticalProbabilityLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.CriticalProbabilityLevelUp();
    }
    public void OnBlueCriticalDamageLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.BlueCriticalIncreaseDamageLevelUp();
    }
    public void OnBlueCriticalProbabilityLevelUpButto()
    {
        PlayerobjManager.Instance.Player.stat.BlueCriticalProbabilityLevelUp();
    }








}
