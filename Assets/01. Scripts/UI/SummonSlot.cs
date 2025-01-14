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
        gradeTxt.text = TranslateGrade(weaponDataSO.grade);
        gradeTxt.color = weaponDataSO.GetGradeColor(weaponDataSO.grade);
        rankTxt.text = weaponDataSO.rank.ToString();
        image.sprite = weaponDataSO.icon;
        EnableUI();
    }

    private void SetAccessorySlot(AccessoryDataSO accessoryDataSO)
    {
        gradeTxt.text = TranslateGrade(accessoryDataSO.grade);
        gradeTxt.color = accessoryDataSO.GetGradeColor(accessoryDataSO.grade);
        rankTxt.text = accessoryDataSO.rank.ToString();
        image.sprite = accessoryDataSO.icon;
        EnableUI();
    }

    private void SetSkillSlot(SkillDataSO skillDataSO)
    {
        gradeTxt.text = TranslateGrade(skillDataSO.grade);
        gradeTxt.color = skillDataSO.gradeColor;
        image.sprite = skillDataSO.icon;
        EnableUI();
    }

    private void EnableUI()
    {
        image.gameObject.SetActive(true);
        gradeTxt.gameObject.SetActive(true);
        rankTxt.gameObject.SetActive(!isSkillSummoning);
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

    private string TranslateGrade(Grade grade)
    {
        return grade switch
        {
            Grade.Normal => "ÀÏ¹Ý",
            Grade.Uncommon => "Èñ±Í",
            Grade.Rare => "·¹¾î",
            Grade.Hero => "¿µ¿õ",
            Grade.Legendary => "Àü¼³",
            Grade.Mythic => "½ÅÈ­",
            Grade.Ultimate => "ºÒ¸ê",
            _ => "¾Ë ¼ö ¾øÀ½"
        };
    }
}