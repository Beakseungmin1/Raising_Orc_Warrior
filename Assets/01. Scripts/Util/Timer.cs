using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float limitTime;
    [SerializeField] private float maxTime;

    bool isStopped = false;

    private void OnEnable()
    {
        GameEventsManager.Instance.bossEvents.onTimerStop += StopTimer;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.bossEvents.onTimerStop -= StopTimer;
    }

    public void SetLimitTime(float limitTime)
    {
        this.limitTime = limitTime;
    }

    private void FixedUpdate()
    {
        if (!isStopped)
        {
            GetLimitTime();

            if (GetLimitTime() <= 0)
            {
                if (DungeonManager.Instance.playerIsInDungeon) //带傈老 版快
                {
                    bool isCleared = false;
                    bool isPlayerDead = false;
                    GameEventsManager.Instance.dungeonEvents.PlayerDeadOrTimeEndInDungeon(isCleared, isPlayerDead);
                }
                else //焊胶老 版快
                {
                    StageManager.Instance.BackToLastStage();
                }
                Destroy(gameObject);
            }
        }
    }

    public float GetLimitTime()
    {
        if (!isStopped)
        {
            limitTime -= Time.deltaTime;
        }
        else
        {
            float savedlimitTime = limitTime;
            limitTime = savedlimitTime;
        }
        return limitTime;
    }

    public float GetMaxTime()
    {
        maxTime = limitTime;
        return maxTime;
    }

    private void StopTimer()
    {
        isStopped = true;
        GetLimitTime();
        Destroy(gameObject);
    }
}