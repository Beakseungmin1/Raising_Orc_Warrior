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
    [SerializeField] private Button settingBtn;


    private void OnEnable()
    {
        GameEventsManager.Instance.currencyEvents.onEmeraldChanged += RefreshUI;
        GameEventsManager.Instance.currencyEvents.onDiamondChanged += RefreshUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.currencyEvents.onEmeraldChanged -= RefreshUI;
        GameEventsManager.Instance.currencyEvents.onDiamondChanged -= RefreshUI;
    }

    private void Start()
    {
        
        stat = PlayerObjManager.Instance.Player.stat;

        stat.UpdateLevelStatUI += UpdateTopInformatinBar;
        stat.UpdateLevelStatUI?.Invoke();

        RefreshUI();
    }

    public void RefreshUI()
    {
        //playerNameTxt.text = PlayerObjManager.Instance.Player.playerInformation.playerName;
        emeraldTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Emerald).ToString();
        diamondTxt.text = CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond).ToString();
    }

    public void ShowPlayerInfoPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<PlayerInfoPopupUI>();
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
        stat.UpdateUserInformationUI?.Invoke();
    }

    public void UpdateTopInformatinBar()
    {
        if (stat != null)
        {
            playerlevelTxt.text = "Lv." + (stat.level + 1).ToString();

            BigInteger currentExp = stat.exp;
            BigInteger needExp = stat.needExp;

            //현재 경험치가 총경험치의 몇% 인지 계산
            float percentage = needExp > 0 ? (float)((double)currentExp / (double)needExp) * 100 : 0;
            
            // (엄청난 int수 / 엄청난 int수)

            //경험치 표시가 100% 을 넘어가지 못하게함
            float ExppercentTxt = percentage >= 100 ? 100 : percentage;

            expBar.value = percentage / 100;

            expTxt.text = ExppercentTxt.ToString("F2") + " %";

        }
    }

    public void ShowSettingPopup()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<SettingPopupUI>();
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
    }




}
