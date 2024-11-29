public enum State
{
    Idle,       // ��� ����
    Playing,    // ���� ���� ��
    Paused,     // �Ͻ�����
    GameOver,   // ���� ����
    Syncing,    // ����ȭ �� (���� ��Ʈ��ũ �߰� �� ���)
    Loading     // �ε� ��
}

public enum Grade
{
    Normal, //�Ϲ�
    Uncommon, //���
    Rare, //����
    Hero, //����
    Legendary, //����
    Mythic, //��ȭ
    Ultimate //�Ҹ�
}

public enum SkillType
{
    Active, // �Ϲ� ��Ƽ�� ��ų
    Buff, // ���� ��ų
    Passive // �нú� ��ų
}

public enum ActivationCondition
{
    Cooldown, // ��ٿ� ��� �ߵ�
    HitBased // ���� Ƚ�� ��� �ߵ�
}

public enum CurrencyType
{
    Gold,
    Emerald,
    Cube,
    Diamond
}
