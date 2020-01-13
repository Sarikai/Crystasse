using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region Variables / Properties

    public AudioClip[] _audioClips;
    public AudioSource _audioSource;

    #endregion

    #region Methods

    private void Start()
    {
        _audioSource.loop = true;
        _audioSource.Play();
        //_audioSource.volume = 100;
    }

    private void Update()
    {
        if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    public void MenuMusic()
    {
        _audioSource.clip = _audioClips[2];
    }

    public void IngameMusic()
    {
        _audioSource.clip = _audioClips[8];
        _audioSource.volume = 0.25f;
    }

    public void FightMusic()
    {
        _audioSource.clip = _audioClips[1];
    }

    #endregion
}
