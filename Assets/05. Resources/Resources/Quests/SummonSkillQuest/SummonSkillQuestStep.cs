using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonSkillQuestStep : QuestStep
{
    public QuestInfoSO questInfo;

    private int level = 1;
    private int summonCount = 0;
    private int summonCountToComplete = 10;

    private void OnEnable()
    {
        GameEventsManager.Instance.summonEvents.onSkillSummoned += SkillSummoned;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.summonEvents.onSkillSummoned -= SkillSummoned;
    }

    private void Start()
    {
        ProressCountChanged(); //����Ʈ����UI �������� �����ֱ� ����.
    }

    public void SkillSummoned(int count)
    {
        if (summonCount < summonCountToComplete)
        {
            summonCount += count;
            UpdateState();
            ProressCountChanged();
        }

        if (summonCount >= summonCountToComplete)
        {
            FinishQuestStep();
            ProressCountChanged();
        }
    }

    private void UpdateState()
    {
        string state = summonCount.ToString();
        ChangeState(state);
    }

    protected override void SetQuestStepState(string state)
    {
        this.summonCount = System.Int32.Parse(state);
        UpdateState();
    }

    public void ProressCountChanged()
    {
        Quest quest = QuestManager.Instance.GetQuestById(questInfo.id);
        GameEventsManager.Instance.questEvents.QuestProgressCountChange(questInfo.id, quest.state, summonCount, summonCountToComplete, level);
    }
}
