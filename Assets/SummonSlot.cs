using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SummonSlot : MonoBehaviour
{
    public WeaponDataSO weaponDataSO;

    public TextMeshProUGUI gradeTxt;
    public TextMeshProUGUI rankTxt;
    public Image image;

    private void Awake()
    {
        gradeTxt.text = weaponDataSO.grade.ToString();
        rankTxt.text = weaponDataSO.rank.ToString();
        image.sprite = weaponDataSO.icon;
    }
}
