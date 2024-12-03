using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{

    [SerializeField] private List<GameObject> prefabList = new List<GameObject>(); //프리팹

    [SerializeField] private Transform canvas;

    private List<GameObject> uiList = new List<GameObject>(); //이미 생성된 리스트



    public GameObject Show(string uiName)
    {
        GameObject go = prefabList.Find(obj => obj.name == uiName); //이름이 같다면 반환해준다.

        GameObject ui = Instantiate(go, canvas);
        ui.name = ui.name.Replace("(Clone)", "");
        uiList.Add(ui); //씬에 생성된 UI의 정보를 갖게된다.
        return ui;
    }

    public void Hide(string uiName)
    {
        GameObject go = uiList.Find(obj => obj.name == uiName);
        uiList.Remove(go);
        Destroy(go);
    }

}
