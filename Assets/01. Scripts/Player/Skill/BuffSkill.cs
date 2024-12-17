using System.Collections;
using UnityEngine;

public class BuffSkill : BaseSkill
{
    private bool isBuffActive = false;

    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || isBuffActive || !ConsumeMana()) return;

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        isBuffActive = true;
        StartCoroutine(EndBuffAfterDelay(skillData.buffDuration));
        ResetCondition();
    }

    private IEnumerator EndBuffAfterDelay(float duration)
    {
        yield return new WaitForSeconds(duration);
        isBuffActive = false;
    }
}