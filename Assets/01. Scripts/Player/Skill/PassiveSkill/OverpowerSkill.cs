using UnityEngine;

public class OverpowerSkill : PassiveSkill
{
    private bool isActivated = false;
    public OverpowerSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Update()
    {
        base.Update();

        // 전투 시작 후 20초 뒤에 발동
        if (!isActivated && Time.timeSinceLevelLoad >= 20f)
        {
            Activate(Vector3.zero);
            isActivated = true;
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("괴력난신 발동! 5초간 전체 공격력 증가.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}