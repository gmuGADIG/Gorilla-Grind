using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

/// <summary>
/// Script for runtime control of post-processing effects.
/// </summary>
public class PostProcessingController : MonoBehaviour
{
    public static PostProcessingController Instance;

    PostProcessVolume postProcessVolume;
    ChromaticAberration chromaticAbberation;
    ColorGrading colorGrading;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        postProcessVolume = GetComponent<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out chromaticAbberation);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

    /// <summary>
    /// Turn on/off the preset chromatic aberration effect.
    /// </summary>
    /// <param name="on"></param>
    public static void ChromaticAberration(bool on)
    {
        if (on)
        {
            Instance.chromaticAbberation.active = true;
        }
        else
        {
            Instance.chromaticAbberation.active = false;
        }
    }

    /// <summary>
    /// Turn on/off the preset color grading effect.
    /// </summary>
    /// <param name="on"></param>
    public static void ColorGrading(bool on)
    {
        if (on)
        {
            Instance.colorGrading.active = true;
        }
        else
        {
            Instance.colorGrading.active = false;
        }
    }


}
