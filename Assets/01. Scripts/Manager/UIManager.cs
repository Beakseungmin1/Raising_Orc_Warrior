using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>(); //이미 생성된 리스트
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    public T Show<T>() where T : UIBase
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
            throw new System.Exception($"UI Resource not found: {uiName}"); // 로드 실패 처리
        }

        var ui = Load<T>(go, uiName); // 새 UI 생성
        uiDictionary[uiName] = ui; // 생성된 UI를 딕셔너리에 추가

        return (T)ui;
    }

    public T Show<T>(DungeonInfoSO dungeonInfoSO) where T : UIBase
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
            throw new System.Exception($"UI Resource not found: {uiName}"); // 로드 실패 처리
        }

        var ui = Load<T>(go, uiName); // 새 UI 생성
        uiDictionary[uiName] = ui; // 생성된 UI를 딕셔너리에 추가

        return (T)ui;
    }


    private T Load<T>(UIBase prefab, string uiName) where T : UIBase //조건문 같은 거라고 보면 된다. T는 UIBase이거나 UIBase를 상속받는 클래스로 제한한다.
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);
        canvasScaler.matchWidthOrHeight = 1;

        newCanvasObject.AddComponent<GraphicRaycaster>();

        UIBase ui = Instantiate(prefab, newCanvasObject.transform);
        ui.name = ui.name.Replace("(Clone)", "");
        ui.canvas = canvas;
        currentSortingOrder++;
        ui.canvas.sortingOrder = currentSortingOrder;
        return (T)ui;
    }

    public void Hide<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString();
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
                // 가장 높은 sortingOrder를 가진 UI를 찾음
                currentSortingOrder = uiDictionary.Values
                    .Select(ui => ui.canvas.sortingOrder)
                    .Max();
            }
            else
            {
                currentSortingOrder = 0; // UI가 없으면 기본 sortingOrder로 설정
            }
        }
    }
}