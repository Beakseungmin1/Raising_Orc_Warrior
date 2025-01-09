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

    private bool isAttackButtonDown;
    private bool isHealthButtonDown;
    private bool isHealthRegenButtonDown;
    private bool isCriticalDamageButtonDown;
    private bool isCriticalProbabilityButtonDown;
    private bool isBlueCriticalDamageButtonDown;
    private bool isBlueCriticalProbabilityButtonDown;

    private float holdDuration = 0.5f; // n초 동안 누르고 있으면 시작
    private float repeatInterval = 0.05f; // n초 마다 실행
    private float holdTimer = 0f;
    private float repeatTimer = 0f;

    private PlayerStat playerstat;

    private void Start()
    {
        playerstat = PlayerObjManager.Instance.Player.stat;
        playerstat.UpdateAllStatUI += UpdateAllStatUI;


        //임시
        UpdateAllStatUI();
        //playerstat.UpdateAllStatUI.Invoke();

    }

    private void Update()
    {
        HandleButtonHold(ref isAttackButtonDown, OnAttackLevelUpButton);
        HandleButtonHold(ref isHealthButtonDown, OnHealthLevelUpButton);
        HandleButtonHold(ref isHealthRegenButtonDown, OnHealthRegenLevelUpButton);
        HandleButtonHold(ref isCriticalDamageButtonDown, OnCriticalDamageLevelUpButton);
        HandleButtonHold(ref isCriticalProbabilityButtonDown, OnCriticalProbabilityLevelUpButton);
        HandleButtonHold(ref isBlueCriticalDamageButtonDown, OnBlueCriticalDamageLevelUpButton);
        HandleButtonHold(ref isBlueCriticalProbabilityButtonDown, OnBlueCriticalProbabilityLevelUpButton);
    }

    public void UpdateAllStatUI()
    {
        attackUi.UpdateAttackStatUI();
        healthUi.UpdateHealthStatUI();
        healthRegenUi.UpdateHealthRegenerationUI();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }

    private void HandleButtonHold(ref bool isButtonDown, System.Action upgradeAction)
    {
        if (isButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration && repeatTimer >= repeatInterval)
            {
                repeatTimer = 0f;
                upgradeAction();
            }
        }
    }

    public void OnAttackLevelUpButton()
    {
        playerstat.AttackLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        attackUi.UpdateAttackStatUI();
    }

    public void AttackPointerDown()
    {
        isAttackButtonDown = true;
        holdTimer = 0f;
    }

    public void AttackPointerUp()
    {
        isAttackButtonDown = false;
        holdTimer = 0f;
    }

    public void OnHealthLevelUpButton()
    {
        playerstat.HealthLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        healthUi.UpdateHealthStatUI();
    }

    public void HealthPointerDown()
    {
        isHealthButtonDown = true;
        holdTimer = 0f;
    }

    public void HealthPointerUp()
    {
        isHealthButtonDown = false;
        holdTimer = 0f;
    }


    public void OnHealthRegenLevelUpButton()
    {
        playerstat.HealthRegenerationLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        healthRegenUi.UpdateHealthRegenerationUI();
    }

    public void HealthRegenPointerDown()
    {
        isHealthRegenButtonDown = true;
        holdTimer = 0f;
    }

    public void HealthRegenPointerUp()
    {
        isHealthRegenButtonDown = false;
        holdTimer = 0f;
    }

    public void OnCriticalDamageLevelUpButton()
    {
        playerstat.CriticalIncreaseDamageLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
    }

    public void CriDamagePointerDown()
    {
        isCriticalDamageButtonDown = true;
        holdTimer = 0f;
    }

    public void CriDamagePointerUp()
    {
        isCriticalDamageButtonDown = false;
        holdTimer = 0f;
    }

    public void OnCriticalProbabilityLevelUpButton()
    {
        playerstat.CriticalProbabilityLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
    }

    public void CriProbabilityPointerDown()
    {
        isCriticalProbabilityButtonDown = true;
        holdTimer = 0f;
    }

    public void CriProbabilityPointerUp()
    {
        isCriticalProbabilityButtonDown = false;
        holdTimer = 0f;
    }


    public void OnBlueCriticalDamageLevelUpButton()
    {
        playerstat.BlueCriticalIncreaseDamageLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
    }

    public void BlueCriDamagePointerDown()
    {
        isBlueCriticalDamageButtonDown = true;
        holdTimer = 0f;
    }

    public void BlueCriDamagePointerUp()
    {
        isBlueCriticalDamageButtonDown = false;
        holdTimer = 0f;
    }

    public void OnBlueCriticalProbabilityLevelUpButton()
    {
        playerstat.BlueCriticalProbabilityLevelUp();
        PlayerObjManager.Instance.Player.stat.UpdateNeedMoney();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }

    public void BlueCriProbabilityPointerDown()
    {
        isBlueCriticalProbabilityButtonDown = true;
        holdTimer = 0f;
    }

    public void BlueCriProbabilityPointerUp()
    {
        isBlueCriticalDamageButtonDown = false;
        holdTimer = 0f;
    }
}