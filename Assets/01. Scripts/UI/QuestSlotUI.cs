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
        GameEventsManager.Instance.questEvents.onFinishQuest += ShowCompleteUI;

        if (questInfo != null)
        {
            displayNameTxt.text = questInfo.displayName;
            rewardAmountTxt.text = questInfo.rewardAmount.ToString();
            rewardImage = questInfo.rewardImage;
        }
    }

    private void Start()
    {
        if (questInfo != null)
        {
            RefreshUI(questInfo.id);
        }
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

    public void RefreshUI(string id)
    {
        foreach (var obj in QuestManager.Instance.questGameObjs)
        {
            QuestStep questStep = obj.GetComponent<QuestStep>();

            if (questStep != null && questStep.questId == questInfo.id)
            {
                progressCountTxt.text = $"{questStep.count} / {questStep.countToComplete}";
                slider.value = (float)questStep.count / questStep.countToComplete;
                levelTxt.text = questStep.level.ToString();
            }
        }

        Quest quest = QuestManager.Instance.GetQuestById(id);
        if (quest.state.Equals(QuestState.FINISHED))
        {
            ShowCompleteUI(id);
        }
        else
        {
            HideCompleteUI(id);
        }
    }


    public void ShowCompleteUI(string id)
    {
        if (questInfo.id == id && completeImage != null)
        {
            completeImage.SetActive(true);
        }
    }

    public void HideCompleteUI(string id)
    {
        completeImage.SetActive(false);
    }

}
