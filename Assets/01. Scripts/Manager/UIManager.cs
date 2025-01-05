using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;

public class UIManager : Singleton<UIManager>
{
    private Dictionary<string, UIBase> uiDictionary = new Dictionary<string, UIBase>(); //�̹� ������ ����Ʈ
    public static float ScreenWidth = 1080;
    public static float ScreenHeight = 1920;

    private int currentSortingOrder = 0;

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
        // ��ųʸ����� UI�� �����ϴ��� Ȯ��
        if (uiDictionary.TryGetValue(uiName, out UIBase go))
        {
            uiDictionary.Remove(uiName); // ��ųʸ����� UI ����
            Destroy(go.canvas.gameObject); // UI ������Ʈ �ı�

            if (uiDictionary.Count > 0)
            {
                // ���� ���� sortingOrder�� ���� UI�� ã��
                currentSortingOrder = uiDictionary.Values
                    .Select(ui => ui.canvas.sortingOrder)
                    .Max();
            }
            else
            {
                currentSortingOrder = 0; // UI�� ������ �⺻ sortingOrder�� ����
            }
        }
    }
}