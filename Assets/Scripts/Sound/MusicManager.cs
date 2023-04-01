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
        PlayLevelMusic();
    }

    void PlayLevelMusic()
    {
        AudioSource usedAudioSource = SoundManager.Instance.PlaySoundGlobal(introMusicID);
        Invoke(nameof(PlayLevelMusic), usedAudioSource.clip.length);
    }

}
