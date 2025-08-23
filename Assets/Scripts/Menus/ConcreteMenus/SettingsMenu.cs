using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : BaseMenu
{
    public AudioMixer mixer;

    public Button backBtn;
    public Button creditsBtn;

    public Slider masterVolSlider;
    public TMP_Text masterVolText;

    public Slider musicVolSlider;
    public TMP_Text musicVolText;

    public Slider sfxVolSlider;
    public TMP_Text sfxVolText;

    public override void Init(MenuController context)
    {
        base.Init(context);
        state = MenuStates.Settings;

        if (backBtn) backBtn.onClick.AddListener(JumpBack);
        if (creditsBtn) creditsBtn.onClick.AddListener(() => SetNextMenu(MenuStates.Credits));

        SetupSliderInformation(masterVolSlider, masterVolText, "MasterVol");
        SetupSliderInformation(musicVolSlider, musicVolText, "MusicVol");
        SetupSliderInformation(sfxVolSlider, sfxVolText, "SFXVol");
    }

    void SetupSliderInformation(Slider slider, TMP_Text sliderText, string parameterName)
    {
        slider.onValueChanged.AddListener((value) => OnSliderValueChanged(value, slider, sliderText, parameterName));
        OnSliderValueChanged(slider.value, slider, sliderText, parameterName);
    }

    private void OnSliderValueChanged(float value, Slider slider, TMP_Text sliderText, string parameterName)
    {
        value = (value == 0.0f) ? -60.0f : 20.0f * Mathf.Log10(slider.value);
        sliderText.text = (value == -60.0f) ? "0%" : $"{(int)(slider.value * 100)}%";
        mixer.SetFloat(parameterName, value);
    }
}
