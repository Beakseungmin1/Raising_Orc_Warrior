using System.Collections;
using UnityEngine;
using System.Numerics;

public class BuffSkill : BaseSkill
{
    public override void Activate(UnityEngine.Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        SkillEffect effect = GetSkillEffect(targetPosition);
        SkillEffectManager.Instance.TriggerEffect(this, UnityEngine.Vector3.zero);
        ResetCondition();

        ApplySpeedBoost();
        ApplyManaBoost();
        ApplyAttackBoost();
        StartCoroutine(EndBuffAfterDuration());
    }

    private void ApplySpeedBoost()
    {
        if (skillData.moveSpeedIncrease > 0)
        {
            ParallaxBackground.Instance.scrollSpeed *= 1 + (skillData.moveSpeedIncrease / 100f);
        }
    }

    private void ApplyManaBoost()
    {
        if (skillData.manaRecoveryAmount > 0)
        {
            playerStat.setMana(skillData.manaRecoveryAmount);
        }
    }

    private void ApplyAttackBoost()
    {
        if (skillData.attackSpeedIncrease > 0)
        {
            player.ChangeAnimatorSpeed(skillData.attackSpeedIncrease);
        }
    }

    private IEnumerator EndBuffAfterDuration()
    {
        yield return new WaitForSeconds(skillData.buffDuration);
        EndEffect();
    }

    protected override void EndEffect()
    {
        base.EndEffect();

        ParallaxBackground.Instance.scrollSpeed /= 1 + (skillData.moveSpeedIncrease / 100f);

        player.ChangeAnimatorSpeed(0.8f);
    }

    protected override void EnhanceSkill() { }
}