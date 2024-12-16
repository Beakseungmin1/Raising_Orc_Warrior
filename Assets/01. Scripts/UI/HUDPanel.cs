using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDPanel : UIBase
{
    private PlayerStat stat;

    [SerializeField] private TextMeshProUGUI playerNameTxt;
    [SerializeField] private TextMeshProUGUI playerlevelTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private Slider expBar;
    [SerializeField] private TextMeshProUGUI emeraldTxt;
    [SerializeField] private TextMeshProUGUI diamondTxt;




    private void Start()
    {
        stat = PlayerObjManager.Instance.Player.stat;

        stat.UpdateLevelStatUI += UpdateTopInformatinBar;
        stat.UpdateLevelStatUI?.Invoke();
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<PlayerInfoPopupUI>();
        stat.UpdateUserInformationUI?.Invoke();
    }

    public void UpdateTopInformatinBar()
    {
        if (stat != null)
        {
            playerlevelTxt.text = "Lv." + stat.level.ToString();

            BigInteger currentExp = stat.exp;
            BigInteger needExp = stat.needExp;

            //���� ����ġ�� �Ѱ���ġ�� ��% ���� ���
            float percentage = needExp > 0 ? (float)(currentExp / needExp) * 100 : 0;

            //����ġ ǥ�ð� 100% �� �Ѿ�� ���ϰ���
            float ExppercentTxt = percentage >= 100 ? 100 : percentage;

            expBar.value = percentage / 100;

            expTxt.text = ExppercentTxt.ToString("F2");

        }
    }




}
