using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    //장비에서 WeaponDataSO->weapon.cs에서 받아오고-> PlayerDamageCaculator한테 전달.

    public void Equip();

    public void Upgrade();

    public void UnEquip();

    public void Fusion();

    public void AddStackAmount(int count);

    public void SubtractStackAmount(int count);
    
}
