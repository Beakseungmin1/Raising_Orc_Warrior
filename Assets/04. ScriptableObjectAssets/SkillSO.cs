using UnityEngine;

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Skill")]
public class SkillSO : ScriptableObject
{
    [Header("Skill Details")]
    public string skillName; // 스킬 이름
    public string description; // 스킬 설명

    [Header("Skill Properties")]
    public float cooldown; // 쿨다운 시간
    public int manaCost; // 마나 소모량

    [Header("Skill Effects")]
    public GameObject skillEffectPrefab; // 스킬 효과 프리팹
    public float damage; // 스킬 데미지
    public float range; // 스킬 범위
}