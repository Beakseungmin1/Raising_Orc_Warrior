using UnityEngine;

public abstract class BaseItemSO : ScriptableObject
{
    [Header("Base Info")]
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public Sprite inGameImage; // �ΰ��� �̹���
    public Grade grade; // ���
    public int currentSkillLevel; // ���� ��ȭ ����
    public int curStackAmount; // ���� ���� ����
}