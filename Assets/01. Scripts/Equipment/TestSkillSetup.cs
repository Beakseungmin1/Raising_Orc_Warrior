using UnityEngine;

public class TestSkillSetup : MonoBehaviour
{
    public SkillDataSO skill1; // Unity 인스펙터에서 설정
    public SkillDataSO skill2;
    public SkillDataSO skill3;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        // 스킬 추가
        AddTestSkills();
    }

    private void AddTestSkills()
    {
        playerInventory.AddItemToInventory(skill1);
        playerInventory.AddItemToInventory(skill2);
        playerInventory.AddItemToInventory(skill3);

        Debug.Log("테스트 스킬이 인벤토리에 추가되었습니다.");
    }
}