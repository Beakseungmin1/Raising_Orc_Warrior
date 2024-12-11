using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EquipmentInventorySlot : UIBase
{
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI currentLevelTxt; // ��ȭ ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI rankTxt; // ��ũ �ؽ�Ʈ
    [SerializeField] private Slider expGauge; // ����ġ ������
    [SerializeField] private TextMeshProUGUI requiredItemCountTxt; // �ʿ��� ������ ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI currentItemCountTxt; // ���� ������ ���� �ؽ�Ʈ
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

            UpdateSlotState(0); // �ʱ�ȭ �� ���� 0 ���·� ����
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
        // ���� ���� �� ��ȭ ���� ������Ʈ
        currentItemCountTxt.text = stackCount.ToString();
        currentLevelTxt.text = $"+{item.EnhancementLevel}";
        requiredItemCountTxt.text = "5"; // �ʿ� ����(���÷� ����)
        expGauge.value = Mathf.Clamp01((float)stackCount / 5f); // �ʿ� ���� �������� ������ ����
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

        // �˾� ǥ�� ����
        UIManager.Instance.Show<DimmedUI>();
        var upgradePopup = UIManager.Instance.Show<EquipmentUpgradePopupUI>();
        upgradePopup.SetEquipmentData(item, isWeaponSlot);
    }

    private void UpdateSlotColor(bool hasItem)
    {
        equipmentIcon.color = hasItem ? defaultColor : emptyColor;
    }
}