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
    private bool isHealtRegenButtonDown;
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
        attackUi.UpdateAttackStatUI();
        healthUi.UpdateHealthStatUI();
        healthRegenUi.UpdateHealthRegenerationUI();
        criticalDamageUi.UpdateCriticalIncreaseDamageUI();
        criticalProbabilityUi.UpdateCriticalProbabilityUI();
        blueCriticalDamageUi.UpdateblueCriticalIncreaseDamageStatUI();
        blueCriticalProbabilityUi.UpdateblueCriticalProbabilityStatUI();
    }

    private void Update()
    {
        if (isAttackButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnAttackLevelUpButton();
                }
            }
        }
        if (isHealthButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnHealthLevelUpButton();
                }
            }
        }
        if (isHealtRegenButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnHealthRegenLevelUpButton();
                }
            }
        }
        if (isCriticalDamageButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnCriticalDamageLevelUpButton();
                }
            }
        }
        if (isCriticalProbabilityButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnCriticalProbabilityLevelUpButton();
                }
            }
        }
        if (isBlueCriticalDamageButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnBlueCriticalDamageLevelUpButton();
                }
            }
        }
        if (isBlueCriticalProbabilityButtonDown)
        {
            holdTimer += Time.deltaTime;
            repeatTimer += Time.deltaTime;
            if (holdTimer >= holdDuration)
            {
                if (repeatTimer >= repeatInterval)
                {
                    repeatTimer = 0f;
                    OnBlueCriticalProbabilityLevelUpButton();
                }
            }
        }

    }

    public void OnAttackLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.AttackLevelUp();
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
        PlayerObjManager.Instance.Player.stat.HealthLevelUp();
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
        PlayerObjManager.Instance.Player.stat.HealthRegenerationLevelUp();
        healthRegenUi.UpdateHealthRegenerationUI();
    }

    public void HealthRegenPointerDown()
    {
        isHealtRegenButtonDown = true;
        holdTimer = 0f;
    }

    public void HealthRegenPointerUp()
    {
        isHealtRegenButtonDown = false;
        holdTimer = 0f;
    }

    public void OnCriticalDamageLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.CriticalIncreaseDamageLevelUp();
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
        PlayerObjManager.Instance.Player.stat.CriticalProbabilityLevelUp();
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
        PlayerObjManager.Instance.Player.stat.BlueCriticalIncreaseDamageLevelUp();
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
        PlayerObjManager.Instance.Player.stat.BlueCriticalProbabilityLevelUp();
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