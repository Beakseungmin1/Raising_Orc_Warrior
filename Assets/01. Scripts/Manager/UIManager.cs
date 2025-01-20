using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>(); //이미 생성된 리스트
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    private const int depthOneSortingOrder = 110; // 특정 UI의 고정 sortingOrder 값
    private const int depthTwoSortingOrder = 120;
    private const int depthThreeSortingOrder = 130;
    private const int depthFourSortingOrder = 140;

    public UIBase currentMainUI = new UIBase();

    private HashSet<string> depthOneUIset = new HashSet<string> //110
    {
        "Main_DungeonUI",
    };

    private HashSet<string> depthTwoUIset = new HashSet<string> //120
    {
        "MainButtonsUI",
    };

    private HashSet<string> depthThreeUIset = new HashSet<string> //130
    {
        "EmeraldDungeonUI",
        "CubeDungeonUI",
        "GoldDungeonUI",
        "EquipmentUpgradePopupUI",
        "DungeonRewardPopupUI",
        "EquipmentFusionPopupUI",
        "PlayerInfoPopupUI",
        "QuestPopupUI",
        "SettingPopupUI",
        "SkillInfoPopupUI",
        "SummonPopupUI",
    };

    private HashSet<string> depthFourUIset = new HashSet<string> //140
    {
        "EmeraldDungeonUI_ConfirmEnterBtnPopUpUI",
        "CubeDungeonUI_ConfirmEnterBtnPopUpUI",
        "GoldDungeonUI_ConfirmEnterBtnPopUpUI"
    };

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T 타입에 따라 UI 이름을 결정합니다.

        // 딕셔너리에서 UI가 이미 생성되었는지 확인
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
            if (uiName.StartsWith("Main_"))
            {
                currentMainUI = existingUI;
            }

            return (T)existingUI; // 이미 존재하는 UI 인스턴스를 반환
        }

        // UI가 존재하지 않을 경우 새로 생성
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI 리소스를 로드합니다.
        if (go == null)
        {
            throw new System.Exception($"UI 리소스를 찾을 수 없습니다: {uiName}"); // 로드 실패 처리
        }

        var ui = Load<T>(go, uiName); // 새 UI 생성
        uiDictionary[uiName] = ui; // 생성된 UI를 딕셔너리에 추가

        // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
        if (uiName.StartsWith("Main_"))
        {
            currentMainUI = ui;
        }

        return (T)ui;
    }


    public T Show<T>(DungeonInfoSO dungeonInfoSO) where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T 타입에 따라 UI 이름을 결정합니다.

        // 딕셔너리에서 UI가 이미 생성되었는지 확인
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
            if (uiName.StartsWith("Main_"))
            {
                currentMainUI = existingUI;
            }

            return (T)existingUI; // 이미 존재하는 UI 인스턴스를 반환
        }

        // UI가 존재하지 않을 경우 새로 생성
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI 리소스를 로드합니다.
        if (go == null)
        {
            throw new System.Exception($"UI 리소스를 찾을 수 없습니다: {uiName}"); // 로드 실패 처리
        }

        var ui = Load<T>(go, uiName); // 새 UI 생성
        uiDictionary[uiName] = ui; // 생성된 UI를 딕셔너리에 추가

        // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
        if (uiName.StartsWith("Main_"))
        {
            currentMainUI = ui;
        }

        return (T)ui;
    }


    private T Load<T>(UIBase prefab, string uiName) where T : UIBase
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referencePixelsPerUnit = 100;

        newCanvasObject.AddComponent<GraphicRaycaster>();

        UIBase ui = Instantiate(prefab, newCanvasObject.transform);
        ui.name = ui.name.Replace("(Clone)", "");
        ui.canvas = canvas;

        if (depthOneUIset.Contains(uiName))
        {
            // 고정된 UI는 ReservedSortingOrder를 사용
            ui.canvas.sortingOrder = depthOneSortingOrder;
        }
        else if (depthTwoUIset.Contains(uiName))
        {
            ui.canvas.sortingOrder = depthTwoSortingOrder;
        }
        else if (depthThreeUIset.Contains(uiName))
        {
            // 고정된 UI는 ReservedSortingOrder를 사용
            ui.canvas.sortingOrder = depthThreeSortingOrder;
        }
        else if (depthFourUIset.Contains(uiName))
        {
            // 팝업 UI는 PopupSortingOrder를 사용
            ui.canvas.sortingOrder = depthFourSortingOrder;
        }
        else
        {
            // 일반 UI는 currentSortingOrder를 사용
            currentSortingOrder++;
            if (currentSortingOrder >= depthOneSortingOrder)
            {
                currentSortingOrder = depthOneSortingOrder - 1; // DepthOneSortingOrder을 넘지 않도록 보장
            }
            ui.canvas.sortingOrder = currentSortingOrder;
        }

        return (T)ui;
    }


    //sortOrder 임의로 설정할 수 있는 매서드
    public T Show<T>(int sortOrder) where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T 타입에 따라 UI 이름을 결정합니다.

        // 딕셔너리에서 UI가 이미 생성되었는지 확인
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
            if (uiName.StartsWith("Main_"))
            {
                currentMainUI = existingUI;
            }

            return (T)existingUI; // 이미 존재하는 UI 인스턴스를 반환
        }

        // UI가 존재하지 않을 경우 새로 생성
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI 리소스를 로드합니다.
        if (go == null)
        {
            throw new System.Exception($"UI 리소스를 찾을 수 없습니다: {uiName}"); // 로드 실패 처리
        }

        var ui = Load<T>(go, uiName, sortOrder); // 새 UI 생성
        uiDictionary[uiName] = ui; // 생성된 UI를 딕셔너리에 추가

        // Key가 "Main_"으로 시작하는 경우 currentMainUI에 할당
        if (uiName.StartsWith("Main_"))
        {
            currentMainUI = ui;
        }

        return (T)ui;
    }

    //sortOrder 임의로 설정할 수 있는 매서드
    private T Load<T>(UIBase prefab, string uiName, int sortOrder) where T : UIBase
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referencePixelsPerUnit = 100;

        newCanvasObject.AddComponent<GraphicRaycaster>();

        UIBase ui = Instantiate(prefab, newCanvasObject.transform);
        ui.name = ui.name.Replace("(Clone)", "");
        ui.canvas = canvas;

        ui.canvas.sortingOrder = sortOrder;

        return (T)ui;
    }

    public T ShowFadePanel<T>(FadeType fadeType) where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T 타입에 따라 UI 이름을 결정합니다.

        // 딕셔너리에서 UI가 이미 생성되었는지 확인
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            return (T)existingUI; // 이미 존재하는 UI 인스턴스를 반환
        }

        // UI가 존재하지 않을 경우 새로 생성
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI 리소스를 로드합니다.
        if (go == null)
        {
            throw new System.Exception($"UI 리소스를 찾을 수 없습니다: {uiName}"); // 로드 실패 처리
        }

        var ui = LoadFadeController<T>(go, uiName, fadeType); // 새 UI 생성

        return (T)ui;
    }


    private T LoadFadeController<T>(UIBase prefab, string uiName, FadeType fadeType) where T : UIBase //조건문 같은 거라고 보면 된다. T는 UIBase이거나 UIBase를 상속받는 클래스로 제한한다.
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);
        canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
        canvasScaler.referencePixelsPerUnit = 100;

        newCanvasObject.AddComponent<GraphicRaycaster>();

        FadeController fadeController = newCanvasObject.AddComponent<FadeController>();

        UIBase ui = Instantiate(prefab, newCanvasObject.transform);
        ui.name = ui.name.Replace("(Clone)", "");
        ui.canvas = canvas;
        ui.canvas.sortingOrder = depthOneSortingOrder - 1; 

        fadeController.panel = ui.gameObject;

        switch (fadeType)
        {
            case (FadeType.FadeIn):
                fadeController.isFadeIn = true;
                break;
            case (FadeType.FadeOut):
                fadeController.isFadeOut = true;
                break;
            case (FadeType.FadeOutFadeIn):
                fadeController.isFadeOutFadeIn = true;
                break;
        }

        return (T)ui;
    }

    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();
        Hide(uiName);
    }

    public void Hide(UIBase uiBase)
    {
        // 매개변수로 전달된 객체의 타입 이름을 가져옴
        string uiName = uiBase.GetType().ToString();
        Hide(uiName);
    }

    public void Hide(string uiName)
    {
        // 딕셔너리에서 UI가 존재하는지 확인
        if (uiDictionary.TryGetValue(uiName, out UIBase go))
        {
            uiDictionary.Remove(uiName); // 딕셔너리에서 UI 제거
            Destroy(go.canvas.gameObject); // UI 오브젝트 파괴

            if (uiDictionary.Count > 0)
            {
                // 가장 높은 sortingOrder를 가진 UI를 찾음 (ReservedSortingOrder와 PopupSortingOrder는 제외)
                currentSortingOrder = uiDictionary.Values
                    .Where(ui => ui.canvas.sortingOrder < depthOneSortingOrder)
                    .Select(ui => ui.canvas.sortingOrder)
                    .DefaultIfEmpty(0) // 딕셔너리에 값이 없으면 기본값 0
                    .Max();
            }
            else
            {
                currentSortingOrder = 0; // UI가 없으면 기본 sortingOrder로 설정
            }
        }
    }
}