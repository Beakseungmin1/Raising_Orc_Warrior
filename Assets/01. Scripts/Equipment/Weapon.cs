using UnityEngine;

public class WeaponObject : MonoBehaviour
{
    [Header("Weapon Data")]
    public WeaponDataSO weaponData;
    public Sprite inGameImage;
    public float enhancedAttackPower;
    public int upgradeLevel = 0;
    public int stackAmount = 0;

    public void InitializeWeapon(WeaponDataSO newWeaponData, float enhancedAtkPower, int upgradedLevel)
    {
        weaponData = newWeaponData;
        inGameImage = newWeaponData.inGameImage;
        enhancedAttackPower = enhancedAtkPower;
        upgradeLevel = upgradedLevel;
        UpdateWeaponVisuals();
    }

    public void UpdateEnhancedData(float newAttackPower, int newUpgradeLevel)
    {
        enhancedAttackPower = newAttackPower;
        upgradeLevel = newUpgradeLevel;
        UpdateWeaponVisuals();
    }

    public void AddStack(int additionalStack)
    {
        stackAmount += additionalStack;
    }

    private void UpdateWeaponVisuals()
    {
        //UIManager.Instance.UpdateWeaponUI(weaponData.itemName, inGameImage, enhancedAttackPower, upgradeLevel, stackAmount);
    }

    public void PrintWeaponInfo()
    {
        Debug.Log($"--- 무기 정보 ---\n" +
                  $"이름: {weaponData.itemName}\n" +
                  $"등급: {weaponData.rank}\n" +
                  $"기본 공격력 증가율: {weaponData.equipAtkIncreaseRate}\n" +
                  $"강화 공격력: {enhancedAttackPower}\n" +
                  $"강화 레벨: {upgradeLevel}\n" +
                  $"스택: {stackAmount}\n");
    }
}