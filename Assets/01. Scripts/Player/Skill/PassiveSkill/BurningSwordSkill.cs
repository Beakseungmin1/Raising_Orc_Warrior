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
        lastActivationTime = Time.time; // 전투 시작 시 타이머 초기화
    }

    private void OnBattleEnd()
    {
        lastActivationTime = 0f; // 전투 종료 시 타이머 리셋
    }

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && Time.time - lastActivationTime >= periodicInterval)
        {
            Activate(Vector3.zero);
            lastActivationTime = Time.time; // 발동 후 타이머 갱신
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("타오르는 검 발동! 전체 공격력 증가 효과 적용.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}