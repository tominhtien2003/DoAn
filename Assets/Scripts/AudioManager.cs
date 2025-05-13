using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public class Sound
    {
        public string name;
        public AudioClip clip;
        [Range(0f, 1f)]
        public float volume = 1f;
        [Range(0.1f, 3f)]
        public float pitch = 1f;
        public bool loop = false;
        [HideInInspector]
        public AudioSource source;
    }

    [Header("Sound Effects")]
    public Sound[] sounds;

    [Header("Music")]
    public Sound[] musicTracks;
    private int currentMusicIndex = 0;
    private bool isPlayingPlaylist = false;
    private Coroutine playlistCoroutine;

    private bool isTurnOnMusic;
    public bool IsTurnOnMusic
    {
        get => isTurnOnMusic; set
        {
            if (value != isTurnOnMusic)
            {
                isTurnOnMusic = value;
                if (isTurnOnMusic)
                {
                    StartPlaylist();
                }
                else
                {
                    StopPlaylist();
                }
            }
        }
    }
    private bool isTurnOnSound;
    public bool IsTurnOnSound
    {
        get => isTurnOnSound; 
        set
        {
            if (value != isTurnOnSound)
            {
                isTurnOnSound = value;
            }
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // Initialize sound effects
        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        // Initialize music tracks
        foreach (Sound m in musicTracks)
        {
            m.source = gameObject.AddComponent<AudioSource>();
            m.source.clip = m.clip;
            m.source.volume = m.volume;
            m.source.pitch = m.pitch;
            m.source.loop = m.loop;
        }
    }
    private void Start()
    {
        IsTurnOnMusic = true;
        IsTurnOnSound = true;
    }
    public void StartPlaylist()
    {
        if (isPlayingPlaylist) return;
        
        isPlayingPlaylist = true;
        currentMusicIndex = 0;
        
        if (playlistCoroutine != null)
            StopCoroutine(playlistCoroutine);
            
        playlistCoroutine = StartCoroutine(PlayPlaylist());
    }

    public void StopPlaylist()
    {
        if (!isPlayingPlaylist) return;
        
        isPlayingPlaylist = false;
        if (playlistCoroutine != null)
            StopCoroutine(playlistCoroutine);
            
        StopAllMusic();
    }

    private IEnumerator PlayPlaylist()
    {
        while (isPlayingPlaylist)
        {
            if (currentMusicIndex >= musicTracks.Length)
            {
                currentMusicIndex = 0;
            }

            Sound currentMusic = musicTracks[currentMusicIndex];
            currentMusic.source.Play();

            // Wait for the current music to finish
            yield return new WaitForSeconds(currentMusic.clip.length);

            currentMusicIndex++;
        }
    }

    public void PlaySound(string name)
    {
        if (!IsTurnOnSound) return;
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }
        s.source.Stop();
    }

    public void PlayMusic(string name)
    {
        if (!IsTurnOnMusic) return;
        // Stop all current music
        foreach (Sound m in musicTracks)
        {
            m.source.Stop();
        }

        Sound music = System.Array.Find(musicTracks, track => track.name == name);
        if (music == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        music.source.Play();
    }

    public void StopMusic(string name)
    {
        Sound music = System.Array.Find(musicTracks, track => track.name == name);
        if (music == null)
        {
            Debug.LogWarning("Music: " + name + " not found!");
            return;
        }
        music.source.Stop();
    }

    public void StopAllMusic()
    {
        foreach (Sound m in musicTracks)
        {
            m.source.Stop();
        }
    }

    public void SetSoundVolume(float volume)
    {
        foreach (Sound s in sounds)
        {
            s.source.volume = volume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        foreach (Sound m in musicTracks)
        {
            m.source.volume = volume;
        }
    }
} 