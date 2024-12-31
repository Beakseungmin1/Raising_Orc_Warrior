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
    [SerializeField] private TextMeshProUGUI currentItemCountTxt;
    [SerializeField] private Button slotButton;

    [SerializeField] private GameObject normalImage;
    [SerializeField] private GameObject uncommonImage;
    [SerializeField] private GameObject rareImage;
    [SerializeField] private GameObject heroImage;
    [SerializeField] private GameObject legendaryImage;
    [SerializeField] private GameObject mythicImage;
    [SerializeField] private GameObject ultimateImage;

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

    public void ClearSlot()
    {
        item = null;
        equipmentIcon.sprite = null;
        rankTxt.text = string.Empty;
        currentLevelTxt.text = string.Empty;
        requiredItemCountTxt.text = "0";
        currentItemCountTxt.text = "0";
        itemCountGauge.value = 0;

        UpdateSlotColor(0);
        equipTxt.gameObject.SetActive(false); // 장착 상태 숨김
        slotButton.onClick.RemoveAllListeners();

        // 그레이드 이미지 비활성화
        DisableAllGradeImages();
    }

    public void UpdateSlotState(int totalItemCount)
    {
        int requiredItemCount = GetRequiredFuseItemCount();
        int displayedItemCount = Mathf.Max(0, totalItemCount - 1);

        if (item != null)
        {
            equipmentIcon.sprite = item.BaseData.icon;
            rankTxt.text = item.BaseData is WeaponDataSO weapon ? weapon.rank.ToString() :
                           item.BaseData is AccessoryDataSO accessory ? accessory.rank.ToString() : string.Empty;
            currentLevelTxt.text = $"+{item.EnhancementLevel}";

            UpdateGradeImage(item.BaseData.grade);
        }
        else
        {
            equipmentIcon.sprite = null;
            rankTxt.text = "N/A";
            currentLevelTxt.text = "N/A";

            DisableAllGradeImages();
        }

        currentItemCountTxt.text = displayedItemCount.ToString();
        requiredItemCountTxt.text = requiredItemCount.ToString();
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

    private void UpdateGradeImage(Grade grade)
    {
        DisableAllGradeImages();

        switch (grade)
        {
            case Grade.Normal:
                normalImage.SetActive(true);
                break;
            case Grade.Uncommon:
                uncommonImage.SetActive(true);
                break;
            case Grade.Rare:
                rareImage.SetActive(true);
                break;
            case Grade.Hero:
                heroImage.SetActive(true);
                break;
            case Grade.Legendary:
                legendaryImage.SetActive(true);
                break;
            case Grade.Mythic:
                mythicImage.SetActive(true);
                break;
            case Grade.Ultimate:
                ultimateImage.SetActive(true);
                break;
        }
    }

    private void DisableAllGradeImages()
    {
        normalImage.SetActive(false);
        uncommonImage.SetActive(false);
        rareImage.SetActive(false);
        heroImage.SetActive(false);
        legendaryImage.SetActive(false);
        mythicImage.SetActive(false);
        ultimateImage.SetActive(false);
    }
}
