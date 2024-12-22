using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public PlayerInformation playerInformation;
    public PlayerStat stat;
    public PlayerInventory inventory;
    public PlayerBattle PlayerBattle;
    public EquipManager EquipManager;
    public PlayerSkillHandler SkillHandler;
    public PlayerDamageCalculator DamageCalculator;


    private void Awake()
    {
        playerInformation = GetComponent<PlayerInformation>();
        stat = GetComponent<PlayerStat>();
        inventory = GetComponent<PlayerInventory>();
        PlayerBattle = GetComponent<PlayerBattle>();
        EquipManager = GetComponent<EquipManager>();
        SkillHandler = GetComponent<PlayerSkillHandler>();
        DamageCalculator = GetComponent<PlayerDamageCalculator>();
        PlayerObjManager.Instance.Player = this;
    }
}
