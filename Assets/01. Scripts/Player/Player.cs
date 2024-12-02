using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    PlayerStat stat;
    PlayerInventory inventory;
    PlayerBattle PlayerBattle;

    private void Awake()
    {
        PlayerobjManager.Instance.Player = this;
        stat = GetComponent<PlayerStat>();
        inventory = GetComponent<PlayerInventory>();
        PlayerBattle = GetComponent<PlayerBattle>();
    }
}
