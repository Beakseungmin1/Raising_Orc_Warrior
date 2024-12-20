public enum State
{
    Idle,       // 대기 상태
    Playing,    // 게임 진행 중
    Paused,     // 일시정지
    GameOver,   // 게임 오버
    Syncing,    // 동기화 중 (추후 네트워크 추가 시 사용)
    Loading     // 로딩 중
}

public enum Grade
{
    Normal, //일반
    Uncommon, //희귀
    Rare, //레어
    Hero, //영웅
    Legendary, //전설
    Mythic, //신화
    Ultimate //불멸
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
    Active, // 일반 액티브 스킬
    Buff, // 버프 스킬
    Passive // 패시브 스킬
}

public enum EffectType
{
    OnPlayer,
    OnMapCenter,
    Projectile
}

public enum ActivationCondition
{
    Cooldown, // 쿨다운 기반 발동
    HitBased, // 공격 횟수 기반 발동
}

public enum CurrencyType
{
    Gold,
    Emerald,
    Cube,
    Diamond
}

public enum ItemType
{
    Weapon,
    Accessory,
    Skill
}

public enum QuestType
{
    Daily, //일일 퀘스트
    Repeat, //반복 퀘스트
    Achievement //업적
}

public enum QuestState
{
    REQUIREMENTS_NOT_MET, //요구조건 불충족
    CAN_START, //시작 가능
    IN_PROGRESS, //진행 중 
    CAN_FINISH, //완료 가능
    FINISHED //완료
}

public enum StatType
{
    Attack,
    Health,
    HealthRegeneration,
    CriticalIncreaseDamage,
    CriticalProbability,
    BluecriticalIncreaseDamage, //회심의 일격
    BluecriticalProbability
}