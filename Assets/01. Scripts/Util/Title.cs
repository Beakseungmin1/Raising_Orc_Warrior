using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Title : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.Init();
        SoundManager.Instance.PlayBGM(BGMType.Title);
    }

}
