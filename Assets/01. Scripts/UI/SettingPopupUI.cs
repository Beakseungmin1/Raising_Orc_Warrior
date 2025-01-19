using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingPopupUI : UIBase
{
    public Button ExitBtn;
    public Slider BgmSlider;
    public Slider SFXSlider;

    private void OnEnable()
    {
        BgmSlider.value = PlayerPrefs.GetFloat("BGM", 1);
        SFXSlider.value = PlayerPrefs.GetFloat("SFX", 1);
    }

    public void OnExitBtnClick()
    {

        Hide();
        SoundManager.Instance.PlaySFXOneShot(SFXType.Button);
    }

    public void OnBGMScrollbar()
    {
        SoundManager.Instance.SetVolume("BGM", BgmSlider.value);
    }

    public void OnSFXScrollbar()
    {
        SoundManager.Instance.SetVolume("SFX", SFXSlider.value);
    }

    public void OnMuteBGMButton()
    {
        SoundManager.Instance.MuteBGM();
    }

    public void OnMuteSFXButton()
    {
        SoundManager.Instance.MuteSFX();
    }

    public bool isBGMMute()
    {
        return BgmSlider.gameObject.activeSelf;
    }
}
