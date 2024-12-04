using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image skillIcon; // ��ų ������
    [SerializeField] private TextMeshProUGUI curAmountTxt; // ���� ���� ���� ����
    [SerializeField] private TextMeshProUGUI maxAmountTxt; // ��ȭ�� �ʿ��� ����
    [SerializeField] private TextMeshProUGUI nameTxt; // ��ų �̸�
    [SerializeField] private Slider amountSlider; // �����̴� ��
    [SerializeField] private Button slotButton; // ���� ������ ��ư ���� ����

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
        // UIManager�� ���� SkillInfoPopupUI�� Ȱ��ȭ�ϰ�, SkillDetailUI�� ����
        //var skillDetailUI = UIManager.Instance.Show("SkillInfoPopupUI").GetComponent<SkillDetailUI>();
        //if (skillDetailUI != null && skillData != null)
        //{
        //    skillDetailUI.DisplaySkillDetails(skillData);
        //}
    }
}