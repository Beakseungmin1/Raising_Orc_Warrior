using UnityEngine;
using UnityEngine.SceneManagement;

public class StarterManager : Singleton<StarterManager>
{
    private bool isInitialized = false;

    void Start()
    {
        if (!isInitialized)
        {
            InitializeGame();
        }
    }

    private void InitializeGame()
    {
        // 데이터 매니저 초기화 및 데이터 로드
        InitializeDataManager();

        // 다른 시스템 초기화
        InitializeGlobalSystems();

        // 초기 씬 로드
        LoadInitialScene();

        isInitialized = true;
    }

    private void InitializeDataManager()
    {
        //DataManager.Instance.Initialize();

        //if (DataManager.Instance.HasSaveData())
        //{
        //    DataManager.Instance.LoadData();
        //}
        //else
        //{
        //    DataManager.Instance.CreateNewData();
        //}
    }

    private void InitializeGlobalSystems()
    {
        //SoundManager.Instance.Initialize();
        //UIManager.Instance.InitializeUI();
        //기타 매니저 싹다 추가
    }

    private void LoadInitialScene()
    {
        string initialSceneName = "MainMenu";
        SceneManager.LoadScene(initialSceneName);
    }
}