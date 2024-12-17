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
                isActivated = true; // �ߵ� ���·� ����
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (isActivated) return; // �ߺ� �ߵ� ����

        Debug.Log("�нú� ��ų �ߵ�!");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        isActivated = true; // ��ų �ߵ� ���¸� true�� ����
        ResetCondition();
    }
}