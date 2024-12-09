using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponFusionPopupUI : UIBase
{
    [Header("Current Weapon Info")]
    [SerializeField] private Image currentWeaponIcon;
    [SerializeField] private TextMeshProUGUI currentWeaponNameTxt;
    [SerializeField] private TextMeshProUGUI currentWeaponAmountTxt;
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    [Header("Fusion Result Weapon Info")]
    [SerializeField] private Image resultWeaponIcon;
    [SerializeField] private TextMeshProUGUI resultWeaponNameTxt;
    [SerializeField] private Button fusionBtn;

    [Header("Material Count Area")]
    [SerializeField] private TextMeshProUGUI materialCountTxt;
    [SerializeField] private Button subtractBtn;
    [SerializeField] private Button addBtn;

    [Header("Exit and Currency Info")]
    [SerializeField] private Button exitBtn;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;
    [SerializeField] private Image curCubeIcon;

    private Weapon currentWeapon;
    private int materialCount;
    private int maxMaterials;

    private void Start()
    {
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(PerformFusion);
        subtractBtn.onClick.AddListener(DecreaseMaterialCount);
        addBtn.onClick.AddListener(IncreaseMaterialCount);
    }

    public void SetWeaponData(Weapon weapon)
    {
        if (weapon == null || weapon.BaseData == null)
        {
            Debug.LogError("[WeaponFusionPopupUI] Weapon �����Ͱ� ��ȿ���� �ʽ��ϴ�.");
            return;
        }

        currentWeapon = weapon;
        materialCount = 0;
        maxMaterials = weapon.StackCount;

        WeaponDataSO weaponData = weapon.BaseData;

        // Current Weapon Info
        currentWeaponIcon.sprite = weaponData.icon;
        currentWeaponNameTxt.text = weaponData.itemName;
        currentWeaponAmountTxt.text = $"x{weapon.StackCount}";

        // Result Weapon Info
        resultWeaponIcon.sprite = weaponData.icon; // �⺻������ ���� ������ ���
        resultWeaponNameTxt.text = "���� ���";

        // Material Count
        materialCountTxt.text = materialCount.ToString();

        // Currency Info
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = weaponData.currencyIcon;
    }

    private void PerformFusion()
    {
        if (currentWeapon == null || materialCount <= 0 || materialCount > maxMaterials)
        {
            Debug.LogWarning("[WeaponFusionPopupUI] ���� ������ �������� �ʾҽ��ϴ�.");
            return;
        }

        // ǻ�� ���� ó��
        Debug.Log($"[WeaponFusionPopupUI] {materialCount}���� ���⸦ ����Ͽ� ������ �õ��մϴ�.");

        // UI ������Ʈ
        SetWeaponData(currentWeapon);
    }

    private void IncreaseMaterialCount()
    {
        if (materialCount < maxMaterials)
        {
            materialCount++;
            materialCountTxt.text = materialCount.ToString();
        }
    }

    private void DecreaseMaterialCount()
    {
        if (materialCount > 0)
        {
            materialCount--;
            materialCountTxt.text = materialCount.ToString();
        }
    }

    private void ClosePopup()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    public void ShowUpgradePopup()
    {
        Hide();
        var upgradePopup = UIManager.Instance.Show<WeaponUpgradePopupUI>();
        upgradePopup.SetWeaponData(currentWeapon);
    }
}