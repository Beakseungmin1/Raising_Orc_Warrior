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

    // id�� �׻� ��ũ���ͺ� ������Ʈ ���°� �̸��� ���ƾ��Ѵ�.
    private void OnValidate()
    {
        #if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this); //����Ƽ���� �ش� ������ ��Ƽ�� �����Ѵ�.
        #endif
    }

}
