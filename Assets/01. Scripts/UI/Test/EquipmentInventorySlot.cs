using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt; // 강화 레벨 텍스트
    [SerializeField] private TextMeshProUGUI rankTxt; // 랭크 텍스트
    [SerializeField] private Slider expGauge; // 경험치 게이지
    [SerializeField] private TextMeshProUGUI requiredItemCountTxt; // 필요한 아이템 수량 텍스트
    [SerializeField] private TextMeshProUGUI currentItemCountTxt; // 현재 아이템 수량 텍스트
    [SerializeField] private Button slotButton;

    private IEnhanceable item;
    private bool isWeaponSlot;

    private Color defaultColor = Color.white;
    private Color emptyColor = new Color32(80, 80, 80, 255);

    public IEnhanceable Item => item;
    public bool IsWeaponSlot => isWeaponSlot;

    public void AssignItem(IEnhanceable newItem, bool isWeapon)
    {
        item = newItem;
        isWeaponSlot = isWeapon;

        if (item != null)
        {
            equipmentIcon.sprite = item.BaseData.icon;
            rankTxt.text = item.BaseData is WeaponDataSO weapon ? weapon.rank.ToString() :
                           item.BaseData is AccessoryDataSO accessory ? accessory.rank.ToString() : string.Empty;

            UpdateSlotState(0); // 초기화 시 스택 0 상태로 설정
        }
        else
        {
            ClearSlot();
        }
    }

    public void ClearSlot()
    {
        item = null;
        equipmentIcon.sprite = null;
        rankTxt.text = string.Empty;
        currentLevelTxt.text = string.Empty;
        requiredItemCountTxt.text = "0";
        currentItemCountTxt.text = "0";
        expGauge.value = 0;

        UpdateSlotColor(false);
        slotButton.onClick.RemoveAllListeners();
    }

    public void UpdateSlotState(int stackCount)
    {
        // 스택 수량 및 강화 레벨 업데이트
        currentItemCountTxt.text = stackCount.ToString();
        currentLevelTxt.text = $"+{item.EnhancementLevel}";
        requiredItemCountTxt.text = "5"; // 필요 수량(예시로 설정)
        expGauge.value = Mathf.Clamp01((float)stackCount / 5f); // 필요 수량 기준으로 게이지 설정
        UpdateSlotColor(stackCount > 0);

        slotButton.onClick.RemoveAllListeners();
        if (stackCount > 0)
        {
            slotButton.onClick.AddListener(() => OpenItemDetails());
        }
    }

    private void OpenItemDetails()
    {
        if (item == null) return;

        // 팝업 표시 로직
        UIManager.Instance.Show<DimmedUI>();
        var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
        upgradePopup.SetEquipmentData(item, isWeaponSlot);
    }

    private void UpdateSlotColor(bool hasItem)
    {
        equipmentIcon.color = hasItem ? defaultColor : emptyColor;
    }
}