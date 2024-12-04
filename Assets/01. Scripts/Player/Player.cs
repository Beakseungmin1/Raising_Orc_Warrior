using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerStat stat;
    public PlayerInventory inventory;
    public PlayerBattle PlayerBattle;

    public Weapon curWeapon;




    private void Awake()
    {
        PlayerobjManager.Instance.Player = this;
        stat = GetComponent<PlayerStat>();
        inventory = GetComponent<PlayerInventory>();
        PlayerBattle = GetComponent<PlayerBattle>();
    }
}
