using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Sound introSong;
    [SerializeField] Sound[] levelSongs;
    [SerializeField] Sound resultsSong;

    int introMusicID;
    int[] levelSongIDs;
    int resultsMusicIDs;

    List<int> unplayed = new List<int>();

    void Start()
    {
        levelSongIDs = new int[levelSongs.Length];
        for (int i = 0; i < levelSongIDs.Length; i++)
        {
            levelSongIDs[i] = SoundManager.Instance.GetSoundID(levelSongs[i].name);
        }
        //ResetUnplayed();
        PlayLevelMusic();
    }

    void ResetUnplayed()
    {
        for (int i = 0; i < levelSongIDs.Length; i++)
        {
            unplayed.Add(levelSongIDs[i]);
        }
    }

    int PickSong()
    {
        if (unplayed.Count == 0)
        {
            ResetUnplayed();
        }
        int randSongID = unplayed[Random.Range(0, unplayed.Count)];
        unplayed.Remove(randSongID);
        return randSongID;
    }

    void PlayLevelMusic()
    {
        int songID = PickSong();
        print(songID);
        AudioSource usedAudioSource = SoundManager.Instance.PlaySoundGlobal(songID);
        Invoke(nameof(PlayLevelMusic), usedAudioSource.clip.length);
    }

}
