using UnityEngine;

public abstract class BaseItemSO : ScriptableObject
{
    [Header("Base Info")]
    public string itemName; // 아이템 이름
    public Sprite icon; // 아이템 아이콘
    public Sprite inGameImage; // 인게임 이미지
    public Grade grade; // 등급
    public int currentSkillLevel; // 현재 강화 레벨
    public int curStackAmount; // 현재 스택 수량
}