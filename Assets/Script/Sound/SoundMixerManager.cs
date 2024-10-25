using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer mixer;
    //music
    [Header("music Settings")]
    [SerializeField] Slider bgmSlider;
    [SerializeField] string bgmVolVariable;
    [SerializeField] TextMeshProUGUI bgmVolText;
    //sfx
    [Header("SFX Settings")]
    [SerializeField] Slider sfxSlider;
    [SerializeField] string sfxVolVariable;
    [SerializeField] TextMeshProUGUI sfxVolText;
    //sfx
    [Header("ambience Settings")]
    [SerializeField] Slider ambienceSlider;
    [SerializeField] string ambienceVolVariable;
    [SerializeField] TextMeshProUGUI ambienceVolText;
    // Start is called before the first frame update
    void Start()
    {
        //set the volume of the slider, default val is 0.75
        bgmSlider.value = PlayerPrefs.GetFloat(bgmVolVariable, 0.75F);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxVolVariable, 0.75F);
        ambienceSlider.value = PlayerPrefs.GetFloat(ambienceVolVariable, 0.75F);
        //set the text of the bgm and sfx val
        bgmVolText.text = ConvertToText(bgmSlider.value);
        sfxVolText.text = ConvertToText(sfxSlider.value);
        ambienceVolText.text = ConvertToText(ambienceSlider.value);
    }
    public void SetBGMLevel(float sliderValue)
    {
        mixer.SetFloat(bgmVolVariable, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(bgmVolVariable, sliderValue);
        //set the text of the bgm val
        bgmVolText.text = ConvertToText(sliderValue);
    }
    public void SetSFXLevel(float sliderValue)
    {
        mixer.SetFloat(sfxVolVariable, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(sfxVolVariable, sliderValue);
        //set the text of the bgm val
        sfxVolText.text = ConvertToText(sliderValue);
    }
    public void SetAmbienceLevel(float sliderValue)
    {
        mixer.SetFloat(ambienceVolVariable, Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat(ambienceVolVariable, sliderValue);
        //set the text of the bgm val
        ambienceVolText.text = ConvertToText(sliderValue);
    }
    public string ConvertToText(float vol)
    {
        vol *= 100f;
        return (int)vol + "%";
    }
}
