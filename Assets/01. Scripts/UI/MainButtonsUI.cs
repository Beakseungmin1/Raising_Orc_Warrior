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
        // ��ư�� �ؽ�Ʈ�� �����Ͽ� ��ųʸ��� ����
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
        if ( ������ �� �ְų�, + ������ �� �ִٸ� )
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
        //��ų���� ĭ�� �������� ��ų ���� ��ŭ ä���� ������ ��
    }
    */

    public void UpdateButtonColors()
    {
        // ���� Ȱ��ȭ�� UI �̸� ��������
        string currentUIName = UIManager.Instance.currentMainUI.GetType().Name;

        foreach (var kvp in buttonElements)
        {
            Image buttonImage = kvp.Value.Item1;
            TextMeshProUGUI buttonLabel = kvp.Value.Item2;

            if (kvp.Key == currentUIName) // currentMainUI�� �ش��ϴ� ��ư
            {
                buttonImage.color = HexToColor("FF8430"); // ��Ȳ��
                buttonLabel.color = Color.white; // ���
            }
            else // currentMainUI�� �ƴ� ��ư
            {
                buttonImage.color = HexToColor("2B2B2B"); // ��ȸ��
                buttonLabel.color = HexToColor("8C8C8C"); // �ణ ȸ��
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

        // ��ư ���� ������Ʈ
        UpdateButtonColors();
    }

    private Color HexToColor(string hex)
    {
        if (ColorUtility.TryParseHtmlString("#" + hex, out Color color))
        {
            return color; // ��ȯ ����
        }
        else
        {
            Debug.LogWarning($"Invalid hex color: {hex}");
            return Color.white; // �⺻������ ��� ��ȯ
        }
    }

    private void ShowOrHideRedDot()
    {
        // playerLevelUpRedDot.SetActive(false); //Awake������ �ѹ� falseó��, ���� ��� ���� �� �ٽ� true�� �� ����
        dungeonRedDot.SetActive(CurrencyManager.Instance.GetCurrency(CurrencyType.DungeonTicket) > 0);
        shopRedDot.SetActive(CurrencyManager.Instance.GetCurrency(CurrencyType.Diamond) >= 50); //50���� �ּ� ��ȯ ���̾Ƹ��

    }

}
