using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillDetailUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI skillDescription1Txt; // 첫 번째 설명
    [SerializeField] private TextMeshProUGUI skillDescription2Txt; // 두 번째 설명
    [SerializeField] private Image skillImage;
    [SerializeField] private Button equipButton;
    private SkillDataSO currentSkill;

    public void DisplaySkillDetails(SkillDataSO skill)
    {
        currentSkill = skill;

        // SO에서 데이터 불러오기
        skillNameTxt.text = skill.itemName;
        skillDescription1Txt.text = skill.description; // 첫 번째 설명
        skillDescription2Txt.text = skill.description2; // 두 번째 설명
        skillImage.sprite = skill.icon;

        // 버튼 이벤트 설정
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(SetSkillToEquip);
    }

    private void SetSkillToEquip()
    {
        // 스킬 장착 준비
        var skillSlotManager = FindObjectOfType<SkillSlotManager>();
        if (skillSlotManager != null)
        {
            skillSlotManager.PrepareSkillForEquip(currentSkill);
        }

        // UI 닫기
        UIManager.Instance.Hide("SkillInfoPopupUI");
    }
}