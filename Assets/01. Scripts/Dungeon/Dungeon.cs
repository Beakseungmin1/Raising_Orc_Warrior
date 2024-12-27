using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon
{
    public DungeonInfoSO info;
    public DungeonState state;
    public DungeonType type;

    public Dungeon(DungeonInfoSO dungeonInfo)
    {
        this.info = dungeonInfo;
        this.state = DungeonState.CLOSED;
        this.type = dungeonInfo.type;
    }
}
