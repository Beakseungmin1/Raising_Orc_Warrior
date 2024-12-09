using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Slider expGauge;
    [SerializeField] private TextMeshProUGUI maxAmountTxt;
    [SerializeField] private TextMeshProUGUI curAmountTxt;
    [SerializeField] private Button slotButton;

    private IEnhanceable equipmentData;
    private EquipmentInventorySlotManager slotManager;
    private bool isWeapon;

    private Color defaultColor = Color.white;
    private Color emptyColor = new Color32(80, 80, 80, 255);

    public void InitializeSlot(IEnhanceable equipment, int currentAmount, int requiredAmount, EquipmentInventorySlotManager manager, bool isWeaponType)
    {
        equipmentData = equipment;
        slotManager = manager;
        isWeapon = isWeaponType;

        if (equipmentData != null)
        {
            equipmentIcon.sprite = equipment.BaseData.icon;
            curAmountTxt.text = equipment.StackCount.ToString();
            maxAmountTxt.text = "1"; // 무기와 악세사리는 항상 1개 필요
            currentLevelTxt.text = $"+{equipment.EnhancementLevel}";
            expGauge.value = Mathf.Clamp01((float)currentAmount / 1);

            UpdateSlotColor(true);

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(() => OpenUpgradePopup());
        }
        else
        {
            ClearSlot();
        }
    }

    private void ClearSlot()
    {
        equipmentData = null;
        equipmentIcon.sprite = null;
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
        if (equipmentData != null)
        {
            UIManager.Instance.Show<DimmedUI>();
            var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
            upgradePopup.SetEquipmentData(equipmentData, isWeapon);
        }
    }

    private void UpdateSlotColor(bool hasEquipment)
    {
        equipmentIcon.color = hasEquipment ? defaultColor : emptyColor;
    }
}