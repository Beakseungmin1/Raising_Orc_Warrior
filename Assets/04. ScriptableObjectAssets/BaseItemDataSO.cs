using UnityEngine;

public abstract class BaseItemDataSO : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName; // 아이템 이름
    public Sprite icon; // 아이템 아이콘
    public Sprite inGameImage; // 인게임 이미지
    public Grade grade; // 등급
    public Color gradeColor; // 등급별 색상

    [Header("Upgrade Information")]
    public Sprite currencyIcon; // 강화에 필요한 재화 아이콘
    public int requiredCurrencyForUpgrade; // 강화에 필요한 재화 수량
    public int currentLevel; // 현재 레벨
}