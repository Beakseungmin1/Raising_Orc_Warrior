using UnityEngine;

public class EquipManager : MonoBehaviour
{
    private PlayerStat playerStat;

    //private void Start()
    //{
    //    var player = PlayerManager.Instance.player;
    //    if (player == null)
    //    {
    //        return;
    //    }

    //    playerStat = player.GetComponent<PlayerStat>();
    //}

    // ���� ������ ������
    public WeaponDataSO EquippedWeapon { get; private set; }
    public AccessoryDataSO EquippedAccessory { get; private set; }

    public void EquipWeapon(WeaponDataSO weapon)
    {
        if (weapon == null)
        {
            return;
        }

        // ���� ���� ȿ�� ����
        if (EquippedWeapon != null)
        {
            //playerStat.RemoveWeaponEffect(EquippedWeapon);
        }

        // ���ο� ���� ����
        EquippedWeapon = weapon;
        //playerStat.ApplyWeaponEffect(weapon);
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null)
        {
            return;
        }

        // ���� ���� ȿ�� ����
        //playerStat.RemoveWeaponEffect(EquippedWeapon);

        EquippedWeapon = null;
    }

    public void EquipAccessory(AccessoryDataSO accessory)
    {
        if (accessory == null)
        {
            return;
        }

        // ���� ������ �Ǽ��縮 ȿ�� ����
        if (EquippedAccessory != null)
        {
            //playerStat.RemoveAccessoryEffect(EquippedAccessory);
        }

        // ���ο� �Ǽ��縮 ����
        EquippedAccessory = accessory;
        //playerStat.ApplyAccessoryEffect(accessory);
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null)
        {
            Debug.Log("���� ������ �Ǽ��縮�� �����ϴ�.");
            return;
        }

        // ���� �Ǽ��縮 ȿ�� ����
        //playerStat.RemoveAccessoryEffect(EquippedAccessory);

        EquippedAccessory = null;
    }
}