using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private TextMeshProUGUI equipTxt;
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt;
    [SerializeField] private TextMeshProUGUI rankTxt;
    [SerializeField] private Slider itemCountGauge;
    [SerializeField] private TextMeshProUGUI requiredItemCountTxt;
    [SerializeField] private Button slotButton;

    private IEnhanceable item;
    private bool isWeaponSlot;

    private Color defaultColor = Color.white;
    private Color emptyColor = new Color32(50, 50, 50, 255);

    public IEnhanceable Item => item;
    public bool IsWeaponSlot => isWeaponSlot;

    public void AssignItem(IEnhanceable newItem, bool isWeapon)
    {
        item = newItem;
        isWeaponSlot = isWeapon;

        UpdateSlotState(0);
        UpdateEquipState(false);
    }

    private void OnWeaponEnhanceComplete()
    {
        UpdateSlotState(0);
    }

    public void ClearSlot()
    {
        item = null;
        equipmentIcon.sprite = null;
        rankTxt.text = string.Empty;
        currentLevelTxt.text = string.Empty;
        requiredItemCountTxt.text = "0";
        itemCountGauge.value = 0;

        UpdateSlotColor(0);
        equipTxt.gameObject.SetActive(false);
        slotButton.onClick.RemoveAllListeners();
    }

    public void UpdateSlotState(int totalItemCount)
    {
        int requiredItemCount = GetRequiredFuseItemCount();
        int displayedItemCount = Mathf.Max(0, totalItemCount - 1);

        if (item != null)
        {
            equipmentIcon.sprite = item.BaseData.icon;
            rankTxt.text = item.BaseData is WeaponDataSO weapon ? $"{weapon.rank} 등급" :
                           item.BaseData is AccessoryDataSO accessory ? $"{accessory.rank} 등급" : "N/A";
            currentLevelTxt.text = $"+{item.EnhancementLevel}";
        }
        else
        {
            equipmentIcon.sprite = null;
            rankTxt.text = "N/A";
            currentLevelTxt.text = "N/A";
        }

        requiredItemCountTxt.text = $"{displayedItemCount} / {requiredItemCount}";
        itemCountGauge.value = Mathf.Clamp01((float)displayedItemCount / requiredItemCount);

        UpdateSlotColor(totalItemCount);
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() => OpenItemDetails());
    }

    public void UpdateEquipState(bool isEquipped)
    {
        equipTxt.gameObject.SetActive(isEquipped);
    }

    private int GetRequiredFuseItemCount()
    {
        if (item != null)
        {
            if (item.BaseData is WeaponDataSO weapon)
            {
                return weapon.requireFuseItemCount;
            }
            else if (item.BaseData is AccessoryDataSO accessory)
            {
                return accessory.requireFuseItemCount;
            }
        }

        return 5;
    }

    private void OpenItemDetails()
    {
        UIManager.Instance.Show<DimmedUI>();

        var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
        if (upgradePopup != null)
        {
            upgradePopup.SetEquipmentData(item, isWeaponSlot);
        }
    }

    private void UpdateSlotColor(int totalItemCount)
    {
        if (totalItemCount == 0)
        {
            equipmentIcon.color = emptyColor;
        }
        else
        {
            equipmentIcon.color = defaultColor;
        }
    }
}