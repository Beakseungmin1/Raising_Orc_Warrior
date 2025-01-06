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
        StartCoroutine(EndBuffAfterDuration());
    }

    private void ApplySpeedBoost()
    {
        ParallaxBackground.Instance.scrollSpeed *= 1 + (skillData.moveSpeedIncrease / 100f);
    }

    private void ApplyManaBoost()
    {
        if (skillData.manaRecoveryAmount > 0)
        {
            playerStat.setMana(skillData.manaRecoveryAmount);
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
    }

    protected override void EnhanceSkill() { }
}