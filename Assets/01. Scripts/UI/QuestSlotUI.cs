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
        //����Ʈ �����տ� �ִ� ��ũ��Ʈ ������Ʈ�� �޾ƿ� ��, QuestStep�� ��ӹ޴� Ŭ������ ���׸����� �޾ƿ� �� �ű⿡ �ִ� ���� ���� �˰� �־��ش�...?

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
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestProgressCountChanged -= RefreshUI;
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
                Debug.Log("���ϸ� ����Ʈ �̱���");
                break;
            case QuestType.Repeat:
                QuestManager.Instance.FinishQuest(questInfo.id, questInfo.questType);
                break;
            default: //QuestType.Achievement
                Debug.Log("��������Ʈ �̱���");
                break;
        }
    }

    public void RefreshUI(string id)
    {
        // ������ GameObject���� QuestStep ������Ʈ�� ����
        QuestStep questStep = QuestManager.Instance.GetQuestStepObjById(id).GetComponent<QuestStep>();
        Debug.Log($"{questStep}: {questStep.count}");
        Debug.Log($"{questStep}: {questStep.countToComplete}");

        if (questStep != null)
        {
            // UI ������Ʈ
            if (questStep.questId == questInfo.id)
            {
                progressCountTxt.text = $"{questStep.count} / {questStep.countToComplete}";
                slider.value = (float)questStep.count / questStep.countToComplete;
                levelTxt.text = questStep.level.ToString();

                // Quest ���� Ȯ��
                Quest quest = QuestManager.Instance.GetQuestById(id);
                if (quest.state.Equals(QuestState.CAN_FINISH))
                {
                    if (questInfo.id == id)
                    {
                        dimmedImage.SetActive(false);
                        questClearBtn.interactable = true;
                    }
                }
                else
                {
                    if (questInfo.id == id)
                    {
                        dimmedImage.SetActive(true);
                        questClearBtn.interactable = false;
                    }
                }
            }
        }
    }
}
