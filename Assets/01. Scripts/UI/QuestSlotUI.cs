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

        if (questInfo != null)
        {
            displayNameTxt.text = questInfo.displayName;
            //levelTxt.text = 
            //curAmountTxt.text =
            //amountToClearTxt.text = questInfo.
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
}
