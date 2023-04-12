using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSliderMaster : MonoBehaviour
{
    [SerializeField] private string[] prefSliderVals;
    [SerializeField] private string[] prefMixerVals;
    [SerializeField] private Slider[] audioSliders;
    [SerializeField] private AudioMixer audioMixer;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("AudioSlidersAdjustedBinary") == 1)
        {
            for (int i = 0; i < prefSliderVals.Length; ++i)
            {
                audioSliders[i].value = PlayerPrefs.GetFloat(prefSliderVals[i]);
            }
            for (int i = 0; i < prefMixerVals.Length; ++i)
            {
                audioMixer.SetFloat(prefMixerVals[i], PlayerPrefs.GetFloat(prefMixerVals[0]));
            }
        }
        else
        {
            for (int i = 0; i < prefSliderVals.Length; i++)
            {
                audioSliders[i].value = 0.5f;
                PlayerPrefs.SetFloat(prefSliderVals[0], 0.5f);
            }
            for (int i = 0; i < prefMixerVals.Length; i++)
            {
                audioMixer.SetFloat(prefMixerVals[i], 0.0f);
                PlayerPrefs.SetFloat(prefMixerVals[i], 0.0f);
            }
            PlayerPrefs.SetInt("AudioSlidersAdjustedBinary", 1);
        }
    }
}
