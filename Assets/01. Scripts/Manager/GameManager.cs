using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : Singleton<GameManager>
{
    private bool isGamePaused = false;

    private State currentGameState;

    [Header("Save Settings")]
    [SerializeField] private float saveInterval = 300f; // 5�и��� ������ ����
    private Coroutine autoSaveCoroutine;

    private void Start()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        // ������ �ʱ�ȭ (DataManager ���� �ּ� ó��)
        // DataManager.Instance.Initialize();

        // ��ġ ���� ���
        // CalculateIdleRewards();

        // ���� �ý��� �ʱ�ȭ
        InitializeGlobalSystems();

        // �ڵ� ���� ����
        StartAutoSave();

        // ���� ���� ����
        ChangeGameState(State.Playing);
    }

    private void InitializeGlobalSystems()
    {
        // �ٸ� �ý��� �ʱ�ȭ (��: AudioManager, UIManager ��)
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
        // ������ ���� (DataManager ���� �ּ� ó��)
        // DataManager.Instance.SaveData();
    }

    private void CalculateIdleRewards()
    {
        // �����Ϳ� ��ġ ���� ��� (DataManager ���� �ּ� ó��)
        /*
        var lastSaveTime = DataManager.Instance.GetLastSaveTime();
        var currentTime = System.DateTime.UtcNow;

        // ��ġ �ð� ���
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