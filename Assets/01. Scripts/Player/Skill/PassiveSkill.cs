using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private void Start()
    {
        var playerBattle = PlayerObjManager.Instance?.Player?.GetComponent<PlayerBattle>();
        if (playerBattle != null)
        {
            playerBattle.OnPlayerAttack += IncrementHitCount;
        }
    }

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && IsSkillEquipped())
        {
            if (cooldownTimer <= 0 && currentHits >= skillData.requiredHits)
            {
                Activate(Vector3.zero);
                ResetSkillState();
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (cooldownTimer > 0 || currentHits < skillData.requiredHits) return;

        Debug.Log("패시브 스킬 발동!");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetSkillState();
    }

    private void ResetCooldown()
    {
        cooldownTimer = skillData.cooldown;
    }

    public void ResetSkillState()
    {
        ResetCooldown();
        currentHits = 0;
    }

    private void IncrementHitCount()
    {
        currentHits++;
    }

    private bool IsSkillEquipped()
    {
        return PlayerObjManager.Instance?.Player?.GetComponent<PlayerSkillHandler>()?.IsSkillEquipped(this) ?? false;
    }

    protected override void EnhanceSkill() { }
}