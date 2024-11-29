using UnityEngine;

public abstract class BaseItemDataSO : ScriptableObject
{
    [Header("Base Info")]
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public Sprite inGameImage; // �ΰ��� �̹���
    public Grade grade; // ���
}