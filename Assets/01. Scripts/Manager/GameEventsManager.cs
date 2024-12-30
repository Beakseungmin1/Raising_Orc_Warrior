using System.Collections.Generic;
using UnityEngine;

//�������� �̺�Ʈ�Լ��� ������ �� �ֵ��� ����� ��ũ��Ʈ
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