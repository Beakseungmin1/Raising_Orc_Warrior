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
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>(); //�̹� ������ ����Ʈ
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

    private const int ReservedSortingOrder = 100; // Ư�� UI�� ���� sortingOrder ��

    private HashSet<string> reservedUISet = new HashSet<string>
    {
        "SummonPopupUI",
        "Main_DungeonUI",
        "EXPDungeonUI",
        "CubeDungeonUI",
        "GoldDungeonUI",
        "EXPDungeonUI_ConfirmEnterBtnPopUpUI",
        "CubeDungeonUI_ConfirmEnterBtnPopUpUI",
        "GoldDungeonUI_ConfirmEnterBtnPopUpUI"
    };

    public T Show<T>() where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T Ÿ�Կ� ���� UI �̸��� �����մϴ�.

        // ��ųʸ����� UI�� �̹� �����Ǿ����� Ȯ��
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            return (T)existingUI; // �̹� �����ϴ� UI �ν��Ͻ��� ��ȯ
        }

        // UI�� �������� ���� ��� ���� ����
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI ���ҽ��� �ε��մϴ�.
        if (go == null)
        {
            throw new System.Exception($"UI Resource not found: {uiName}"); // �ε� ���� ó��
        }

        var ui = Load<T>(go, uiName); // �� UI ����
        uiDictionary[uiName] = ui; // ������ UI�� ��ųʸ��� �߰�

        return (T)ui;
    }

    public T Show<T>(DungeonInfoSO dungeonInfoSO) where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T Ÿ�Կ� ���� UI �̸��� �����մϴ�.

        // ��ųʸ����� UI�� �̹� �����Ǿ����� Ȯ��
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            return (T)existingUI; // �̹� �����ϴ� UI �ν��Ͻ��� ��ȯ
        }

        // UI�� �������� ���� ��� ���� ����
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI ���ҽ��� �ε��մϴ�.
        if (go == null)
        {
            throw new System.Exception($"UI Resource not found: {uiName}"); // �ε� ���� ó��
        }

        var ui = Load<T>(go, uiName); // �� UI ����
        uiDictionary[uiName] = ui; // ������ UI�� ��ųʸ��� �߰�

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
            ui.canvas.sortingOrder = ReservedSortingOrder;
        }
        else
        {
            currentSortingOrder++;
            if (currentSortingOrder >= ReservedSortingOrder)
            {
                currentSortingOrder = ReservedSortingOrder - 1; // ReservedSortingOrder�� ���� �ʵ��� ����
            }
            ui.canvas.sortingOrder = currentSortingOrder;
        }

        return (T)ui;
    }


    public T ShowFadePanel<T>(FadeType fadeType) where T : UIBase
    {
        string uiName = typeof(T).ToString(); // T Ÿ�Կ� ���� UI �̸��� �����մϴ�.

        // ��ųʸ����� UI�� �̹� �����Ǿ����� Ȯ��
        if (uiDictionary.TryGetValue(uiName, out UIBase existingUI))
        {
            return (T)existingUI; // �̹� �����ϴ� UI �ν��Ͻ��� ��ȯ
        }

        // UI�� �������� ���� ��� ���� ����
        UIBase go = Resources.Load<UIBase>("UI/" + uiName); // UI ���ҽ��� �ε��մϴ�.
        if (go == null)
        {
            throw new System.Exception($"UI Resource not found: {uiName}"); // �ε� ���� ó��
        }

        var ui = LoadFadeController<T>(go, uiName, fadeType); // �� UI ����

        //uiDictionary[uiName] = ui; ��ųʸ��� �߰��ϸ� SortingOrder�� ��ȸ�ϱ� ������, �ֻ�ܿ� ��ġ�ϱ����� ��ųʸ��� ���� ����.
        //��� �׷��⶧���� HIde�ż��尡 ������ ����. Destroy�ؾ���.
        return (T)ui;
    }


    private T LoadFadeController<T>(UIBase prefab, string uiName, FadeType fadeType) where T : UIBase //���ǹ� ���� �Ŷ�� ���� �ȴ�. T�� UIBase�̰ų� UIBase�� ��ӹ޴� Ŭ������ �����Ѵ�.
    {
        GameObject newCanvasObject = new GameObject(uiName + "Canvas");

        var canvas = newCanvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        var canvasScaler = newCanvasObject.AddComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(ScreenWidth, ScreenHeight);
        canvasScaler.matchWidthOrHeight = 1;

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

    public void Hide(string uiName)
    {
        // ��ųʸ����� UI�� �����ϴ��� Ȯ��
        if (uiDictionary.TryGetValue(uiName, out UIBase go))
        {
            uiDictionary.Remove(uiName); // ��ųʸ����� UI ����
            Destroy(go.canvas.gameObject); // UI ������Ʈ �ı�

            if (uiDictionary.Count > 0)
            {
                // ���� ���� sortingOrder�� ���� UI�� ã�� (ReservedSortingOrder�� ����)
                currentSortingOrder = uiDictionary.Values
                    .Where(ui => ui.canvas.sortingOrder < ReservedSortingOrder)
                    .Select(ui => ui.canvas.sortingOrder)
                    .DefaultIfEmpty(0) // ��ųʸ��� ���� ������ �⺻�� 0
                    .Max();
            }
            else
            {
                currentSortingOrder = 0; // UI�� ������ �⺻ sortingOrder�� ����
            }
        }
    }
}