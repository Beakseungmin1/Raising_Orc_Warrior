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

    private void Start()
    {
        //юс╫ц
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
        PlayerobjManager.Instance.Player.stat.AttackLevelUp();
        attackUi.UpdateAttackStatUI();
    }
    public void OnHealthLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.HealthLevelUp();
        healthUi.UpdateHealthStatUI();
    }
    public void OnHealthRegenLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.HealthRegenerationLevelUp();
        healthRegenUi.UpdateHealthRegenerationUI();
    }
    public void OnCriticalDamageLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.CriticalIncreaseDamageLevelUp();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
    }
    public void OnCriticalProbabilityLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.CriticalProbabilityLevelUp();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
    }
    public void OnBlueCriticalDamageLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.BlueCriticalIncreaseDamageLevelUp();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
    }
    public void OnBlueCriticalProbabilityLevelUpButto()
    {
        PlayerobjManager.Instance.Player.stat.BlueCriticalProbabilityLevelUp();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }








}
