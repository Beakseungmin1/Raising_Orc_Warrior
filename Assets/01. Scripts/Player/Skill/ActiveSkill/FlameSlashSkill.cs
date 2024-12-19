using UnityEngine;

public class FlameSlashSkill : ActiveSkill
{
    public override void Activate(Vector3 targetPosition)
    {
        if (!IsReadyToActivate() || !ConsumeMana()) return;

        Debug.Log("불꽃베기 발동! 범위 내의 적에게 1~2회 공격.");
        SkillEffectManager.Instance.TriggerEffect(this, targetPosition);

        ResetCondition();
    }

    protected override void EnhanceSkill()
    {
        skillData.damagePercent += 10f;
        Debug.Log("FlameSlashSkill의 damagePercent가 강화되었습니다. 새로운 값: " + skillData.damagePercent);
    }
}