using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerObjManager : SingletonDontDestroy<PlayerObjManager>
{
    private Player _player;

    public Player Player
    {
        get { return _player; }
        set { _player = value; }
    }
}
