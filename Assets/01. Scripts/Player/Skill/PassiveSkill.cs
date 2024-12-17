using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private float periodicTimer = 0f;

    public PassiveSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Update()
    {
        base.Update();

        if (skillData.activationCondition == ActivationCondition.Periodic)
        {
            periodicTimer += Time.deltaTime;

            if (periodicTimer >= skillData.periodicInterval)
            {
                Activate(Vector3.zero);
                periodicTimer = 0f;
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);
        ResetCondition();
    }
}