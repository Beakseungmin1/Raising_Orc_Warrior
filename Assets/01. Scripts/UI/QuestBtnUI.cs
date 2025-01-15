using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBtnUI : UIBase
{
    public Animation redDotAnim;
    public Image redDotImage;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestStateChange += RefreshRedDot;

        redDotImage.enabled = QuestManager.Instance.GetIsAnyQuestCanFinish();
        redDotAnim.enabled = QuestManager.Instance.GetIsAnyQuestCanFinish();
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
        if (redDotImage != null && redDotAnim != null)
        {
            redDotImage.enabled = QuestManager.Instance.GetIsAnyQuestCanFinish();
            redDotAnim.enabled = QuestManager.Instance.GetIsAnyQuestCanFinish();
        }
    }
}
