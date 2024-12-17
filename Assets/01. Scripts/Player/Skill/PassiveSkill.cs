using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private float periodicTimer = 0f;

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && !isActivated)
        {
            periodicTimer += Time.deltaTime;

            if (periodicTimer >= skillData.periodicInterval)
            {
                Activate(Vector3.zero);
                isActivated = true; // 발동 상태로 설정
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (isActivated) return; // 중복 발동 방지

        Debug.Log("패시브 스킬 발동!");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        isActivated = true; // 스킬 발동 상태를 true로 설정
        ResetCondition();
    }
}