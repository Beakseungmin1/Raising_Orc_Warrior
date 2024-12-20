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
        isActivated = false; // 전투 시작 시 상태 초기화
    }

    private void OnBattleEnd()
    {
        isActivated = false; // 전투 종료 시 상태 초기화
    }

    public override void Update()
    {
        if (!isActivated && BattleManager.Instance.IsBattleActive && Time.timeSinceLevelLoad >= 20f)
        {
            Activate(Vector3.zero);
            isActivated = true; // 발동 상태로 설정
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        Debug.Log("괴력난신 발동! 5초간 전체 공격력 증가.");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        ResetCondition();
    }
}