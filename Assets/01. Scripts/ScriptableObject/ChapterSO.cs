using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DefaultChapterSO", menuName = "ChapterSO", order = 1)]
public class ChapterSO : ScriptableObject
{
    public int chapterIndex;
    public AudioClip chapterMusic;
    public List<StageSO> stageSOs;
    public List<BossStageSO> bossStageSOs;

    [Header("BackGround")]
    public Sprite bgSprite; //¹è°æ
    public string chapterName;
}
