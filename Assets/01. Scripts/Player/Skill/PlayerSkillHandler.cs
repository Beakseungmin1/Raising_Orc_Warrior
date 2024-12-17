using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillHandler : MonoBehaviour
{
    private List<BaseSkill> equippedSkills; // ������ ��ų ���
    private EquipManager equipManager;
    private PlayerStat playerStat;

    private void Start()
    {
        // EquipManager�� PlayerStat ��������
        equipManager = PlayerObjManager.Instance?.Player?.GetComponent<EquipManager>();
        playerStat = PlayerObjManager.Instance?.Player?.GetComponent<PlayerStat>();

        if (equipManager == null || playerStat == null)
        {
            Debug.LogError("PlayerSkillHandler: EquipManager�� PlayerStat�� ã�� �� �����ϴ�.");
            return;
        }

        SyncWithEquipManager();
    }

    private void Update()
    {
        UpdateSkills();
    }

    /// <summary>
    /// EquipManager�� ����ȭ�Ͽ� ������ ��ų ��� �ʱ�ȭ
    /// </summary>
    public void SyncWithEquipManager()
    {
        equippedSkills = equipManager.GetAllEquippedSkills();

        // �� ��ų �ʱ�ȭ
        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Initialize(skill.SkillData, playerStat);
        }
    }

    /// <summary>
    /// ��� ������ ��ų�� Update ����
    /// </summary>
    private void UpdateSkills()
    {
        foreach (var skill in equippedSkills)
        {
            if (skill != null)
                skill.Update();
        }
    }

    /// <summary>
    /// ��ų ��� (������ ��ų�� �ߵ� ����)
    /// </summary>
    public void UseSkill(BaseSkill skill, Vector3 targetPosition)
    {
        if (skill == null || !IsSkillEquipped(skill))
        {
            Debug.LogWarning($"{skill?.SkillData.itemName} ��ų�� �������� �ʾҰų� ��ȿ���� �ʽ��ϴ�.");
            return;
        }

        if (!skill.IsReadyToActivate())
        {
            Debug.LogWarning($"{skill.SkillData.itemName} ��ų�� ���� �غ���� �ʾҽ��ϴ�.");
            return;
        }

        // ��ų �ߵ�
        skill.Activate(targetPosition);
    }

    /// <summary>
    /// Ư�� ��ų�� ��Ʈ ���
    /// </summary>
    public void RegisterHit(BaseSkill skill)
    {
        if (skill == null || !IsSkillEquipped(skill))
        {
            Debug.LogWarning("�������� ���� ��ų���� ��Ʈ�� ����� �� �����ϴ�.");
            return;
        }

        skill.RegisterHit();
    }

    /// <summary>
    /// �ش� ��ų�� �����Ǿ����� Ȯ��
    /// </summary>
    private bool IsSkillEquipped(BaseSkill skill)
    {
        return equippedSkills.Contains(skill);
    }
}