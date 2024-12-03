using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<GameObject> prefabList = new List<GameObject>(); //������

    [SerializeField] private Transform canvas;

    private List<GameObject> uiList = new List<GameObject>(); //�̹� ������ ����Ʈ



    public GameObject Show(string uiName)
    {
        GameObject go = prefabList.Find(obj => obj.name == uiName); //�̸��� ���ٸ� ��ȯ���ش�.

        GameObject ui = Instantiate(go, canvas);
        ui.name = ui.name.Replace("(Clone)", "");
        uiList.Add(ui); //���� ������ UI�� ������ ���Եȴ�.
        return ui;
    }

    public void Hide(string uiName)
    {
        GameObject go = uiList.Find(obj => obj.name == uiName);
        uiList.Remove(go);
        Destroy(go);
    }

}
