using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDetailUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI skillDescription1Txt; // ù ��° ����
    [SerializeField] private TextMeshProUGUI skillDescription2Txt; // �� ��° ����
    [SerializeField] private Image skillImage;
    [SerializeField] private Button equipButton;
    private SkillDataSO currentSkill;

    public void DisplaySkillDetails(SkillDataSO skill)
    {
        currentSkill = skill;

        // SO���� ������ �ҷ�����
        skillNameTxt.text = skill.itemName;
        skillDescription1Txt.text = skill.description; // ù ��° ����
        skillDescription2Txt.text = skill.description2; // �� ��° ����
        skillImage.sprite = skill.icon;

        // ��ư �̺�Ʈ ����
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(SetSkillToEquip);
    }

    private void SetSkillToEquip()
    {
        // ��ų ���� �غ�
        var skillSlotManager = FindObjectOfType<SkillSlotManager>();
        if (skillSlotManager != null)
        {
            skillSlotManager.PrepareSkillForEquip(currentSkill);
        }

        // UI �ݱ�
        UIManager.Instance.Hide("SkillInfoPopupUI");
    }
}