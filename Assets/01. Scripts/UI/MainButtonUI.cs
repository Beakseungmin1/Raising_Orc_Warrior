using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainButtonUI : MonoBehaviour
{

    public void ShowCanvas(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Show("PlayerLevelupBottomUI");
                break;
            case 1:
                UIManager.Instance.Show("SkillBottomUI");
                break;
            case 2:
                UIManager.Instance.Show("EquipmentBottomUI");
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
    }

}

