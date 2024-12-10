using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Slider expGauge;
    [SerializeField] private TextMeshProUGUI requiredItemCountTxt;
    [SerializeField] private TextMeshProUGUI currentItemCountTxt;
    [SerializeField] private Button slotButton;

    private IEnhanceable equipmentData;
    private EquipmentInventorySlotManager slotManager;
    private bool isWeapon;

    private int requiredItemCount = 1; // 기본 필요 수량

    private Color defaultColor = Color.white;
    private Color emptyColor = new Color32(80, 80, 80, 255);

    public void InitializeSlot(IEnhanceable equipment, int currentItemCount, int requiredItemCount, EquipmentInventorySlotManager manager, bool isWeaponType)
    {
        equipmentData = equipment;
        slotManager = manager;
        isWeapon = isWeaponType;

        if (equipmentData != null)
        {
            equipmentIcon.sprite = equipment.BaseData.icon;

            // 현재 보유 수량 표시
            currentItemCountTxt.text = currentItemCount.ToString();

            // 필요 수량 설정
            requiredItemCountTxt.text = requiredItemCount.ToString();

            // 강화 레벨 표시
            currentLevelTxt.text = $"+{equipment.EnhancementLevel}";

            // 랭크 표시 (무기 또는 악세서리만 해당)
            if (isWeaponType || equipment is Accessory)
            {
                rankTxt.text = (equipment.BaseData as WeaponDataSO)?.rank.ToString() ??
                               (equipment.BaseData as AccessoryDataSO)?.rank.ToString();
            }
            else
            {
                rankTxt.text = string.Empty; // 스킬에는 rank 없음
            }

            // 경험치 게이지 업데이트
            expGauge.value = Mathf.Clamp01((float)currentItemCount / requiredItemCount);

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
        currentItemCountTxt.text = "0";
        requiredItemCountTxt.text = "0";
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