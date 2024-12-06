using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemys/Enemy")]
public class EnemySO : ScriptableObject
{

    [Header("Enemy information")]
    public string enemyCode; // 적식별코드
    public float hp; // 체력
    public float maxHp; // 최대체력
    public int giveExp; // 주는 경험치
    public Sprite sprite; //적 이미지

    [Header("Skill Properties")]
    public float cooldown; // 쿨다운 시간 (보스가 스킬을 지니고 있을시 사용)

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // 스킬 효과 프리팹
    public Collider2D effectRange; // 스킬 효과 범위
    public float damagePercent; // 액티브 스킬: 범위 내 적에게 주는 공격력 비율 (%)

    
}