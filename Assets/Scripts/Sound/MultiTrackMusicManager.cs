using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTrackMusicManager : MonoBehaviour
{
    [SerializeField] Sound[] songs;

    int[] songIDs;

    int currentSongID = -1;

    List<int> unplayed = new List<int>();

    public static MultiTrackMusicManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        songIDs = new int[songs.Length];
        for (int i = 0; i < songIDs.Length; i++)
        {
            songIDs[i] = SoundManager.Instance.GetSoundID(songs[i].name);
        }
        PlayMusicOnShuffle();
    }

    /// <summary>
    /// Plays music at random without repeats.
    /// </summary>
    public void PlayMusicOnShuffle()
    {
        int songID = PickSong();
        AudioSource usedAudioSource = SoundManager.Instance.PlaySoundGlobal(songID);
        currentSongID = songID;
        Invoke(nameof(PlayMusicOnShuffle), usedAudioSource.clip.length);
    }

    /// <summary>
    /// Stops the song currently playing.
    /// </summary>
    public void StopMusic()
    {
        CancelInvoke();
        if (currentSongID != -1)
        {
            SoundManager.Instance.StopPlayingGlobal(currentSongID);
            currentSongID = -1;
        }
    }

    /// <summary>
    /// Re-adds all songs to the list of unplayed songs. Automatically used when there are no more unplayed songs in the list.
    /// </summary>
    void ResetUnplayed()
    {
        for (int i = 0; i < songIDs.Length; i++)
        {
            unplayed.Add(songIDs[i]);
        }
    }

    /// <summary>
    /// Picks a song randomly from the list of unplayed songs.
    /// </summary>
    /// <returns>The sound ID of the chosen song.</returns>
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

}
