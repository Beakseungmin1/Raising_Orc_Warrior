using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestSlotUI : UIBase
{
    public TextMeshProUGUI displayNameTxt;
    public TextMeshProUGUI levelTxt;
    public TextMeshProUGUI curAmountTxt;
    public TextMeshProUGUI amountToClearTxt;
    public TextMeshProUGUI rewardAmountTxt;
    public Image rewardImage;

    private void Awake()
    {
        //curAmountTxt = 
        //����Ʈ �����տ� �ִ� ��ũ��Ʈ ������Ʈ�� �޾ƿ� ��, QuestStep�� ��ӹ޴� Ŭ������ ���׸����� �޾ƿ� �� �ű⿡ �ִ� ���� ���� �˰� �־��ش�...?

    }

    public void OnRewardBtnClick()
    {
    }
}
