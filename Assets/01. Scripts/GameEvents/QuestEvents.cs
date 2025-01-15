using System;

public class QuestEvents
{
    public event Action<string> onStartQuest;

    public void StartQuest(string id)
    {
        if (onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest;

    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<string, QuestType> onFinishQuest;
    public void FinishQuest(string id, QuestType questType)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id, questType);
        }
    }

    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }

    public event Action<string, int, QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id, stepIndex, questStepState);
        }
    }

    public event Action<string> onQuestProgressCountChanged;
    
    public void QuestProgressCountChange(string id)
    {
        if (onQuestProgressCountChanged != null)
        {
            onQuestProgressCountChanged(id);
        }
    }

    public event Action<string> onFinishQuestStep;
    public void FinishQuestStep(string id)
    {
        if (onFinishQuestStep != null)
        {
            onFinishQuestStep(id);
        }
    }

    public event Action<string> onRestartQuestStep;
    public void RestartQuestStep(string id)
    {
        if (onRestartQuestStep != null)
        {
            onRestartQuestStep(id);
        }
    }
}
