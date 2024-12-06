using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Slider expGauge;
    [SerializeField] private TextMeshProUGUI maxAmountTxt;
    [SerializeField] private TextMeshProUGUI curAmountTxt;
    [SerializeField] private Button slotButton;

    private Weapon weaponData;
    private EquipmentInventorySlotManager slotManager;

    private Color defaultColor = Color.white; // 기본 색상 (장착된 경우)
    private Color emptyColor = new Color32(80, 80, 80, 255); // 비어 있을 때의 색상

    public void InitializeSlot(Weapon weapon, int currentAmount, int requiredAmount, EquipmentInventorySlotManager manager)
    {
        weaponData = weapon;
        slotManager = manager;

        if (weaponData != null && weaponData.BaseData != null)
        {
            WeaponDataSO weaponBaseData = weaponData.BaseData;

            weaponIcon.sprite = weaponBaseData.icon;
            currentLevelTxt.text = $"+{weaponBaseData.currentLevel}";
            rankTxt.text = $"{weaponBaseData.rank}등급";
            curAmountTxt.text = currentAmount.ToString();
            maxAmountTxt.text = requiredAmount.ToString();
            expGauge.value = (float)currentAmount / requiredAmount;

            UpdateSlotColor(true);

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(OpenUpgradePopup);
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        weaponData = null;
        weaponIcon.sprite = null;
        currentLevelTxt.text = string.Empty;
        rankTxt.text = string.Empty;
        curAmountTxt.text = "0";
        maxAmountTxt.text = "0";
        expGauge.value = 0;

        UpdateSlotColor(false);

        slotButton.onClick.RemoveAllListeners();
    }

    private void OpenUpgradePopup()
    {
        if (weaponData != null)
        {
            UIManager.Instance.Show<DimmedUI>();
            var upgradePopup = UIManager.Instance.Show<WeaponUpgradePopupUI>();
            upgradePopup.SetWeaponData(weaponData);
        }
    }

    private void UpdateSlotColor(bool hasWeapon)
    {
        weaponIcon.color = hasWeapon ? defaultColor : emptyColor;
    }
}