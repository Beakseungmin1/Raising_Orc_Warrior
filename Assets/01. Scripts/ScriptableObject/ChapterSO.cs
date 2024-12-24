using UnityEngine;

[CreateAssetMenu(fileName = "DefaultChapterSO", menuName = "ChapterSO", order = 1)]
public class ChapterSO : ScriptableObject
{
    public int chapterIndex;
    public AudioClip chapterMusic;
    public StageSO[] stageSOs;
    public BossStageSO bossStageSO;

    [Header("BackGround")]
    public Sprite bgSprite; //���
    public string chapterName;
}
