using UnityEngine;

public class OverpowerSkill : PassiveSkill
{
    private void OnEnable()
    {
        BattleManager.Instance.OnBattleStart += OnBattleStart;
        BattleManager.Instance.OnBattleEnd += OnBattleEnd;
    }

    private void OnDisable()
    {
        BattleManager.Instance.OnBattleStart -= OnBattleStart;
        BattleManager.Instance.OnBattleEnd -= OnBattleEnd;
    }

    private void OnBattleStart()
    {
        isActivated = false; // ���� ���� �� ���� �ʱ�ȭ
    }

    private void OnBattleEnd()
    {
        isActivated = false; // ���� ���� �� ���� �ʱ�ȭ
    }

    public override void Update()
    {
        if (!isActivated && BattleManager.Instance.IsBattleActive && Time.timeSinceLevelLoad >= 20f)
        {
            Activate(Vector3.zero);
            isActivated = true; // �ߵ� ���·� ����
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("���³��� �ߵ�! 5�ʰ� ��ü ���ݷ� ����.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}