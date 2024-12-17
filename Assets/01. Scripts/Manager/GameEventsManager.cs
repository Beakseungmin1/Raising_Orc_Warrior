using System.Collections.Generic;
using UnityEngine;

//�������� �̺�Ʈ�Լ��� ������ �� �ֵ��� ����� ��ũ��Ʈ
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