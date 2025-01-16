using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBtnUI : UIBase
{
    public GameObject redDot;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += RefreshRedDot;

        redDot.SetActive(QuestManager.Instance.GetIsAnyQuestCanFinish());
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange -= RefreshRedDot;
    }

    public void ShowQuestPopupUI()
    {
        UIManager.Instance.Show<DimmedUI>();
        UIManager.Instance.Show<QuestPopupUI>();
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
    }

    public void RefreshRedDot(Quest quest)
    {
        if (redDot != null)
        {
            redDot.SetActive(QuestManager.Instance.GetIsAnyQuestCanFinish());
        }
    }
}
