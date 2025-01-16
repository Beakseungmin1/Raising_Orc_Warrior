using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>(); //이미 생성된 리스트
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    private const int ReservedSortingOrder = 100; // 특정 UI의 고정 sortingOrder 값
    private const int PopupSortingOrder = 110;

    public UIBase currentMainUI = new UIBase();

    private HashSet<string> reservedUISet = new HashSet<string>
    {
        "EquipmentUpgradePopupUI",
        "DungeonRewardPopupUI",
        "EquipmentFusionPopupUI",
        "PlayerInfoPopupUI",
        "QuestPopupUI",
        "SettingPopupUI",
        "SkillInfoPopupUI",
        "SummonPopupUI",
        "Main_DungeonUI",
        "EXPDungeonUI",
        "CubeDungeonUI",
        "GoldDungeonUI"
    };

    private HashSet<string> dungeonPopupUISet = new HashSet<string>
    {
        "EXPDungeonUI_ConfirmEnterBtnPopUpUI",
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

        // 스크린 비율에 따라 UI 위치 조정
        RectTransform rectTransform = ui.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 기존 위치값 가져오기
            Vector2 currentPosition = rectTransform.anchoredPosition;

            // 스크린 비율 계산
            float targetAspect = 9f / 16f; // 목표 비율
            float screenAspect = (float)Screen.width / Screen.height;
            float scaleHeight = screenAspect / targetAspect; //목표비율 대비 얼마나 커졌는가.(크기가 얼마나 커졌는지 확인)

            if (scaleHeight < 1)
            {
                // 스크린이 길어졌을 때 - 기존 Y 위치에서 추가로 이동
                currentPosition.y -= (1f - scaleHeight) * Screen.height / 2f;
            }
            else
            {
                // 스크린이 짧아졌을 때 - 기존 위치 유지
                currentPosition.y += 0; // 필요 시 로직 추가 가능
            }

            // 조정된 위치를 다시 적용
            rectTransform.anchoredPosition = currentPosition;
        }
        else
        {
            Debug.LogWarning($"UI에 RectTransform이 없습니다: {uiName}");
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

        // 스크린 비율에 따라 UI 위치 조정
        RectTransform rectTransform = ui.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 기존 위치값 가져오기
            Vector2 currentPosition = rectTransform.anchoredPosition;

            // 스크린 비율 계산
            float targetAspect = 9f / 16f; // 목표 비율
            float screenAspect = (float)Screen.width / Screen.height;
            float scaleHeight = screenAspect / targetAspect; //목표비율 대비 얼마나 커졌는가.(크기가 얼마나 커졌는지 확인)

            if (scaleHeight < 1)
            {
                // 스크린이 길어졌을 때 - 기존 Y 위치에서 추가로 이동
                currentPosition.y -= (1f - scaleHeight) * Screen.height / 2f;
            }
            else
            {
                // 스크린이 짧아졌을 때 - 기존 위치 유지
                currentPosition.y += 0; // 필요 시 로직 추가 가능
            }

            // 조정된 위치를 다시 적용
            rectTransform.anchoredPosition = currentPosition;
        }
        else
        {
            Debug.LogWarning($"UI에 RectTransform이 없습니다: {uiName}");
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

        if (reservedUISet.Contains(uiName))
        {
            // 고정된 UI는 ReservedSortingOrder를 사용
            ui.canvas.sortingOrder = ReservedSortingOrder;
        }
        else if (dungeonPopupUISet.Contains(uiName))
        {
            // 팝업 UI는 PopupSortingOrder를 사용
            ui.canvas.sortingOrder = PopupSortingOrder;
        }
        else
        {
            // 일반 UI는 currentSortingOrder를 사용
            currentSortingOrder++;
            if (currentSortingOrder >= ReservedSortingOrder)
            {
                currentSortingOrder = ReservedSortingOrder - 1; // ReservedSortingOrder를 넘지 않도록 보장
            }
            ui.canvas.sortingOrder = currentSortingOrder;
        }

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

        // 스크린 비율에 따라 UI 위치 조정
        RectTransform rectTransform = ui.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            // 기존 위치값 가져오기
            Vector2 currentPosition = rectTransform.anchoredPosition;

            // 스크린 비율 계산
            float targetAspect = 9f / 16f; // 목표 비율
            float screenAspect = (float)Screen.width / Screen.height;
            float scaleHeight = screenAspect / targetAspect; //목표비율 대비 얼마나 커졌는가.(크기가 얼마나 커졌는지 확인)

            if (scaleHeight < 1)
            {
                // 스크린이 길어졌을 때 - 기존 Y 위치에서 추가로 이동
                currentPosition.y -= (1f - scaleHeight) * Screen.height / 2f;
            }
            else
            {
                // 스크린이 짧아졌을 때 - 기존 위치 유지
                currentPosition.y += 0; // 필요 시 로직 추가 가능
            }

            // 조정된 위치를 다시 적용
            rectTransform.anchoredPosition = currentPosition;
        }
        else
        {
            Debug.LogWarning($"UI에 RectTransform이 없습니다: {uiName}");
        }

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
        ui.canvas.sortingOrder = 99;

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
                    .Where(ui => ui.canvas.sortingOrder < ReservedSortingOrder)
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