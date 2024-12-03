using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_SkillUI : MonoBehaviour
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
                Debug.Log("동료캔버스열기");
                break;
            case 4:
                Debug.Log("모험캔버스열기");
                break;
            case 5:
                Debug.Log("상점캔버스열기");
                break;
        }
        UIManager.Instance.Hide("Main_SkillUI");
    }

    public void ShowSkillInfoPopupUI()
    {
        UIManager.Instance.Show("DimmedImage");
        //UIManager.Instance.Show("SkillInfoPopupUI");
    }
}
