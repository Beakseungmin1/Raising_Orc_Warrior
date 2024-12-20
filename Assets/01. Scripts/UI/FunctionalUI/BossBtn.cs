using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBtn : MonoBehaviour
{
    public void OnBossButton()
    {
        StageManager.Instance.StageClear();
    }
}
