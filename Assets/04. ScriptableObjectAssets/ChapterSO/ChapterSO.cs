using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChapterSO", menuName = "ChapterSO", order = 1)]
public class ChapterSO : ScriptableObject
{
    public int chapterNum;
    //public StageSO[] stageSOs;
    public AudioClip chapterMusic;

    [Header("BackGround")]
    public Sprite bgSprite; //¹è°æ
    public string chapterName;
}
