using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_PlayerUpgradeUI : MonoBehaviour
{
    public void ShowMainUI(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Show("Main_PlayerUpgradeUI");
                break;
            case 1:
                UIManager.Instance.Show("Main_SkillUI");
                break;
            case 2:
                UIManager.Instance.Show("Main_EquipmentUI");
                break;
            case 3:
                Debug.Log("����ĵ��������");
                break;
            case 4:
                Debug.Log("����ĵ��������");
                break;
            case 5:
                Debug.Log("����ĵ��������");
                break;
        }
        UIManager.Instance.Hide("Main_PlayerUpgradeUI");
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show("DimmedImage");
        UIManager.Instance.Show("PlayerInfoPopupUI");
    }
}
