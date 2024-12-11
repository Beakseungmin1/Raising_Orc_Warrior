using UnityEngine;

public abstract class BaseItemDataSO : ScriptableObject
{
    [Header("Basic Information")]
    public string itemName; // ������ �̸�
    public Sprite icon; // ������ ������
    public Sprite inGameImage; // �ΰ��� �̹���
    public Grade grade; // ���
    public Color gradeColor; // ��޺� ����

    [Header("Upgrade Information")]
    public Sprite currencyIcon; // ��ȭ�� �ʿ��� ��ȭ ������
    public int requiredCurrencyForUpgrade; // ��ȭ�� �ʿ��� ��ȭ ����
    public int currentLevel; // ���� ����

    public override bool Equals(object obj)
    {
        if (obj is BaseItemDataSO other)
        {
            return ReferenceEquals(this, other) || itemName == other.itemName;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return itemName != null ? itemName.GetHashCode() : base.GetHashCode();
    }
}