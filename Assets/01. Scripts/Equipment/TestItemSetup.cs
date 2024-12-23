using UnityEngine;

public class TestItemSetup : MonoBehaviour
{
    public SkillDataSO skill1;
    public SkillDataSO skill2;
    public SkillDataSO skill3;
    public WeaponDataSO weapon1;
    public WeaponDataSO weapon2;
    public WeaponDataSO weapon3;
    public WeaponDataSO weapon4;
    public WeaponDataSO weapon5;
    public AccessoryDataSO accessory1;
    public AccessoryDataSO accessory2;
    public AccessoryDataSO accessory3;
    public AccessoryDataSO accessory4;
    public AccessoryDataSO accessory5;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory�� ã�� �� �����ϴ�.");
            return;
        }

        AddTestItems();
    }

    private void AddTestItems()
    {
        // ��ų �߰�
        playerInventory.AddItemToInventory(skill1);
        playerInventory.AddItemToInventory(skill2);
        playerInventory.AddItemToInventory(skill3);

        // ���� �߰�
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon1);
        playerInventory.AddItemToInventory(weapon2);
        playerInventory.AddItemToInventory(weapon3);
        playerInventory.AddItemToInventory(weapon4);
        playerInventory.AddItemToInventory(weapon5);
        playerInventory.AddItemToInventory(weapon5);
        playerInventory.AddItemToInventory(weapon5);
        playerInventory.AddItemToInventory(weapon5);
        playerInventory.AddItemToInventory(weapon5);

        // �Ǽ��縮 �߰�
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory1);
        playerInventory.AddItemToInventory(accessory2);
        playerInventory.AddItemToInventory(accessory3);
        playerInventory.AddItemToInventory(accessory4);
        playerInventory.AddItemToInventory(accessory5);
        playerInventory.AddItemToInventory(accessory5);
        playerInventory.AddItemToInventory(accessory5);
        playerInventory.AddItemToInventory(accessory5);
        playerInventory.AddItemToInventory(accessory5);
        playerInventory.AddItemToInventory(accessory5);

        Debug.Log("�׽�Ʈ �������� �κ��丮�� �߰��Ǿ����ϴ�.");
    }
}