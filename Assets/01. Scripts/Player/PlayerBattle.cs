using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattle : MonoBehaviour, IDamageable
{
    private enum State
    {
        Idle,
        Attacking,
        Dead
    }

    private State currentState;
    private PlayerDamageCalculator PlayerDamageCalculator;
    private PlayerStat playerStat;

    private float totalDamage; // 계산기에서 받아온 최종데미지
    private float attackSpeed; // 공격 속도 퍼센트 게이지로 만들어 딜레이 에서 빼줄예정
    private float attackDelay = 1f; // 공격 딜레이
    public List<Skill> activeBuffSkills = new List<Skill>(); // 현재 활성화된 버프 스킬 리스트

    private Enemy currentMonster; // 현재 공격 중인 몬스터

    private void Start()
    {
        PlayerDamageCalculator = GetComponent<PlayerDamageCalculator>();
        playerStat = GetComponent<PlayerStat>();
        currentState = State.Idle;
    }

    private void Update()
    {
        switch (currentState)
        {
            case State.Idle:
                CancelInvoke("PlayerAttack");
                BattleManager.Instance.EndBattle();
                break;
            case State.Attacking:
                if (currentMonster != null && !IsInvoking("PlayerAttack"))
                {
                    InvokeRepeating("PlayerAttack", 0f, attackDelay);
                    BattleManager.Instance.StartBattle();
                }
                break;
            case State.Dead:
                BattleManager.Instance.StartBattle();
                break;
        }
    }


    public void TakeDamage(float Damage)
    {

    }

    private void PlayerAttack()
    {
        if (currentMonster != null && currentMonster.GetActive())
        {
            totalDamage = PlayerDamageCalculator.GetTotalDamage();
            // 공격 애니메이션 재생예정
            // animator.SetTrigger("Attack");
            currentMonster.TakeDamage(totalDamage);
        }
        else
        {
            Debug.Log("몬스터 잡음");
            // 몬스터가 사망하면 Idle 상태로 전환
            currentState = State.Idle;

            // 몬스터의 giveexp 값만큼 플레이어가 exp 획득
            playerStat.AddExpFromMonsters(currentMonster);

            // 플레이어 레벨 ui 업데이트
            PlayerLevelInfoUI.Instance.UpdateLevelUI();

            currentMonster = null;
        }
    }

    public void UseBuffSkill(Skill skill)
    {
        // 버프 정보를 저장
        activeBuffSkills.Add(skill);
        StartCoroutine(BuffCoroutine(skill, skill.BaseData.buffDuration));
    }

    private IEnumerator BuffCoroutine(Skill skill, float skillTime)
    {
        // 지정된 시간 동안 대기
        yield return new WaitForSeconds(skillTime);

        // 버프 해제
        activeBuffSkills.Remove(skill);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            currentMonster = collision.gameObject.GetComponent<Enemy>();
            currentState = State.Attacking;
        }
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Monster"))
    //    {
    //        currentState = State.Idle;
    //    }
    //}

}
