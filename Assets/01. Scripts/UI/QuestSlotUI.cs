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
        //����Ʈ �����տ� �ִ� ��ũ��Ʈ ������Ʈ�� �޾ƿ� ��, QuestStep�� ��ӹ޴� Ŭ������ ���׸����� �޾ƿ� �� �ű⿡ �ִ� ���� ���� �˰� �־��ش�...?

    }

    public void OnRewardBtnClick()
    {
        GameEventsManager.Instance.questEvents.FinishQuest(quest.info.id);
        GameEventsManager.Instance.questEvents.StartQuest(quest.info.id);
    }
}
