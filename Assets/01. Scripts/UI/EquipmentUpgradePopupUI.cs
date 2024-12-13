using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class EquipmentUpgradePopupUI : UIBase
{
    [Header("Equipment Icon Area")]
    [SerializeField] private Image equipmentIcon;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI gradeTxt;
    [SerializeField] private Slider progressSlider;
    [SerializeField] private TextMeshProUGUI currentAmountTxt;
    [SerializeField] private TextMeshProUGUI neededAmountTxt;

    [Header("Effect Area")]
    [SerializeField] private TextMeshProUGUI equipEffectTypeTxt;
    [SerializeField] private TextMeshProUGUI equipEffectValueTxt;

    [Header("Possess Effect Area")]
    [SerializeField] private GameObject possessEffectRow0;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt0;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt0;
    [SerializeField] private GameObject possessEffectRow1;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt1;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt1;
    [SerializeField] private GameObject possessEffectRow2;
    [SerializeField] private TextMeshProUGUI possessEffectTypeTxt2;
    [SerializeField] private TextMeshProUGUI possessEffectValueTxt2;

    [Header("Upgrade Info")]
    [SerializeField] private Button upgradeBtn;
    [SerializeField] private Button equipBtn;
    [SerializeField] private TextMeshProUGUI upgradeCostTxt;
    [SerializeField] private TextMeshProUGUI curCubeAmountTxt;
    [SerializeField] private Image curCubeIcon;
    [SerializeField] private Button fusionBtn;
    [SerializeField] private Button exitBtn;

    [Header("Navigation Buttons")]
    [SerializeField] private Button leftArrowBtn;
    [SerializeField] private Button rightArrowBtn;

    private IEnhanceable currentItem;
    private bool isWeapon;
    private Button currentlyEquippedButton;

    private void Start()
    {
        upgradeBtn.onClick.AddListener(UpgradeItem);
        equipBtn.onClick.AddListener(EquipItem);
        exitBtn.onClick.AddListener(ClosePopup);
        fusionBtn.onClick.AddListener(ReturnToFusionUI);
        leftArrowBtn.onClick.AddListener(SelectPreviousItem);
        rightArrowBtn.onClick.AddListener(SelectNextItem);
    }

    public void SetEquipmentData(IEnhanceable equipment, bool isWeaponType)
    {
        if (equipment == null)
        {
            Debug.LogError("SetEquipmentData: equipment is null!");
            DisableEnhanceAndEquipButtons();
            return;
        }

        currentItem = equipment; // ���� ������ ����
        isWeapon = isWeaponType;

        InitializeUI(); // UI ������Ʈ
        UpdateEquipButtonState(); // ���� ���� ����ȭ
        UpdateNavigationButtons(); // ȭ��ǥ ���� ������Ʈ
    }

    private void InitializeUI()
    {
        if (currentItem == null)
        {
            DisableEnhanceAndEquipButtons();
            return;
        }

        if (isWeapon && currentItem is Weapon weapon)
        {
            equipEffectTypeTxt.text = "���ݷ� ����";
            equipEffectValueTxt.text = $"{weapon.EquipAtkIncreaseRate}%";
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            equipEffectTypeTxt.text = "ü��/ü��ȸ���� ����";
            equipEffectValueTxt.text = $"{accessory.EquipHpAndHpRecoveryIncreaseRate}%";
        }

        // ���� ������ ���� ���� Ȯ��
        int actualStackCount = PlayerObjManager.Instance.Player.inventory.GetItemStackCount(currentItem);
        bool isItemAvailable = actualStackCount > 0; // �κ��丮�� �������� �ִ��� Ȯ��

        int requiredAmount = GetRequiredFuseItemCount();

        equipmentIcon.sprite = currentItem.BaseData.icon;
        equipmentIcon.color = isItemAvailable ? Color.white : new Color(0.2f, 0.2f, 0.2f, 1f); // ������ ó��
        nameTxt.text = currentItem.BaseData.itemName;
        gradeTxt.text = $"[{currentItem.BaseData.grade}]";

        currentAmountTxt.text = Mathf.Max(0, actualStackCount - 1).ToString();
        neededAmountTxt.text = requiredAmount.ToString();
        progressSlider.value = Mathf.Clamp01((float)(actualStackCount - 1) / requiredAmount);

        upgradeCostTxt.text = currentItem.RequiredCurrencyForUpgrade.ToString();
        curCubeAmountTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Cube).ToString();
        curCubeIcon.sprite = currentItem.BaseData.currencyIcon;

        UpdateEquipButtonState();
        UpdatePossessEffects();
        UpdateUpgradeButtonState(isItemAvailable);
    }

    private bool IsCurrentlyEquipped(IEnhanceable item)
    {
        if (isWeapon && item is Weapon weapon)
        {
            return PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.IsWeaponEquipped(weapon) ?? false;
        }
        else if (!isWeapon && item is Accessory accessory)
        {
            return PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.IsAccessoryEquipped(accessory) ?? false;
        }
        return false;
    }

    private int GetRequiredFuseItemCount()
    {
        if (currentItem is Weapon weapon)
        {
            return weapon.BaseData.requireFuseItemCount;
        }
        else if (currentItem is Accessory accessory)
        {
            return accessory.BaseData.requireFuseItemCount;
        }

        return 5; // �⺻��
    }

    private void UpdateUpgradeButtonState(bool isItemAvailable)
    {
        bool canEnhance = currentItem != null && currentItem.CanEnhance();
        bool canEquip = isItemAvailable; // �κ��丮�� �������� ���� ���� ���� ����

        upgradeBtn.gameObject.SetActive(isItemAvailable && canEnhance); // �������� ������ ��ư ��ü ����
        equipBtn.gameObject.SetActive(isItemAvailable && canEquip);    // �������� ������ ��ư ��ü ����
    }

    private void UpdateEquipButtonState()
    {
        if (currentItem == null)
        {
            equipBtn.gameObject.SetActive(false);
            return;
        }

        var equipManager = PlayerObjManager.Instance.Player.GetComponent<EquipManager>();

        // ���� �������� ���� �������� Ȯ��
        if (isWeapon && currentItem is Weapon weapon)
        {
            if (equipManager.EquippedWeapon == weapon)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "������";
                equipBtn.interactable = false;
                return; // ���� ����
            }
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (equipManager.EquippedAccessory == accessory)
            {
                equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "������";
                equipBtn.interactable = false;
                return; // ���� ����
            }
        }

        // ���� �������� ���� ���°� �ƴ� ��� �⺻ ��
        equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "����";
        equipBtn.interactable = true;
    }

    private void UpdatePossessEffects()
    {
        possessEffectRow0.SetActive(false);
        possessEffectRow1.SetActive(false);
        possessEffectRow2.SetActive(false);

        int rowIndex = 0;

        if (isWeapon && currentItem is Weapon weapon)
        {
            if (weapon.PassiveEquipAtkIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "���ݷ� ����", $"{weapon.PassiveEquipAtkIncreaseRate}%");
            }
            if (weapon.PassiveCriticalDamageBonus > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ġ��Ÿ ������", $"{weapon.PassiveCriticalDamageBonus}%");
            }
            if (weapon.PassiveGoldGainRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ��� ȹ�淮", $"{weapon.PassiveGoldGainRate}%");
            }
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            if (accessory.PassiveHpAndHpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "ü��/ü��ȸ�� ����", $"{accessory.PassiveHpAndHpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveMpAndMpRecoveryIncreaseRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "����/����ȸ�� ����", $"{accessory.PassiveMpAndMpRecoveryIncreaseRate}%");
            }
            if (accessory.PassiveAddEXPRate > 0)
            {
                AddPossessEffectRow(rowIndex++, "�߰� ����ġ", $"{accessory.PassiveAddEXPRate}%");
            }
        }
    }

    private void AddPossessEffectRow(int rowIndex, string effectType, string effectValue)
    {
        switch (rowIndex)
        {
            case 0:
                possessEffectRow0.SetActive(true);
                possessEffectTypeTxt0.text = effectType;
                possessEffectValueTxt0.text = effectValue;
                break;
            case 1:
                possessEffectRow1.SetActive(true);
                possessEffectTypeTxt1.text = effectType;
                possessEffectValueTxt1.text = effectValue;
                break;
            case 2:
                possessEffectRow2.SetActive(true);
                possessEffectTypeTxt2.text = effectType;
                possessEffectValueTxt2.text = effectValue;
                break;
        }
    }

    public void DisableEnhanceAndEquipButtons()
    {
        upgradeBtn.gameObject.SetActive(false);
        equipBtn.gameObject.SetActive(false);
    }

    private void UpgradeItem()
    {
        if (currentItem == null || !currentItem.CanEnhance())
        {
            return;
        }

        bool success = currentItem.Enhance();

        if (success)
        {
            InitializeUI();
        }
    }

    private void EquipItem()
    {
        if (currentItem == null)
        {
            Debug.LogError("EquipItem: currentItem is null!");
            return;
        }

        // ���� ���� ���� ����
        if (currentlyEquippedButton != null)
        {
            currentlyEquippedButton.interactable = true;
            currentlyEquippedButton.GetComponentInChildren<TextMeshProUGUI>().text = "�����ϱ�";
        }

        if (isWeapon && currentItem is Weapon weapon)
        {
            PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.EquipWeapon(weapon);
        }
        else if (!isWeapon && currentItem is Accessory accessory)
        {
            PlayerObjManager.Instance.Player.GetComponent<EquipManager>()?.EquipAccessory(accessory);
        }

        // ���� ��ư ���� ������Ʈ
        equipBtn.interactable = false;
        equipBtn.GetComponentInChildren<TextMeshProUGUI>().text = "������";
        currentlyEquippedButton = equipBtn;

        Debug.Log($"{currentItem.BaseData.itemName} ���� �Ϸ�!");
    }

    private void ClosePopup()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }

    private void ReturnToFusionUI()
    {
        Hide();
        var fusionPopup = UIManager.Instance.Show<EquipmentFusionPopupUI>();
        if (fusionPopup != null && currentItem is IFusable fusableItem)
        {
            fusionPopup.SetEquipmentData(fusableItem, currentItem.BaseData);
        }
    }

    private void SelectPreviousItem()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = inventory.WeaponInventory.GetAllItems(); // ���� ���� ������ ��������
            int currentIndex = weaponList.IndexOf((Weapon)currentItem);

            if (currentIndex > 0)
            {
                currentItem = weaponList[currentIndex - 1]; // ���� ����� �̵�
                SetEquipmentData(currentItem, true);
            }
        }
        else
        {
            var accessoryList = inventory.AccessoryInventory.GetAllItems(); // ���� �Ǽ��縮 ������ ��������
            int currentIndex = accessoryList.IndexOf((Accessory)currentItem);

            if (currentIndex > 0)
            {
                currentItem = accessoryList[currentIndex - 1]; // ���� �Ǽ��縮�� �̵�
                SetEquipmentData(currentItem, false);
            }
        }

        UpdateNavigationButtons();
        UpdateEquipButtonState();
    }

    private void SelectNextItem()
    {
        var inventory = PlayerObjManager.Instance.Player.inventory;

        if (isWeapon)
        {
            var weaponList = inventory.WeaponInventory.GetAllItems(); // ���� ���� ������ ��������
            int currentIndex = weaponList.IndexOf((Weapon)currentItem);

            if (currentIndex < weaponList.Count - 1)
            {
                currentItem = weaponList[currentIndex + 1]; // ���� ����� �̵�
                SetEquipmentData(currentItem, true);
            }
        }
        else
        {
            var accessoryList = inventory.AccessoryInventory.GetAllItems(); // ���� �Ǽ��縮 ������ ��������
            int currentIndex = accessoryList.IndexOf((Accessory)currentItem);

            if (currentIndex < accessoryList.Count - 1)
            {
                currentItem = accessoryList[currentIndex + 1]; // ���� �Ǽ��縮�� �̵�
                SetEquipmentData(currentItem, false);
            }
        }

        UpdateNavigationButtons();
        UpdateEquipButtonState();
    }

    private void UpdateNavigationButtons()
    {
        // ������ ����Ʈ ��������
        var dataList = isWeapon
            ? DataManager.Instance.GetAllWeapons()
                .OrderBy(item => item.grade)
                .ThenByDescending(item => item.rank)
                .Cast<BaseItemDataSO>()
                .ToList()
            : DataManager.Instance.GetAllAccessories()
                .OrderBy(item => item.grade)
                .ThenByDescending(item => item.rank)
                .Cast<BaseItemDataSO>()
                .ToList();

        // ���� �������� �ε��� ���
        int currentIndex = dataList.FindIndex(item => item == currentItem.BaseData);

        // ��ư ���� ������Ʈ
        leftArrowBtn.gameObject.SetActive(currentIndex > 0);               // ù ��° �����ۿ��� ���� ��ư ����
        rightArrowBtn.gameObject.SetActive(currentIndex < dataList.Count - 1); // ������ �����ۿ��� ������ ��ư ����
    }

    private IEnhanceable CreateEnhanceableItem(BaseItemDataSO baseItemData)
    {
        if (baseItemData is WeaponDataSO weaponData)
        {
            return new Weapon(weaponData);
        }
        else if (baseItemData is AccessoryDataSO accessoryData)
        {
            return new Accessory(accessoryData);
        }
        return null;
    }
}