using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MainButtonsUI : UIBase
{
    public Image playerLevelupUIBtn;
    public Image skillUIBtn;
    public Image equipmentUIBtn;
    public Image dungeonUIBtn;
    public Image shopUIBtn;

    public TextMeshProUGUI playerLevelupUIBtnLabel;
    public TextMeshProUGUI skillUIBtnLabel;
    public TextMeshProUGUI equipmentUIBtnLabel;
    public TextMeshProUGUI dungeonUIBtnLabel;
    public TextMeshProUGUI shopUIBtnLabel;

    public GameObject playerLevelUpRedDot;
    public GameObject skillRedDot;
    public GameObject equipmentRedDot;
    public GameObject dungeonRedDot;
    public GameObject shopRedDot;

    private Dictionary<string, (Image, TextMeshProUGUI)> buttonElements;

    private void Awake()
    {
        // 버튼과 텍스트를 매핑하여 딕셔너리에 저장
        buttonElements = new Dictionary<string, (Image, TextMeshProUGUI)>
        {
            { "Main_PlayerUpgradeUI", (playerLevelupUIBtn, playerLevelupUIBtnLabel) },
            { "Main_SkillUI", (skillUIBtn, skillUIBtnLabel) },
            { "Main_EquipmentUI", (equipmentUIBtn, equipmentUIBtnLabel) },
            { "Main_DungeonUI", (dungeonUIBtn, dungeonUIBtnLabel) },
            { "Main_ShopUI", (shopUIBtn, shopUIBtnLabel) }
        };

        playerLevelUpRedDot.SetActive(false);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.dungeonEvents.onDungeonUIChanged += UpdateButtonColors;
        GameEventsManager.Instance.currencyEvents.onDungeonTicketChanged += ShowOrHideRedDot;
        GameEventsManager.Instance.currencyEvents.onDiamondChanged += ShowOrHideRedDot;
        UpdateButtonColors();
        ShowOrHideRedDot();
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.dungeonEvents.onDungeonUIChanged -= UpdateButtonColors;
        GameEventsManager.Instance.currencyEvents.onDiamondChanged -= ShowOrHideRedDot;
    }

    /*
    public void RefreshEquipmentRedDot()
    {
        if ( 융합할 게 있거나, + 장착할 게 있다면 )
        {
            dungeonRedDot.SetActive(true);
        }
        else
        {
            dungeonRedDot.SetActive(false);
        }
    }

    public void RefreshSkillRedDot()
    {
        //스킬슬롯 칸을 보유중인 스킬 개수 만큼 채우지 못했을 때
    }
    */

    public void UpdateButtonColors()
    {
        // 현재 활성화된 UI 이름 가져오기
        string currentUIName = UIManager.Instance.currentMainUI.GetType().Name;

        foreach (var kvp in buttonElements)
        {
            Image buttonImage = kvp.Value.Item1;
            TextMeshProUGUI buttonLabel = kvp.Value.Item2;

            if (kvp.Key == currentUIName) // currentMainUI에 해당하는 버튼
            {
                buttonImage.color = HexToColor("FF8430"); // 주황색
                buttonLabel.color = Color.white; // 흰색
            }
            else // currentMainUI가 아닌 버튼
            {
                buttonImage.color = HexToColor("2B2B2B"); // 진회색
                buttonLabel.color = HexToColor("8C8C8C"); // 약간 회색
            }
        }
    }

    public void ShowMainUI(int index)
    {
        switch (index)
        {
            case 0:
                UIManager.Instance.Hide(UIManager.Instance.currentMainUI);
                UIManager.Instance.Show<Main_PlayerUpgradeUI>();
                break;
            case 1:
                UIManager.Instance.Hide(UIManager.Instance.currentMainUI);
                UIManager.Instance.Show<Main_SkillUI>();
                break;
            case 2:
                UIManager.Instance.Hide(UIManager.Instance.currentMainUI);
                UIManager.Instance.Show<Main_EquipmentUI>();
                break;
            case 3:
                if (DungeonManager.Instance.isPlayerInDungeon || StageManager.Instance.isPlayerInBossStage)
                {
                    Debug.Log("isPlayerInDungeon:" + DungeonManager.Instance.isPlayerInDungeon);
                    Debug.Log("isPlayerInBossStage:" + StageManager.Instance.isPlayerInBossStage);
                    GameEventsManager.Instance.messageEvents.ShowMessage(MessageTextType.DungeonEntryBlocked, 0.4f, 100);
                }
                else
                {
                    UIManager.Instance.Hide(UIManager.Instance.currentMainUI);
                    UIManager.Instance.Show<Main_DungeonUI>();
                }
                break;
            case 4:
                UIManager.Instance.Hide(UIManager.Instance.currentMainUI);
                UIManager.Instance.Show<Main_ShopUI>();
                break;
        }

        // 버튼 색상 업데이트
        UpdateButtonColors();
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            return color; // 변환 성공
        }
        else
        {
            Debug.LogWarning($"Invalid hex color: {hex}");
            return Color.white; // 기본값으로 흰색 반환
        }
    }

    private void ShowOrHideRedDot()
    {
        // playerLevelUpRedDot.SetActive(false); //Awake딴에서 한번 false처리, 현재 기능 구현 상 다시 true될 일 없음
        dungeonRedDot.SetActive(CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) > 0);
        shopRedDot.SetActive(CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= 50); //50원은 최소 소환 다이아몬드

    }

}
