using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SuggetionGroupUI : UIBase
{
    public delegate void EquipCompleteHandler();
    public static event EquipCompleteHandler OnEquipComplete;

    [Header("Weapon UI")]
    [SerializeField] private GameObject weaponUI;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponGradeText;
    [SerializeField] private TextMeshProUGUI weaponRankText;
    [SerializeField] private Button weaponEquipButton;
    [SerializeField] private GameObject weaponOutline;

    [Header("Accessory UI")]
    [SerializeField] private GameObject accessoryUI;
    [SerializeField] private Image accessoryIcon;
    [SerializeField] private TextMeshProUGUI accessoryGradeText;
    [SerializeField] private TextMeshProUGUI accessoryRankText;
    [SerializeField] private Button accessoryEquipButton;
    [SerializeField] private GameObject accessoryOutline;

    private PlayerInventory playerInventory;
    private EquipManager equipManager;

    private void Start()
    {
        playerInventory = PlayerObjManager.Instance.Player.inventory;
        equipManager = PlayerObjManager.Instance.Player.EquipManager;

        if (playerInventory != null && equipManager != null)
        {
            playerInventory.OnInventoryChanged += OnInventoryChanged;
            equipManager.OnEquippedChanged += OnEquippedChanged;
        }

        weaponUI.SetActive(false);
        accessoryUI.SetActive(false);

        weaponEquipButton.onClick.AddListener(EquipSuggestedWeapon);
        accessoryEquipButton.onClick.AddListener(EquipSuggestedAccessory);
    }

    private void OnDestroy()
    {
        if (playerInventory != null)
        {
            playerInventory.OnInventoryChanged -= OnInventoryChanged;
        }
    }

    private void OnInventoryChanged(bool isWeapon)
    {
        if (isWeapon)
        {
            HandleWeaponSuggestion();
        }
        else
        {
            HandleAccessorySuggestion();
        }
    }

    private void OnEquippedChanged()
    {
        bool isWeapon = equipManager.EquippedWeapon != null;
        bool isAccessory = equipManager.EquippedAccessory != null;

        if (isWeapon)
        {
            HandleWeaponSuggestion();
        }

        if (isAccessory)
        {
            HandleAccessorySuggestion();
        }
    }

    private void HandleWeaponSuggestion()
    {
        Weapon currentWeapon = equipManager.EquippedWeapon;
        Weapon bestWeapon = GetBestWeaponInInventory();

        if (bestWeapon != null && (currentWeapon == null || IsBetterItem(bestWeapon, currentWeapon)))
        {
            ShowWeaponUI(bestWeapon);
            StartOutlineGlow(weaponOutline);
        }
        else
        {
            HideWeaponUI();
            StopOutlineGlow(weaponOutline);
        }
    }

    private void HandleAccessorySuggestion()
    {
        Accessory currentAccessory = equipManager.EquippedAccessory;
        Accessory bestAccessory = GetBestAccessoryInInventory();

        if (bestAccessory != null && (currentAccessory == null || IsBetterItem(bestAccessory, currentAccessory)))
        {
            ShowAccessoryUI(bestAccessory);
            StartOutlineGlow(accessoryOutline);
        }
        else
        {
            HideAccessoryUI();
            StopOutlineGlow(accessoryOutline);
        }
    }

    private Weapon GetBestWeaponInInventory()
    {
        var allWeapons = playerInventory.WeaponInventory.GetAllItems();
        return allWeapons
            .OrderByDescending(w => w.Grade)
            .ThenBy(w => w.Rank)
            .FirstOrDefault();
    }

    private Accessory GetBestAccessoryInInventory()
    {
        var allAccessories = playerInventory.AccessoryInventory.GetAllItems();
        return allAccessories
            .OrderByDescending(a => a.Grade)
            .ThenBy(a => a.Rank)
            .FirstOrDefault();
    }

    private bool IsBetterItem<T>(T newItem, T currentItem) where T : class, IFusable
    {
        if (newItem.Grade > currentItem.Grade) return true;
        if (newItem.Grade == currentItem.Grade && newItem.Rank < currentItem.Rank) return true;
        return false;
    }

    private void ShowWeaponUI(Weapon bestWeapon)
    {
        weaponGradeText.text = $"{bestWeapon.Grade.ToLocalizedString()}";
        weaponRankText.text = $"{bestWeapon.Rank}등급";
        weaponIcon.sprite = bestWeapon.BaseData.icon;
        weaponGradeText.color = bestWeapon.GradeColor;

        weaponUI.SetActive(true);
    }

    private void HideWeaponUI()
    {
        weaponUI.SetActive(false);
    }

    private void EquipSuggestedWeapon()
    {
        Weapon bestWeapon = GetBestWeaponInInventory();
        if (bestWeapon != null)
        {
            equipManager.EquipWeapon(bestWeapon);
            HideWeaponUI();
            GameEventsManager.Instance.messageEvents.ShowMessage(MessageTextType.Equipped, 0.5f, 100);
        }
    }

    private void ShowAccessoryUI(Accessory bestAccessory)
    {
        accessoryGradeText.text = $"{bestAccessory.Grade.ToLocalizedString()}";
        accessoryRankText.text = $"{bestAccessory.Rank}등급";
        accessoryIcon.sprite = bestAccessory.BaseData.icon;
        accessoryGradeText.color = bestAccessory.GradeColor;

        accessoryUI.SetActive(true);
    }

    private void HideAccessoryUI()
    {
        accessoryUI.SetActive(false);
    }

    private void EquipSuggestedAccessory()
    {
        Accessory bestAccessory = GetBestAccessoryInInventory();
        if (bestAccessory != null)
        {
            equipManager.EquipAccessory(bestAccessory);
            HideAccessoryUI();
            GameEventsManager.Instance.messageEvents.ShowMessage(MessageTextType.Equipped, 0.5f, 100);
        }
    }

    private void StartOutlineGlow(GameObject outlineObject)
    {
        StartCoroutine(GlowOutlineCoroutine(outlineObject));
    }

    private IEnumerator GlowOutlineCoroutine(GameObject outlineObject)
    {
        Image outlineImage = outlineObject.GetComponent<Image>();
        if (outlineImage != null)
        {
            float glowSpeed = 1f;
            float minAlpha = 0f;
            float maxAlpha = 1f;
            float elapsedTime = 0f;

            while (true)
            {
                float alpha = Mathf.PingPong(elapsedTime * glowSpeed, maxAlpha - minAlpha) + minAlpha;
                Color currentColor = outlineImage.color;
                outlineImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, alpha);

                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }

    private void StopOutlineGlow(GameObject outlineObject)
    {
        StopCoroutine(GlowOutlineCoroutine(outlineObject));

        Image outlineImage = outlineObject.GetComponent<Image>();
        if (outlineImage != null)
        {
            Color currentColor = outlineImage.color;
            outlineImage.color = new Color(currentColor.r, currentColor.g, currentColor.b, 1f);
        }
    }
}