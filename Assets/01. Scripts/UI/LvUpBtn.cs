using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LvUpBtn : MonoBehaviour
{
    public void OnLevelUpButton()
    {
        PlayerobjManager.Instance.Player.stat.LevelUp();
    }
}
