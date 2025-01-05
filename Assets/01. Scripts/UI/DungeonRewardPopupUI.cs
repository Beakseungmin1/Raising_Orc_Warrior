using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Numerics;

public class DungeonRewardPopupUI : UIBase
{
    public DungeonInfoSO dungeonInfoSO;
    public BigInteger rewardAmount;

    public Image rewardIconImage;
    public TextMeshProUGUI rewardAmountTxt;

    public Sprite rewardTypeCube;
    public Sprite rewardTypeGold;
    public Sprite rewardTypeExp;

    public void SetUI(DungeonInfoSO infoSO, BigInteger lastRewardAmount)
    {
        dungeonInfoSO = infoSO;
        rewardAmount = lastRewardAmount;

        switch (dungeonInfoSO.type)
        {
            case(DungeonType.EXPDungeon):
                rewardIconImage.sprite = rewardTypeExp;
                break;
            case (DungeonType.CubeDungeon):
                rewardIconImage.sprite = rewardTypeCube;
                break;
            case (DungeonType.GoldDungeon):
                rewardIconImage.sprite= rewardTypeGold;
                break;
        }

        rewardAmountTxt.text = rewardAmount.ToString();
    }
}
