using System.Collections;
using UnityEngine;

public class BuffSkill : BaseSkill
{
    public override void Activate(UnityEngine.Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        SkillEffect effect = GetSkillEffect(targetPosition);
        SkillEffectManager.Instance.TriggerEffect(this, UnityEngine.Vector3.zero);
        ResetCondition();

        ApplySpeedBoost(effect);
        ApplyManaBoost(effect);
        ApplyAttackBoost(effect);
        StartCoroutine(EndBuffAfterDuration());
    }

    private void ApplySpeedBoost(SkillEffect effect)
    {
        if (effect.MoveSpeedIncrease > 0)
        {
            BackgroundManager.Instance.ParallaxBackground.scrollSpeed *= 1 + (effect.MoveSpeedIncrease / 100f);
        }
    }

    private void ApplyManaBoost(SkillEffect effect)
    {
        if (effect.ManaRecoveryAmount > 0)
        {
            playerStat.setMana(effect.ManaRecoveryAmount);
        }
    }

    private void ApplyAttackBoost(SkillEffect effect)
    {
        if (effect.AttackSpeedIncrease > 0)
        {
            player.ChangeAnimatorSpeed(effect.AttackSpeedIncrease);
        }
    }

    private IEnumerator EndBuffAfterDuration()
    {
        yield return new WaitForSeconds(skillEffect.BuffDuration);
        EndEffect();
    }

    protected override void EndEffect()
    {
        base.EndEffect();

        BackgroundManager.Instance.ParallaxBackground.ChangeScrollSpeed();

        player.ChangeAnimatorSpeed(0.8f);
    }

    protected override void EnhanceSkill()
    {
        if (skillEffect.AttackIncreasePercent > 0)
        {
            skillEffect.AttackIncreasePercent *= 1.05f;
            skillEffect.AttackIncreasePercent = Mathf.RoundToInt(skillEffect.AttackIncreasePercent);
        }

        if (skillEffect.ManaRecoveryAmount > 0)
        {
            skillEffect.ManaRecoveryAmount += 10;
            skillEffect.ManaRecoveryAmount = Mathf.RoundToInt(skillEffect.ManaRecoveryAmount);
        }

        if (skillEffect.MoveSpeedIncrease > 0)
        {
            skillEffect.MoveSpeedIncrease += 10f;
            skillEffect.MoveSpeedIncrease = Mathf.RoundToInt(skillEffect.MoveSpeedIncrease);
        }

        if (skillEffect.AttackSpeedIncrease > 0)
        {
            skillEffect.AttackSpeedIncrease += 0.1f;
        }
    }
}