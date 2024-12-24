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
                Debug.Log("[UIExternalClickDetector] EquipSlot UI가 클릭되었습니다. 대기 상태를 유지합니다.");
                return;
            }
        }

        if (equipManager.WaitingSkillForEquip != null)
        {
            equipManager.ClearWaitingSkillForEquip();
            Debug.Log("[UIExternalClickDetector] 다른 UI 또는 빈 공간 클릭으로 대기 상태가 초기화되었습니다.");
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