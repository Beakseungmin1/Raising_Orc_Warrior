using UnityEngine;

public abstract class BaseItemDataSO : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName;
    public string englishItemName;
    public Sprite icon;
    public Sprite inGameImage;
    public Grade grade;
    public Color gradeColor;

    [Header("Upgrade Information")]
    public Sprite currencyIcon;
    public int requiredCurrencyForUpgrade;
    public int currentLevel;
}