using UnityEngine;

public class UIBase : MonoBehaviour
{
    //UI������ �ִϸ��̼� ����̳� ���尡 ���������� �ʿ��� ��� �ش� �� Ŭ������ ��ӹ޾Ƽ� ó�����ִ� Ŭ����

    public Canvas canvas;
    
    public void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }
}
