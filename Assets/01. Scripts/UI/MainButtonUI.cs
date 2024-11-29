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
                UIManager.Instance.Show("PlayerLevelupBottomUICanvas");
                break;
            case 1:
                UIManager.Instance.Show("SkillCanvas");
                break;
            case 2:
                UIManager.Instance.Show("EquipmentCanvas");
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
    }

}

