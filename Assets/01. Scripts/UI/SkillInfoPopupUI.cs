using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInfoPopupUI : UIBase
{
    [SerializeField] private TextMeshProUGUI skillNameTxt;
    [SerializeField] private TextMeshProUGUI skillDescription1Txt;
    [SerializeField] private TextMeshProUGUI skillDescription2Txt;
    [SerializeField] private Image skillImage;
    [SerializeField] private Button equipButton;
    [SerializeField] private Button exitButton;


    private Skill currentSkill;
    private SkillEquipSlotManager equipSlotManager;

    private void Start()
    {
        exitButton.onClick.AddListener(ExitBtn);
    }

    public void Initialize(SkillEquipSlotManager manager)
    {
        equipSlotManager = manager;
    }

    public void DisplaySkillDetails(Skill skill)
    {
        currentSkill = skill;

        skillNameTxt.text = skill.BaseData.itemName;
        skillDescription1Txt.text = skill.BaseData.description;
        skillDescription2Txt.text = skill.BaseData.description2;
        skillImage.sprite = skill.BaseData.icon;

        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(PrepareSkillForEquip);
    }

    private void PrepareSkillForEquip()
    {
        if (currentSkill == null) return;

        equipSlotManager.PrepareSkillForEquip(currentSkill);

        UIManager.Instance.Hide<SkillInfoPopupUI>();
        UIManager.Instance.Hide<DimmedUI>();
    }

    public void ExitBtn()
    {
        UIManager.Instance.Hide<DimmedUI>();
        Hide();
    }
}