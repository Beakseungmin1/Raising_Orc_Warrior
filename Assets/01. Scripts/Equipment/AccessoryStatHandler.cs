using UnityEngine;
public class AccessoryStatHandler : MonoBehaviour
{
    //악세SO로부터 값을 받아와서 패시브로 마나,체력, 골드획득량을 높이는 스크립트
    private AccessoryDataSO accessoryDataSO;

    private void Awake()
    {
        // 1. 플레이어에 부착된 악세서리DataSO를 받아온다.
        // 이후에는 마나, 체력을 플레이어에게 적용한다.
        // 근데 이 스크립트가 필요한가? 그냥 한번에 플레이어 스탯쪽에서 처리해도 되지않나?
    }

}
