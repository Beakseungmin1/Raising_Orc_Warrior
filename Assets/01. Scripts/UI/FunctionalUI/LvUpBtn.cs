using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpBtn : MonoBehaviour
{
    public void OnLevelUpButton()
    {
        PlayerObjManager.Instance.Player.stat.LevelUp();
    }
}
