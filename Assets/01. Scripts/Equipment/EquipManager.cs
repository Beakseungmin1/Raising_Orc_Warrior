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

    // 현재 장착된 아이템
    public WeaponDataSO EquippedWeapon { get; private set; }
    public AccessoryDataSO EquippedAccessory { get; private set; }

    public void EquipWeapon(WeaponDataSO weapon)
    {
        if (weapon == null)
        {
            return;
        }

        // 기존 무기 효과 제거
        if (EquippedWeapon != null)
        {
            //playerStat.RemoveWeaponEffect(EquippedWeapon);
        }

        // 새로운 무기 장착
        EquippedWeapon = weapon;
        //playerStat.ApplyWeaponEffect(weapon);
    }

    public void UnequipWeapon()
    {
        if (EquippedWeapon == null)
        {
            return;
        }

        // 기존 무기 효과 제거
        //playerStat.RemoveWeaponEffect(EquippedWeapon);

        EquippedWeapon = null;
    }

    public void EquipAccessory(AccessoryDataSO accessory)
    {
        if (accessory == null)
        {
            return;
        }

        // 기존 장착된 악세사리 효과 제거
        if (EquippedAccessory != null)
        {
            //playerStat.RemoveAccessoryEffect(EquippedAccessory);
        }

        // 새로운 악세사리 장착
        EquippedAccessory = accessory;
        //playerStat.ApplyAccessoryEffect(accessory);
    }

    public void UnequipAccessory()
    {
        if (EquippedAccessory == null)
        {
            Debug.Log("현재 장착된 악세사리가 없습니다.");
            return;
        }

        // 기존 악세사리 효과 제거
        //playerStat.RemoveAccessoryEffect(EquippedAccessory);

        EquippedAccessory = null;
    }
}