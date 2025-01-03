using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatUpgradeUI : MonoBehaviour
{
    public UpgradeTabUI attackUi;
    public UpgradeTabUI healthUi;
    public UpgradeTabUI healthRegenUi;
    public UpgradeTabUI criticalDamageUi;
    public UpgradeTabUI criticalProbabilityUi;
    public UpgradeTabUI blueCriticalDamageUi;
    public UpgradeTabUI blueCriticalProbabilityUi;

    private PlayerStat playerstat;

    private void Start()
    {
        attackUi.UpdateAttackStatUI();
        healthUi.UpdateHealthStatUI();
        healthRegenUi.UpdateHealthRegenerationUI();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }
    public void OnAttackLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.AttackLevelUp();
        attackUi.UpdateAttackStatUI();
    }
    public void OnHealthLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.HealthLevelUp();
        healthUi.UpdateHealthStatUI();
    }
    public void OnHealthRegenLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.HealthRegenerationLevelUp();
        healthRegenUi.UpdateHealthRegenerationUI();
    }
    public void OnCriticalDamageLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.CriticalIncreaseDamageLevelUp();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
    }
    public void OnCriticalProbabilityLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.CriticalProbabilityLevelUp();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
    }
    public void OnBlueCriticalDamageLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.BlueCriticalIncreaseDamageLevelUp();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
    }
    public void OnBlueCriticalProbabilityLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.BlueCriticalProbabilityLevelUp();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }
}