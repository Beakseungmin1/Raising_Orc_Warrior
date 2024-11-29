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
        Debug.Log($"--- ���� ���� ---\n" +
                  $"�̸�: {weaponData.itemName}\n" +
                  $"���: {weaponData.rank}\n" +
                  $"�⺻ ���ݷ� ������: {weaponData.equipAtkIncreaseRate}\n" +
                  $"��ȭ ���ݷ�: {enhancedAttackPower}\n" +
                  $"��ȭ ����: {upgradeLevel}\n" +
                  $"����: {stackAmount}\n");
    }
}