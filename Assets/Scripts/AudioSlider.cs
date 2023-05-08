using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string mixerGroup;
    
    public void SetAudio()
    {
        audioMixer.SetFloat(mixerGroup, Mathf.Log10(gameObject.GetComponent<Slider>().value) * 20.0f);
        PlayerPrefs.SetFloat(mixerGroup, Mathf.Log10(gameObject.GetComponent<Slider>().value) * 20.0f);
    }
}
