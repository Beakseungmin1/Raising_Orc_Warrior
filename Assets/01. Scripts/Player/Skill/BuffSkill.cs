using System.Collections;
using UnityEngine;

public class BuffSkill : BaseSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);
        ResetCondition();
        StartCoroutine(EndBuffAfterDuration());
    }

    private IEnumerator EndBuffAfterDuration()
    {
        yield return new WaitForSeconds(skillData.buffDuration);
        EndEffect();
    }

    protected override void EndEffect()
    {
        base.EndEffect();
    }

    protected override void EnhanceSkill() { }
}