using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] string introMusicName = "";
    [SerializeField] string levelMusicName = "";
    [SerializeField] string resultsMusicName = "";

    int introMusicID;
    int levelMusicID;
    int resultsMusicID;

    void Start()
    {
        introMusicID = SoundManager.Instance.GetSoundID(introMusicName);
        //levelMusicID = SoundManager.Instance.GetSoundID(levelMusicName);
        //resultsMusicID = SoundManager.Instance.GetSoundID(resultsMusicName);
        //SoundManager.Instance.PlaySoundGlobal(introMusicID);
        SoundManager.Instance.PlayGlobalFadeIn(introMusicID, 3f);
        StartCoroutine(TestFadeOut());
    }

    IEnumerator TestFadeOut()
    {
        yield return new WaitForSeconds(10f);
        SoundManager.Instance.StopPlayGlobalFadeOut(introMusicID, 3f);
    }

}
