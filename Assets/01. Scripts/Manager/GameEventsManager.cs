using System.Collections.Generic;
using UnityEngine;

//�������� �̺�Ʈ�Լ��� ������ �� �ֵ��� ����� ��ũ��Ʈ
public class GameEventsManager : Singleton<GameEventsManager>
{
    public EnemyEvents enemyEvents;

    private void Awake()
    {
        enemyEvents = new EnemyEvents();
    }
}