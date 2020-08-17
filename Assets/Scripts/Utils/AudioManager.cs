using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum Sounds
{
    GUNSHOT,
    BOMB_SHOOT,
    BOMB_EXPLODE,
    START_GAME,
    PAUSE,
    UNPAUSE,
    LOSE_LIFE,
    LOSE_SHIELD,
    PICKUP,
    ENEMY_WEAK,
    ENEMY_STRONG,
    CLICK,
    CLICK_BACK
}

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioClip[] gunshots;
    [SerializeField] AudioClip[] enemyWeak;

    [SerializeField] AudioClip bombShot;
    [SerializeField] AudioClip bombExplode;

    [SerializeField] AudioClip startGame;
    [SerializeField] AudioClip pause;
    [SerializeField] AudioClip unpause;

    [SerializeField] AudioClip loseLife;
    [SerializeField] AudioClip loseShield;
    [SerializeField] AudioClip pickup;

    [SerializeField] AudioClip enemyStrong;

    [SerializeField] AudioClip click;
    [SerializeField] AudioClip click_back;

    [SerializeField] AudioClip introLoop;
    [SerializeField] AudioClip mainLoop;

    [SerializeField] AudioSource sfxAudioSource;
    [SerializeField] AudioSource musicAudioSource;

    //AudioMixer sfxMixer;
    //public AudioMixerGroup musicMixer;

    AudioLowPassFilter lpf;

    Dictionary<Sounds, AudioClip> audioDict;

    bool isPaused = false;
    bool isAtStartScreen = true;

    public bool IsPaused{ set { isPaused = value; } }
    public bool IsAtStartScreen { set { isAtStartScreen = value; } }

    private void Awake()
    {
        //sfxAudioSource = gameObject.AddComponent<AudioSource>();

        //lpf = gameObject.AddComponent<AudioLowPassFilter>();
        //musicAudioSource = gameObject.AddComponent<AudioSource>();
        
        //musicAudioSource.outputAudioMixerGroup = musicMixer;

    }

    // Start is called before the first frame update
    void Start()
    {
        lpf = musicAudioSource.gameObject.GetComponent<AudioLowPassFilter>();
        //lpf.cutoffFrequency = 500;
        lpf.enabled = false;

        audioDict = new Dictionary<Sounds, AudioClip>
        {
            {Sounds.BOMB_EXPLODE, bombExplode },
            {Sounds.BOMB_SHOOT, bombShot },
            {Sounds.ENEMY_STRONG, enemyStrong },
            //{Sounds.ENEMY_WEAK, enemyWeak },
            {Sounds.START_GAME, startGame},
            {Sounds.PAUSE, pause },
            {Sounds.UNPAUSE, unpause },
            {Sounds.LOSE_LIFE, loseLife },
            {Sounds.LOSE_SHIELD, loseShield},
            {Sounds.PICKUP, pickup },
            {Sounds.CLICK, click },
            {Sounds.CLICK_BACK, click_back }
        };

        musicAudioSource.clip = introLoop;
        musicAudioSource.loop = true;
        musicAudioSource.Play();
    }

    public void PlaySound(Sounds sound)
    {
        if (sound == Sounds.GUNSHOT)
        {
            sfxAudioSource.PlayOneShot(gunshots[Random.Range(0, gunshots.Length - 1)]);
        }

        else if (sound == Sounds.ENEMY_WEAK)
        {
            sfxAudioSource.PlayOneShot(enemyWeak[Random.Range(0, enemyWeak.Length - 1)]);
        }

        else
        {
            sfxAudioSource.PlayOneShot(audioDict[sound]);
        }
    }

    void PlayMusic()
    {
        // if still on title screen, loop synth intro

        // if at pause menu, filter song
        

        // if song ends, play from beginning
    }

    // Update is called once per frame
    void Update()
    {
        if (isPaused)
        {
            lpf.enabled = true;
        }
        else
        {
            lpf.enabled = false;
        }

        if (!isAtStartScreen && !musicAudioSource.isPlaying)
        {
            musicAudioSource.clip = mainLoop;
            musicAudioSource.loop = true;
            musicAudioSource.Play();
        }
        else if (!isAtStartScreen && musicAudioSource.isPlaying)
        {
            musicAudioSource.loop = false;
        }
    }
}
