using System.Numerics;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public PlayerInformation playerInformation;
    public PlayerStat stat;
    public PlayerInventory inventory;
    public PlayerBattle PlayerBattle;
    public EquipManager EquipManager;
    public PlayerSkillHandler SkillHandler;
    public PlayerDamageCalculator DamageCalculator;
    public PlayerStatCalculator StatCalculator;

    [Header("UI Elements")]
    public Image hpBar;
    public Image manaBar;

    private void Awake()
    {
        playerInformation = GetComponent<PlayerInformation>();
        stat = GetComponent<PlayerStat>();
        inventory = GetComponent<PlayerInventory>();
        PlayerBattle = GetComponent<PlayerBattle>();
        EquipManager = GetComponent<EquipManager>();
        SkillHandler = GetComponent<PlayerSkillHandler>();
        DamageCalculator = GetComponent<PlayerDamageCalculator>();
        StatCalculator = GetComponent<PlayerStatCalculator>();
        PlayerObjManager.Instance.Player = this;
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    public void UpdateHealthBar()
    {
        BigInteger adjustedMaxHealth = StatCalculator.GetAdjustedMaxHealth();

        float fillAmount = (float)stat.health / (float)adjustedMaxHealth;
        hpBar.fillAmount = fillAmount;
    }

    public void UpdateManaBar()
    {
        manaBar.fillAmount = (float)stat.mana / stat.maxMana;
    }
}
