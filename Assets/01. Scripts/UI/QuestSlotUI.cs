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
    public GameObject dimmedImage;
    public Button questClearBtn;

    private void Awake()
    {
        //퀘스트 프리팹에 있는 스크립트 컴포넌트를 받아온 뒤, QuestStep을 상속받는 클래스를 제네릭으로 받아온 뒤 거기에 있는 값을 뭔줄 알고 넣어준담...?

        if (questInfo != null)
        {
            displayNameTxt.text = questInfo.displayName;
            rewardAmountTxt.text = questInfo.rewardAmount.ToString();
            rewardImage.sprite = questInfo.rewardImage;
        }

        dimmedImage.SetActive(true);
        questClearBtn.interactable = false;
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestProgressCountChanged += RefreshUI;
        GameEventsManager.Instance.questEvents.onFinishQuest += RefreshUI;
        //GameEventsManager.Instance.questEvents.onCompleteQuest += ManageCompleteUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestProgressCountChanged -= RefreshUI;
        GameEventsManager.Instance.questEvents.onFinishQuest -= RefreshUI;
        //GameEventsManager.Instance.questEvents.onCompleteQuest -= ManageCompleteUI;
    }

    private void Start()
    {
        if (questInfo != null)
        {
            RefreshUI(questInfo.id);
            ManageCompleteUI(questInfo.id);
        }
    }

    public void OnRewardBtnClick()
    {
        switch (questInfo.questType)
        {
            case QuestType.Daily:
                Debug.Log("데일리 퀘스트 미구현");
                break;
            case QuestType.Repeat:
                RepeatQuestManager.Instance.FinishQuest(questInfo.id);
                break;
            default: //QuestType.Achievement
                Debug.Log("업적퀘스트 미구현");
                break;
        }
    }

    public void RefreshUI(string id)
    {
        foreach (var obj in QuestManager.Instance.questGameObjs)
        {
            QuestStep questStep = obj.GetComponent<QuestStep>();

            if (questStep != null)
            {
                if (questStep.questId == questInfo.id)
                {
                    progressCountTxt.text = $"{questStep.count} / {questStep.countToComplete}";
                    slider.value = (float)questStep.count / questStep.countToComplete;
                    levelTxt.text = questStep.level.ToString();

                    Quest quest = QuestManager.Instance.GetQuestById(id);
                    if (quest.state.Equals(QuestState.CAN_FINISH) && quest.info.id == questInfo.id)
                    {
                        dimmedImage.SetActive(false);
                        questClearBtn.interactable = true;
                    }
                    else
                    {
                        dimmedImage.SetActive(true);
                        questClearBtn.interactable = false;
                    }
                }
            }
        }
    }

    public void ManageCompleteUI(string id)
    {
        foreach (var obj in QuestManager.Instance.questGameObjs)
        {
            QuestStep questStep = obj.GetComponent<QuestStep>();

            if (questStep != null)
            {
                if (questStep.questId == questInfo.id)
                {
                    Quest quest = QuestManager.Instance.GetQuestById(id);
                    if (quest.state.Equals(QuestState.FINISHED))
                    {
                        if (questInfo.id == id)
                        {
                            completeImage.SetActive(true);
                        }
                    }
                    else
                    {
                        completeImage.SetActive(false);
                    }
                }
            }
        }
    }
}
