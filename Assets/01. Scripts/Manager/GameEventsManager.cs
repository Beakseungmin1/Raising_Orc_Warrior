using System.Collections.Generic;
using UnityEngine;

//전역에서 이벤트함수에 접근할 수 있도록 만드는 스크립트
public class GameEventsManager : Singleton<GameEventsManager>
{
    public EnemyEvents enemyEvents;
    
    public PlayerEvents playerEvents;

    public SummonEvents summonEvents;

    public QuestEvents questEvents;

    public CurrencyEvents currencyEvents;

    public BossEvents bossEvents;

    public StageEvents stageEvents;

    private void Awake()
    {
        enemyEvents = new EnemyEvents();
        playerEvents = new PlayerEvents();
        summonEvents = new SummonEvents();
        questEvents = new QuestEvents();
        currencyEvents = new CurrencyEvents();
        bossEvents = new BossEvents();
        stageEvents = new StageEvents();
    }
}