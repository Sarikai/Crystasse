using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables / Properties

    public AudioClip[] _ambientClips;
    public AudioSource _ambientAudioSource;

    #endregion

    #region Methods

    private void Start()
    {
        _ambientAudioSource.loop = true;
        _ambientAudioSource.Play();
        //_audioSource.volume = 100;
    }

    private void Update()
    {
        if (!_ambientAudioSource.isPlaying)
        {
            _ambientAudioSource.Play();
        }
    }

    public void MenuMusic()
    {
        _ambientAudioSource.clip = _ambientClips[2];
    }

    public void IngameMusic()
    {
        _ambientAudioSource.clip = _ambientClips[8];
        _ambientAudioSource.volume = 0.25f;
    }

    public void FightMusic()
    {
        _ambientAudioSource.clip = _ambientClips[1];
    }

    #endregion
}
