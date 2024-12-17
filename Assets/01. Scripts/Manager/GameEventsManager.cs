using System.Collections.Generic;
using UnityEngine;

//전역에서 이벤트함수에 접근할 수 있도록 만드는 스크립트
public class GameEventsManager : Singleton<GameEventsManager>
{
    public EnemyEvents enemyEvents;
    
    public QuestEvents questEvents;

    public PlayerEvents playerEvents;

    private void Awake()
    {
        enemyEvents = new EnemyEvents();
        questEvents = new QuestEvents();
        playerEvents = new PlayerEvents();
    }
}