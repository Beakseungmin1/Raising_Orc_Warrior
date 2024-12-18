using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlotUI : UIBase
{
    public Quest quest;
    
    public TextMeshProUGUI displayNameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI curAmountTxt;
    public TextMeshProUGUI amountToClearTxt;
    public TextMeshProUGUI rewardAmountTxt;
    public Image rewardImage;

    private void Awake()
    {
        QuestType questType = quest.info.questType;
        displayNameTxt.text = quest.info.displayName;
        levelTxt.text = quest.info.questLevel.ToString();
        //curAmountTxt = 
        //퀘스트 프리팹에 있는 스크립트 컴포넌트를 받아온 뒤, QuestStep을 상속받는 클래스를 제네릭으로 받아온 뒤 거기에 있는 값을 뭔줄 알고 넣어준담...?

    }

    public void OnRewardBtnClick()
    {
        GameEventsManager.Instance.questEvents.FinishQuest(quest.info.id);
        GameEventsManager.Instance.questEvents.StartQuest(quest.info.id);
    }
}
