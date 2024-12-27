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
        OpenFirstDungeons();
        if (!isInitialized)
        {
            //InitializeGame();
        }
    }

    private void InitializeGame()
    {
        //SetUI(); //start보다는 여기에 있는 게 나음.
        //CreateEventSystem(); //start보다는 여기에 있는 게 나음.
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
        //기타 매니저 싹다 추가
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