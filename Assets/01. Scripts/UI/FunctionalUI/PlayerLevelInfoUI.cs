using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelInfoUI : MonoBehaviour
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI UILevelTxt;
    [SerializeField] private Slider ExpBar;
    [SerializeField] private TextMeshProUGUI UIExppercentTxt;
    [SerializeField] private TextMeshProUGUI UICurExpTxt;
    [SerializeField] private TextMeshProUGUI UINeedExpTxt;


    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;

        stat.UpdateLevelStatUI += UpdateLevelUI;

        UpdateLevelUI();
    }



    public void UpdateLevelUI()
    {
        if (stat != null)
        {
            UILevelTxt.text = stat.level.ToString();

            BigInteger currentExp = stat.exp;
            BigInteger needExp = stat.needExp;

            //현재 경험치가 총경험치의 몇% 인지 계산
            float percentage = needExp > 0 ? (float)(currentExp / needExp) * 100 : 0;

            //경험치 표시가 100% 을 넘어가지 못하게함
            float ExppercentTxt = percentage >= 100 ? 100 : percentage;

            ExpBar.value = percentage / 100;

            UIExppercentTxt.text = ExppercentTxt.ToString("F2");

            UICurExpTxt.text = currentExp.ToString();
            UINeedExpTxt.text = needExp.ToString();

        }
        else
        {
            return;
        }
    }






}
