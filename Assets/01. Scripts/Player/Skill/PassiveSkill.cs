using UnityEngine;

public class PassiveSkill : BaseSkill
{
    private float periodicTimer = 0f; // 발동 주기 타이머

    public PassiveSkill() { }

    public override void Initialize(SkillDataSO data, PlayerStat stat)
    {
        base.Initialize(data, stat); // BaseSkill의 Initialize 호출
    }

    private void OnBattleStart()
    {
        periodicTimer = 0f; // 전투 시작 시 타이머 초기화
        isActivated = false; // 발동 상태 초기화
    }

    private void OnBattleEnd()
    {
        isActivated = false; // 전투 종료 시 상태 초기화
    }

    public override void Update()
    {
        base.Update();

        if (BattleManager.Instance.IsBattleActive && IsSkillEquipped()) // 전투 중이고 스킬이 장착된 경우에만 발동
        {
            periodicTimer += Time.deltaTime;

            // 주기적으로 발동할 때
            if (periodicTimer >= skillData.periodicInterval && !isActivated)
            {
                Activate(Vector3.zero);
                isActivated = true; // 발동 상태로 설정
                periodicTimer = 0f; // 타이머 리셋
            }
            // 일정 시간이 지난 후 발동 상태 초기화
            else if (periodicTimer >= skillData.periodicInterval * 2) // 예: 10초 후
            {
                isActivated = false; // 상태 초기화
            }
        }
    }

    public override void Activate(Vector3 targetPosition)
    {
        if (isActivated) return; // 이미 발동되었으면 리턴

        Debug.Log("패시브 스킬 발동!");
        SkillEffectManager.Instance.TriggerEffect(this, Vector3.zero);

        isActivated = true; // 스킬 발동 상태 설정
        ResetCondition();
    }

    private bool IsSkillEquipped()
    {
        // 장착된 상태에서만 발동 (PlayerSkillHandler를 통해 확인)
        return PlayerObjManager.Instance?.Player?.GetComponent<PlayerSkillHandler>()?.IsSkillEquipped(this) ?? false;
    }
}