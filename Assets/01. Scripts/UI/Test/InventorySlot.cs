using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image skillIcon; // 스킬 아이콘
    [SerializeField] private TextMeshProUGUI curAmountTxt; // 현재 보유 중인 갯수
    [SerializeField] private TextMeshProUGUI maxAmountTxt; // 강화에 필요한 갯수
    [SerializeField] private TextMeshProUGUI nameTxt; // 스킬 이름
    [SerializeField] private Slider amountSlider; // 슬라이더 바
    [SerializeField] private Button slotButton; // 슬롯 하위의 버튼 직접 참조

    private SkillDataSO skillData;

    public void InitializeSlot(SkillDataSO skill, int currentAmount, int requiredAmount)
    {
        skillData = skill;

        if (skillData != null)
        {
            skillIcon.sprite = skillData.icon;
            nameTxt.text = skillData.itemName;
            curAmountTxt.text = currentAmount.ToString();
            maxAmountTxt.text = requiredAmount.ToString();
            amountSlider.value = (float)currentAmount / requiredAmount;

            slotButton.onClick.RemoveAllListeners();
            slotButton.onClick.AddListener(OnClickSlot);
        }
        else
        {
            skillIcon.sprite = null;
            nameTxt.text = "";
            curAmountTxt.text = "0";
            maxAmountTxt.text = "0";
            amountSlider.value = 0;

            slotButton.onClick.RemoveAllListeners();
        }
    }

    private void OnClickSlot()
    {
        // UIManager를 통해 SkillInfoPopupUI를 활성화하고, SkillDetailUI를 설정
        //var skillDetailUI = UIManager.Instance.Show("SkillInfoPopupUI").GetComponent<SkillDetailUI>();
        //if (skillDetailUI != null && skillData != null)
        //{
        //    skillDetailUI.DisplaySkillDetails(skillData);
        //}
    }
}