using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEquipment
{
    public void Equip();

    public void Upgrade();

    public void UnEquip();

    public void Fusion();

    public void AddStackAmount(int count);

    public void SubtractStackAmount(int count);
    
}