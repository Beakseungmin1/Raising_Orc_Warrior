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

public enum Rank
{
    Rank4 = 4,
    Rank3 = 3,
    Rank2 = 2,
    Rank1 = 1
}

public enum SkillType
{
    Active, // �Ϲ� ��Ƽ�� ��ų
    Buff, // ���� ��ų
    Passive // �нú� ��ų
}

public enum EffectType
{
    OnPlayer,
    OnMapCenter,
    Projectile
}

public enum ActivationCondition
{
    Cooldown, // ��ٿ� ��� �ߵ�
    HitBased, // ���� Ƚ�� ��� �ߵ�
}

public enum CurrencyType
{
    Gold,
    Emerald,
    Cube,
    Diamond,
    DungeonTicket
}

public enum ItemType
{
    Weapon,
    Accessory,
    Skill
}

public enum QuestType
{
    Daily, //���� ����Ʈ
    Repeat, //�ݺ� ����Ʈ
    Achievement //����
}

public enum QuestState
{
    REQUIREMENTS_NOT_MET, //�䱸���� ������
    CAN_START, //���� ����
    IN_PROGRESS, //���� �� 
    CAN_FINISH, //�Ϸ� ����
    FINISHED //�Ϸ�
}

public enum StatType
{
    Attack,
    Health,
    HealthRegeneration,
    CriticalIncreaseDamage,
    CriticalProbability,
    BluecriticalIncreaseDamage, //ȸ���� �ϰ�
    BluecriticalProbability
}

public enum DungeonType
{
    GoldDungeon,
    CubeDungeon,
    EXPDungeon
}

public enum DungeonState
{
    CLOSED, //�䱸���� ������(���� �ȵ�)
    OPENED, //���� ����
    CLEARED //Ŭ����
}

public enum FadeType
{
    FadeIn, //����ȭ�鿡�� ���� �����
    FadeOut, //����ȭ�鿡�� ���� ��ο���
    FadeOutFadeIn //����ȭ�� -> ��ο��� -> �����
}