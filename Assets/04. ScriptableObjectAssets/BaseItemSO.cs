using UnityEngine;

public abstract class BaseItemSO : ScriptableObject
{
    //����, �Ǽ�, ��ų

    [Header("Base Info")]
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public Sprite inGameImage; // �ΰ��� �̹���
    public Grade grade; // ���

}