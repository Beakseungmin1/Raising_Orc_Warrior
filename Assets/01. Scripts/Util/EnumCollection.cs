public enum State
{
    Idle,       // 대기 상태
    Playing,    // 게임 진행 중
    Paused,     // 일시정지
    GameOver,   // 게임 오버
    Syncing,    // 동기화 중 (추후 네트워크 추가 시 사용)
    Loading     // 로딩 중
}