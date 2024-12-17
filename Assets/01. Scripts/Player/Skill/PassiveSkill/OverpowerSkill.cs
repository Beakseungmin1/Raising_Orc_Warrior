using UnityEngine;

public class OverpowerSkill : PassiveSkill
{
    private bool isActivated = false;
    public OverpowerSkill(SkillDataSO data, PlayerStat stat) : base(data, stat) { }

    public override void Update()
    {
        base.Update();

        // ���� ���� �� 20�� �ڿ� �ߵ�
        if (!isActivated && Time.timeSinceLevelLoad >= 20f)
        {
            Activate(Vector3.zero);
            isActivated = true;
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("���³��� �ߵ�! 5�ʰ� ��ü ���ݷ� ����.");
        SkillEffect effect = GetSkillEffect(Vector3.zero);

        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}