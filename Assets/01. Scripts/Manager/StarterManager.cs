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
        // ������ �Ŵ��� �ʱ�ȭ �� ������ �ε�
        InitializeDataManager();

        // �ٸ� �ý��� �ʱ�ȭ
        InitializeGlobalSystems();

        // �ʱ� �� �ε�
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
        //��Ÿ �Ŵ��� �ϴ� �߰�
    }

    private void LoadInitialScene()
    {
        string initialSceneName = "MainMenu";
        SceneManager.LoadScene(initialSceneName);
    }
}