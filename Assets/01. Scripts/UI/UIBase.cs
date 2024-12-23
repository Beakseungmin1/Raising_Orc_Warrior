using UnityEngine;

public class UIBase : MonoBehaviour
{
    //UI생성시 애니메이션 재생이나 사운드가 공통적으로 필요한 경우 해당 이 클래스를 상속받아서 처리해주는 클래스

    public Canvas canvas;
    
    public void Hide()
    {
        UIManager.Instance.Hide(gameObject.name);
    }
}
