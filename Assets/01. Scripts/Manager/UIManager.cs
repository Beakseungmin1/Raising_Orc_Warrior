using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    private List<UIBase> uiList = new List<UIBase>(); //�̹� ������ ����Ʈ
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString(); //UIManager.Instance.Show<MainPopup>(); ���� �ڵ带 ���� MainPopup�� uiName���� �ݿ��ȴ�. T�� ���� Ŭ������ ������.
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); //�̸��� ���ٸ� ��ȯ���ش�.
        var ui = Load<T>(go, uiName);
        uiList.Add(ui); //���� ������ UI�� ������ ���Եȴ�.

        return (T)ui;
    }

    private T Load<T>(UIBase prefab, string uiName) where T : UIBase //���ǹ� ���� �Ŷ�� ���� �ȴ�. T�� UIBase�̰ų� UIBase�� ��ӹ޴� Ŭ������ �����Ѵ�.
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