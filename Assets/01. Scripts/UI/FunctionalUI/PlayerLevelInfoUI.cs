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

            //���� ����ġ�� �Ѱ���ġ�� ��% ���� ���
            float percentage = needExp > 0 ? (float)(currentExp / needExp) * 100 : 0;

            //����ġ ǥ�ð� 100% �� �Ѿ�� ���ϰ���
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
