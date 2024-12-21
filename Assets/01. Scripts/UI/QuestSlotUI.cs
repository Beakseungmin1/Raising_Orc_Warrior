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
        //퀘스트 프리팹에 있는 스크립트 컴포넌트를 받아온 뒤, QuestStep을 상속받는 클래스를 제네릭으로 받아온 뒤 거기에 있는 값을 뭔줄 알고 넣어준담...?
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
                Debug.Log("반복퀘스트 미구현");
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
