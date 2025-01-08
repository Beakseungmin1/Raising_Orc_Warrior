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
    public Animator animator;

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

    private void Start()
    {
        Transform playerModelTransform = transform.GetChild(1);
        Transform unitRootTransform = playerModelTransform.GetChild(0);
        animator = unitRootTransform.GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }

    public void ChangeAnimatorSpeed(float value)
    {
        animator.speed = value;
    }

    public void UpdateHealthBar()
    {
        BigInteger adjustedMaxHealth = StatCalculator.GetAdjustedMaxHealth();

        float fillAmount = (float)stat.health / (float)adjustedMaxHealth;
        hpBar.fillAmount = fillAmount;
    }

    public void UpdateManaBar()
    {
        manaBar.fillAmount = stat.mana / StatCalculator.GetAdjustedMaxMana();
    }
}
