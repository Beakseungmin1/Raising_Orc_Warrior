using UnityEngine;
using UnityEngine.EventSystems;

public class UIExternalClickDetector : MonoBehaviour
{
    private EquipManager equipManager;

    private void Start()
    {
        equipManager = PlayerObjManager.Instance.Player.EquipManager;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            HandleClick(Input.mousePosition);
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            HandleClick(Input.GetTouch(0).position);
        }
    }

    private void HandleClick(Vector3 position)
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            if (IsEquipSlotUI())
            {
                Debug.Log("[UIExternalClickDetector] EquipSlot UI�� Ŭ���Ǿ����ϴ�. ��� ���¸� �����մϴ�.");
                return;
            }
        }

        if (equipManager.WaitingSkillForEquip != null)
        {
            equipManager.ClearWaitingSkillForEquip();
            Debug.Log("[UIExternalClickDetector] �ٸ� UI �Ǵ� �� ���� Ŭ������ ��� ���°� �ʱ�ȭ�Ǿ����ϴ�.");
        }
    }

    private bool IsEquipSlotUI()
    {
        GameObject selectedObject = EventSystem.current.currentSelectedGameObject;

        if (selectedObject != null)
        {
            return selectedObject.GetComponentInParent<SkillEquipSlot>() != null;
        }

        return false;
    }
}