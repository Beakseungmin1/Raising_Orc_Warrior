using System;

public class StageEvents
{
    public event Action onStageChange;

    public void ChangeStage()
    {
        if (onStageChange != null)
        {
            onStageChange();
        }
    }

    public event Action onChapterChange;

    public void ChapterChange()
    {
        if (onChapterChange != null)
        {
            onChapterChange();
        }
    }
}