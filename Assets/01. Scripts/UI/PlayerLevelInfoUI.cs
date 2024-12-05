using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelInfoUI : Singleton<PlayerLevelInfoUI>
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI UILevelTxt;
    [SerializeField] private Slider ExpBar;
    [SerializeField] private TextMeshProUGUI UIExppercentTxt;
    [SerializeField] private TextMeshProUGUI UICurExpTxt;
    [SerializeField] private TextMeshProUGUI UINeedExpTxt;

    private void Awake()
    {
        stat = PlayerobjManager.Instance.Player.stat;
    }


    public void UpdateLevelUI()
    {
        if (stat != null)
        {
            UILevelTxt.text = stat.GetLevel().ToString();

            float currentExp = stat.GetExp();
            float needExp = stat.GetNeedExp();

            //���� ����ġ�� �Ѱ���ġ�� ��% ���� ���
            float percentage = needExp > 0 ? (currentExp / needExp) * 100 : 0;

            //����ġ ǥ�ð� 100% �� �Ѿ�� ���ϰ���
            float ExppercentTxt = percentage >= 100 ? 100 : percentage;

            ExpBar.value = percentage / 100;

            UIExppercentTxt.text = ExppercentTxt.ToString("F2");

            UICurExpTxt.text = currentExp.ToString();
            UINeedExpTxt.text = needExp.ToString();
        }
    }






}
