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

    private int requiredItemCount = 1; // �⺻ �ʿ� ����

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

            // ���� ���� ���� ǥ��
            currentItemCountTxt.text = currentItemCount.ToString();

            // �ʿ� ���� ����
            requiredItemCountTxt.text = requiredItemCount.ToString();

            // ��ȭ ���� ǥ��
            currentLevelTxt.text = $"+{equipment.EnhancementLevel}";

            // ��ũ ǥ�� (���� �Ǵ� �Ǽ������� �ش�)
            if (isWeaponType || equipment is Accessory)
            {
                rankTxt.text = (equipment.BaseData as WeaponDataSO)?.rank.ToString() ??
                               (equipment.BaseData as AccessoryDataSO)?.rank.ToString();
            }
            else
            {
                rankTxt.text = string.Empty; // ��ų���� rank ����
            }

            // ����ġ ������ ������Ʈ
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