using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    private bool isGamePaused = false;

    private State currentGameState;

    [Header("Save Settings")]
    [SerializeField] private float saveInterval = 300f; // 5분마다 데이터 저장
    private Coroutine autoSaveCoroutine;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // 데이터 초기화 (DataManager 관련 주석 처리)
        // DataManager.Instance.Initialize();

        // 방치 보상 계산
        // CalculateIdleRewards();

        // 전역 시스템 초기화
        InitializeGlobalSystems();

        // 자동 저장 시작
        StartAutoSave();

        // 게임 상태 설정
        ChangeGameState(State.Playing);
    }

    private void InitializeGlobalSystems()
    {
        // 다른 시스템 초기화 (예: AudioManager, UIManager 등)
        //UIManager.Instance.InitializeUI();
        //SoundManager.Instance.Initialize();
    }

    private void StartAutoSave()
    {
        if (autoSaveCoroutine != null)
            StopCoroutine(autoSaveCoroutine);

        autoSaveCoroutine = StartCoroutine(AutoSaveRoutine());
    }

    private IEnumerator AutoSaveRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(saveInterval);
            SaveGame();
        }
    }

    public void PauseGame()
    {
        if (isGamePaused) return;

        isGamePaused = true;
        Time.timeScale = 0f;
        ChangeGameState(State.Paused);
    }

    public void ResumeGame()
    {
        if (!isGamePaused) return;

        isGamePaused = false;
        Time.timeScale = 1f;
        ChangeGameState(State.Playing);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        SaveGame();
        Application.Quit();
    }

    private void SaveGame()
    {
        // 데이터 저장 (DataManager 관련 주석 처리)
        // DataManager.Instance.SaveData();
    }

    private void CalculateIdleRewards()
    {
        // 데이터와 방치 보상 계산 (DataManager 관련 주석 처리)
        /*
        var lastSaveTime = DataManager.Instance.GetLastSaveTime();
        var currentTime = System.DateTime.UtcNow;

        // 방치 시간 계산
        var idleTime = (currentTime - lastSaveTime).TotalSeconds;

        if (idleTime > 0)
        {
            int idleGold = Mathf.FloorToInt((float)idleTime / 60f * DataManager.Instance.GetIdleGoldPerMinute());
            DataManager.Instance.AddGold(idleGold);

        }
        */
    }

    private void ChangeGameState(State newState)
    {
        currentGameState = newState;
    }

    public State GetCurrentGameState()
    {
        return currentGameState;
    }
}