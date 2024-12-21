using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlotUI : UIBase
{
    public QuestInfoSO questInfo;

    public TextMeshProUGUI displayNameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI progressCountTxt;
    public TextMeshProUGUI rewardAmountTxt;
    public Image rewardImage;
    public Slider slider;

    public GameObject completeImage;

    private void Awake()
    {
        //����Ʈ �����տ� �ִ� ��ũ��Ʈ ������Ʈ�� �޾ƿ� ��, QuestStep�� ��ӹ޴� Ŭ������ ���׸����� �޾ƿ� �� �ű⿡ �ִ� ���� ���� �˰� �־��ش�...?
        GameEventsManager.Instance.questEvents.onQuestProgressCountChanged += RefreshUI;
        GameEventsManager.Instance.questEvents.onFinishQuest += SetCompleteUI;

        if (questInfo != null)
        {
            displayNameTxt.text = questInfo.displayName;
            rewardAmountTxt.text = questInfo.rewardAmount.ToString();
            rewardImage = questInfo.rewardImage;
        }
        completeImage.SetActive(false);
    }

    public void OnRewardBtnClick()
    {
        switch (questInfo.questType)
        {
            case QuestType.Daily:
                DailyQuestManager.Instance.FinishQuest(questInfo.id);
                break;
            case QuestType.Repeat:
                Debug.Log("�ݺ�����Ʈ �̱���");
                break;
            default: //QuestType.Achievement
                break;
        }
    }

    public void RefreshUI(string id, QuestState state, int currentProgressCount, int countToComplete, int level)
    {
        if (questInfo != null && questInfo.id == id)
        {
            progressCountTxt.text = $"{currentProgressCount} / {countToComplete}";
            slider.value = currentProgressCount / countToComplete;
            levelTxt.text = level.ToString();
        }
    }

    public void SetCompleteUI(string id)
    {
        if (questInfo != null && questInfo.id == id)
        {
            completeImage.SetActive(true);
        }
    }
}
