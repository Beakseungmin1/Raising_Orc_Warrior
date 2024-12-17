using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private float periodicTimer = 0f; // �ߵ� �ֱ� Ÿ�̸�

    public PassiveSkill() { }

    public override void Initialize(SkillDataSO data, PlayerStat stat)
    {
        base.Initialize(data, stat); // BaseSkill�� Initialize ȣ��
    }

    private void OnBattleStart()
    {
        periodicTimer = 0f; // ���� ���� �� Ÿ�̸� �ʱ�ȭ
        isActivated = false; // �ߵ� ���� �ʱ�ȭ
    }

    private void OnBattleEnd()
    {
        isActivated = false; // ���� ���� �� ���� �ʱ�ȭ
    }

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && IsSkillEquipped()) // ���� ���̰� ��ų�� ������ ��쿡�� �ߵ�
        {
            periodicTimer += Time.deltaTime;

            // �ֱ������� �ߵ��� ��
            if (periodicTimer >= skillData.periodicInterval && !isActivated)
            {
                Activate(Vector3.zero);
                isActivated = true; // �ߵ� ���·� ����
                periodicTimer = 0f; // Ÿ�̸� ����
            }
            // ���� �ð��� ���� �� �ߵ� ���� �ʱ�ȭ
            else if (periodicTimer >= skillData.periodicInterval * 2) // ��: 10�� ��
            {
                isActivated = false; // ���� �ʱ�ȭ
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (isActivated) return; // �̹� �ߵ��Ǿ����� ����

        Debug.Log("�нú� ��ų �ߵ�!");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        isActivated = true; // ��ų �ߵ� ���� ����
        ResetCondition();
    }

    private bool IsSkillEquipped()
    {
        // ������ ���¿����� �ߵ� (PlayerSkillHandler�� ���� Ȯ��)
        return PlayerObjManager.Instance?.Player?.GetComponent<PlayerSkillHandler>()?.IsSkillEquipped(this) ?? false;
    }
}