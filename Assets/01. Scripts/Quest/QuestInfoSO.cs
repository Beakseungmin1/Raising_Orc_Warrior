using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DefaultQuestSO", menuName = "QuestSO", order = 1)]
public class QuestInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("General")]
    public string displayName;
    public QuestType questType;

    [Header("Rewards")]
    public CurrencyType rewardType;
    public int rewardAmount;

    [Header("Steps")]
    public GameObject questStepPrefab;

    // id가 항상 스크립터블 오브젝트 에셋과 이름이 같아야한다.
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this); //유니티에서 해당 에셋을 더티로 설정한다.
        #endif
    }

}
