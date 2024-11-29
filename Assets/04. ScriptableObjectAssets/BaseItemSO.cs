using UnityEngine;

public abstract class BaseItemSO : ScriptableObject
{
    //무기, 악세, 스킬

    [Header("Base Info")]
    public string itemName; // 아이템 이름
    public Sprite icon; // 아이템 아이콘
    public Sprite inGameImage; // 인게임 이미지
    public Grade grade; // 등급

}