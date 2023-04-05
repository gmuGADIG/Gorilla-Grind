using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField] Sound introSong;
    [SerializeField] Sound[] levelSongs;
    [SerializeField] Sound resultsSong;

    [SerializeField] string introSceneName;
    [SerializeField] string levelSceneName;

    int introMusicID;
    int[] levelSongIDs;
    int resultsMusicID;

    int currentLevelSongID = -1;

    List<int> unplayed = new List<int>();

    public static MusicManager Instance { get; private set; }

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
        introMusicID = SoundManager.Instance.GetSoundID(introSong.name);
        resultsMusicID = SoundManager.Instance.GetSoundID(resultsSong.name);
        levelSongIDs = new int[levelSongs.Length];
        for (int i = 0; i < levelSongIDs.Length; i++)
        {
            levelSongIDs[i] = SoundManager.Instance.GetSoundID(levelSongs[i].name);
        }
    }

    /// <summary>
    /// Plays intro/main menu music.
    /// </summary>
    public void PlayIntroMusic()
    {
        AudioSource audioSource = SoundManager.Instance.PlaySoundGlobal(introMusicID);
        Invoke(nameof(PlayIntroMusic), audioSource.clip.length);
    }

    /// <summary>
    /// Plays results screen music.
    /// </summary>
    public void PlayResultsMusic()
    {
        AudioSource audioSource = SoundManager.Instance.PlaySoundGlobal(resultsMusicID);
        Invoke(nameof(PlayResultsMusic), audioSource.clip.length);
    }

    /// <summary>
    /// Plays level music at random without repeats.
    /// </summary>
    public void PlayLevelMusic()
    {
        int songID = PickSong();
        AudioSource usedAudioSource = SoundManager.Instance.PlaySoundGlobal(songID);
        currentLevelSongID = songID;
        Invoke(nameof(PlayLevelMusic), usedAudioSource.clip.length);
    }

    /// <summary>
    /// Stops all songs currently playing.
    /// </summary>
    public void StopAllSongs()
    {
        CancelInvoke();
        SoundManager.Instance.StopPlayingGlobal(introMusicID);
        SoundManager.Instance.StopPlayingGlobal(resultsMusicID);
        if (currentLevelSongID != -1)
        {
            SoundManager.Instance.StopPlayingGlobal(currentLevelSongID);
            currentLevelSongID = -1;
        }
    }

    /// <summary>
    /// Re-adds all level songs to the list of unplayed songs. Automatically used when there are no more unplayed songs in the list.
    /// </summary>
    void ResetUnplayed()
    {
        for (int i = 0; i < levelSongIDs.Length; i++)
        {
            unplayed.Add(levelSongIDs[i]);
        }
    }

    /// <summary>
    /// Picks a level song randomly from the list of unplayed songs.
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
