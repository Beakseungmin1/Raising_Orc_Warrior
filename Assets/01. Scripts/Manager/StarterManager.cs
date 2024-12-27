using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class StarterManager : Singleton<StarterManager>
{
    private bool isInitialized = false;
    private GameObject eventSystemObject;

    void Start()
    {

        CreateInitialUI();
        CreateEventSystem();
        //임시
        SoundManager.Instance.Init();
        SoundManager.Instance.PlayBGM(BGMType.Title);

        OpenFirstDungeons();
        if (!isInitialized)
        {
            //InitializeGame();
        }
    }

    private void InitializeGame()
    {
        //SetUI(); //start���ٴ� ���⿡ �ִ� �� ����.
        //CreateEventSystem(); //start���ٴ� ���⿡ �ִ� �� ����.
        InitializeDataManager();
        InitializeGlobalSystems();
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

    private void CreateInitialUI()
    {
        UIManager.Instance.Show<HUDPanel>();
        UIManager.Instance.Show<StageInfoUI>();
        UIManager.Instance.Show<Main_PlayerUpgradeUI>();
        UIManager.Instance.Show<RightSideBtnsAreaUI>();
        UIManager.Instance.Show<MainButtonsUI>();
    }

    private void CreateEventSystem()
    {
        if (eventSystemObject == null)
        {
            eventSystemObject = new GameObject("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<StandaloneInputModule>();
        }
    }

    private void OpenFirstDungeons()
    {
        DungeonManager.Instance.ChangeDungeonState(DungeonType.CubeDungeon, 1, DungeonState.OPENED);
        DungeonManager.Instance.ChangeDungeonState(DungeonType.EXPDungeon, 1, DungeonState.OPENED);
        DungeonManager.Instance.ChangeDungeonState(DungeonType.GoldDungeon, 1, DungeonState.OPENED);
    }
}