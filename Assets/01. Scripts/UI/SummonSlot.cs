using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummonSlot : UIBase
{
    public BaseItemDataSO baseItemDataSO;

    public TextMeshProUGUI gradeTxt;
    public TextMeshProUGUI rankTxt;
    public TextMeshProUGUI rankLabel;
    public Image image;

    public bool isSkillSummoning = false;

    private void Awake()
    {
        image.gameObject.SetActive(false);
        gradeTxt.gameObject.SetActive(false);
        rankTxt.gameObject.SetActive(false);
        rankLabel.gameObject.SetActive(false);
    }

    public void SetSlot<T>(T so) where T : BaseItemDataSO
    {
        baseItemDataSO = so;

        if (so is WeaponDataSO weaponDataSO)
        {
            isSkillSummoning = false;
            SetWeaponSlot(weaponDataSO);
        }
        else if (so is AccessoryDataSO accessoryDataSO)
        {
            isSkillSummoning = false;
            SetAccessorySlot(accessoryDataSO);
        }
        else if (so is SkillDataSO skillDataSO)
        {
            isSkillSummoning = true;
            SetSkillSlot(skillDataSO);
        }
        else
        {
            Debug.LogWarning("Unsupported data type for SetSlot.");
        }
    }

    private void SetWeaponSlot(WeaponDataSO weaponDataSO)
    {
        gradeTxt.text = weaponDataSO.grade.ToString();
        rankTxt.text = weaponDataSO.rank.ToString();
        image.sprite = weaponDataSO.icon;
        EnableUI();
    }

    private void SetAccessorySlot(AccessoryDataSO accessoryDataSO)
    {
        gradeTxt.text = accessoryDataSO.grade.ToString();
        rankTxt.text = accessoryDataSO.rank.ToString();
        image.sprite = accessoryDataSO.icon;
        EnableUI();
    }

    private void SetSkillSlot(SkillDataSO skillDataSO)
    {
        gradeTxt.text = skillDataSO.grade.ToString();
        rankTxt.text = ""; // ½ºÅ³¿¡ ·©Å©°¡ ¾øÀ¸¸é ºóÄ­ Ã³¸®
        image.sprite = skillDataSO.icon;
        EnableUI();
    }

    private void EnableUI()
    {
        image.gameObject.SetActive(true);
        gradeTxt.gameObject.SetActive(true);
        rankTxt.gameObject.SetActive(true);
        rankLabel.gameObject.SetActive(!isSkillSummoning);
    }


    public void ClearSlot()
    {
        baseItemDataSO = null;
        gradeTxt.text = null;
        rankTxt.text = null;
        image.sprite = null;
        image.gameObject.SetActive(false);
        gradeTxt.gameObject.SetActive(false);
        rankTxt.gameObject.SetActive(false);
        rankLabel.gameObject.SetActive(false);
    }
}
