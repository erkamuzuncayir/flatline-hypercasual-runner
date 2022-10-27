using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource _themeAudio;
    private AudioSource _hitAudio;
    private AudioSource _strafeAudio;
    private AudioSource _jumpAudio;
    private AudioSource _collectAudio;
    private AudioSource _lossAudio;
    public GameObject themeAudioFile;
    public GameObject hitAudioFile;
    public GameObject strafeAudioFile;
    public GameObject jumpAudioFile;
    public GameObject collectAudioFile;
    public GameObject lossAudioFile;
    public bool DeathSounds { get; set; }
    public bool StrafeSounds { get; set; }
    public bool JumpSounds { get; set; }
    public bool CollectSounds { get; set; }

    // Start is called before the first frame update
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        _hitAudio = hitAudioFile.GetComponent<AudioSource>();
        _lossAudio = lossAudioFile.GetComponent<AudioSource>();
        _themeAudio = themeAudioFile.GetComponent<AudioSource>();
        _strafeAudio = strafeAudioFile.GetComponent<AudioSource>();
        _jumpAudio = jumpAudioFile.GetComponent<AudioSource>();
        _collectAudio = collectAudioFile.GetComponent<AudioSource>();
        _themeAudio.Play();
    }
    public void Sounds()
    {
        if (StrafeSounds)
        {
            _strafeAudio.Play();
            StrafeSounds = false;
        }
        if (JumpSounds)
        {
            _jumpAudio.Play();
            JumpSounds = false;
        }
        if (CollectSounds)
        {
            _collectAudio.Play();
            CollectSounds = false;
        }
        if (!DeathSounds) return;
        DeathSounds = false;
        _hitAudio.Play();
        _lossAudio.Play();
        _themeAudio.Stop();
    }
}
