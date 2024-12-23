using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private List<UIBase> uiList = new List<UIBase>(); //이미 생성된 리스트
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString(); //UIManager.Instance.Show<MainPopup>(); 으로 코드를 쓰면 MainPopup이 uiName으로 반영된다. T는 각종 클래스에 대응함.
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); //이름이 같다면 반환해준다.
        var ui = Load<T>(go, uiName);
        uiList.Add(ui); //씬에 생성된 UI의 정보를 갖게된다.

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
        UIBase go = uiList.Find(obj => obj.name == uiName);
        uiList.Remove(go);
        if (go != null)
        {
            uiList.Remove(go);
            Destroy(go.canvas.gameObject);

            if (uiList.Count > 0)
                currentSortingOrder = uiList[uiList.Count - 1].canvas.sortingOrder;
            else
                currentSortingOrder = 0;
        }
    }
}