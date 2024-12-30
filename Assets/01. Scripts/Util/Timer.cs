using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] private float limitTime;
    [SerializeField] private float maxTime;

    public void SetLimitTime(float limitTime)
    {
        this.limitTime = limitTime;
    }

    private void FixedUpdate()
    {
        GetLimitTime();

        if (GetLimitTime() <= 0)
        {
            StageManager.Instance.BackToLastStage();
            Destroy(gameObject);
        }
    }

    public float GetLimitTime()
    {
        limitTime -= Time.deltaTime;
        return limitTime;
    }

    public float GetMaxTime()
    {
        maxTime = limitTime;
        return maxTime;
    }
}