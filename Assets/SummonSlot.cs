using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummonSlot : UIBase
{
    [SerializeField] private WeaponDataSO weaponDataSO;

    public TextMeshProUGUI gradeTxt;
    public TextMeshProUGUI rankTxt;
    public TextMeshProUGUI rankLabel;
    public Image image;

    private void Awake()
    {
        image.enabled = false;
        gradeTxt.enabled = false;
        rankTxt.enabled = false;
        rankLabel.enabled = false;
    }

    public void SetSlot(WeaponDataSO so)
    {
        Debug.Log("SetSLot");
        weaponDataSO = so;
        gradeTxt.text = weaponDataSO.grade.ToString();
        rankTxt.text = weaponDataSO.rank.ToString();
        image.sprite = weaponDataSO.icon;
        image.enabled = true;
        gradeTxt.enabled = true;
        rankTxt.enabled = true;
        rankLabel.enabled = true;
    }
}
