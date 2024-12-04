using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChapterSO", menuName = "ChapterSO", order = 1)]
public class ChapterSO : ScriptableObject
{
    public int chapterNum;
    public AudioClip chapterMusic;
    public StageSO[] stageSOs;

    [Header("BackGround")]
    public Sprite bgSprite; //¹è°æ
    public string chapterName;
}
