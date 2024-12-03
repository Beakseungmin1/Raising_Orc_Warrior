using UnityEngine;

public class TestSkillSetup : MonoBehaviour
{
    public SkillDataSO skill1; // Unity �ν����Ϳ��� ����
    public SkillDataSO skill2;
    public SkillDataSO skill3;

    private PlayerInventory playerInventory;

    private void Start()
    {
        playerInventory = PlayerobjManager.Instance.Player.inventory;
        if (playerInventory == null)
        {
            Debug.LogError("PlayerInventory�� ã�� �� �����ϴ�.");
            return;
        }

        // ��ų �߰�
        AddTestSkills();
    }

    private void AddTestSkills()
    {
        playerInventory.AddItemToInventory(skill1);
        playerInventory.AddItemToInventory(skill2);
        playerInventory.AddItemToInventory(skill3);

        Debug.Log("�׽�Ʈ ��ų�� �κ��丮�� �߰��Ǿ����ϴ�.");
    }
}