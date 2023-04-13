using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSliderMaster : MonoBehaviour
{
    private string[] prefSliderVals = { "MasterSlider", "MusicSlider", "FXSlider" };
    private string[] prefMixerVals = { "Master", "Music", "FX" };
    [SerializeField] private Slider[] audioSliders;
    [SerializeField] private AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < prefSliderVals.Length; ++i)
        {
            audioSliders[i].value = PlayerPrefs.GetFloat(prefSliderVals[i], 0.683f);
        }
        for (int i = 0; i < prefMixerVals.Length; ++i)
        {
            audioMixer.SetFloat(prefMixerVals[i], PlayerPrefs.GetFloat(prefMixerVals[i], 0.0f));
        }
    }
}
