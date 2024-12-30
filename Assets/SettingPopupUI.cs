using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPopupUI : UIBase
{
    public Button ExitBtn;
    public Slider BgmSlider;
    public Slider SFXSlider;

    public void OnExitBtnClick()
    {
        Hide();
        UIManager.Instance.Hide<DimmedUI>();
    }
}
