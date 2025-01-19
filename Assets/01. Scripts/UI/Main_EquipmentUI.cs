using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Main_EquipmentUI : UIBase
{
    public Sprite openedTabSprite;
    public Sprite closedTabSprite;

    public Image weaponTab;
    public Image accTab;

    public TextMeshProUGUI weaponLabel;
    public TextMeshProUGUI accLabel;

    bool isWeaponTab = true;

    private void OnEnable()
    {
        isWeaponTab = true;
        weaponTab.sprite = openedTabSprite;
        accTab.sprite = closedTabSprite;
        weaponLabel.color = Color.black;
        accLabel.color = Color.white;
    }

    public void ShowEquipmentUpgradePopupUI()
    {
        UIManager.Instance.Show<EquipmentUpgradePopupUI>();
    }

    public void OnClickWeaponTab()
    {
        if (!isWeaponTab)
        {
            weaponTab.sprite = openedTabSprite;
            accTab.sprite = closedTabSprite;
            weaponLabel.color = Color.black;
            accLabel.color = Color.white;
            isWeaponTab = true;
        }
    }

    public void OnClickAccTab()
    {
        if (isWeaponTab)
        {
            weaponTab.sprite = closedTabSprite;
            accTab.sprite = openedTabSprite;
            weaponLabel.color = Color.white;
            accLabel.color = Color.black;
            isWeaponTab = false;
        }
    }
}