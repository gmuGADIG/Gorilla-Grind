using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

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
        postProcessVolume = FindObjectOfType<PostProcessVolume>();
        postProcessVolume.profile.TryGetSettings(out chromaticAbberation);
        postProcessVolume.profile.TryGetSettings(out colorGrading);
    }

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
