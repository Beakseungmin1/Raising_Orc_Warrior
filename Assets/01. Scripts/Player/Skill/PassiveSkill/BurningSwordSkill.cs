using UnityEngine;

public class BurningSwordSkill : PassiveSkill
{
    private float lastActivationTime = 0f;
    private float periodicInterval = 5f; // 5초 주기

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
        lastActivationTime = Time.time;
    }

    private void OnBattleEnd()
    {
        lastActivationTime = 0f;
    }

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && Time.time - lastActivationTime >= periodicInterval)
        {
            Activate(Vector3.zero);
            lastActivationTime = Time.time;
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("타오르는 검 발동! 전체 공격력 증가 효과 적용.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}