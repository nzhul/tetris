using UnityEngine;

public class SoundManager : MonoBehaviour
{

    public bool _musicEnabled = true;

    public bool _fxEnabled = true;

    [Range(0, 1)]
    public float _musicVolume = 1.0f;

    [Range(0, 1)]
    public float _fxVolume = 1.0f;

    public AudioClip _clearRowSound;

    public AudioClip _moveSound;

    public AudioClip _dropSound;

    public AudioClip _gameOverSound;

    public AudioSource _musicSource;

    public AudioClip _errorSound;

    public AudioClip _holdSound;

    public AudioClip[] _musicClips;

    AudioClip _randomMusicClip;

    public AudioClip[] _vocalClips;

    public AudioClip _gameOverVocalClip;

    public AudioClip _levelUpVocalClip;

    public IconToggle _musicIconToggle;

    public IconToggle _fxIconToggle;

    private void Start()
    {
        PlayBackgroundMusic(GetRandomClip(_musicClips));
    }

    public AudioClip GetRandomClip(AudioClip[] clips)
    {
        return clips[Random.Range(0, clips.Length)];
    }


    public void PlayBackgroundMusic(AudioClip musicClip)
    {
        if (!_musicEnabled || !musicClip || !_musicSource)
        {
            return;
        }

        // if music is playing, stop it
        _musicSource.Stop();

        _musicSource.clip = musicClip;

        _musicSource.volume = _musicVolume;

        _musicSource.loop = true;

        _musicSource.Play();
    }

    void UpdateMusic()
    {
        if (_musicSource.isPlaying != _musicEnabled)
        {
            if (_musicEnabled)
            {
                PlayBackgroundMusic(GetRandomClip(_musicClips));
            }
            else
            {
                _musicSource.Stop();
            }
        }
    }

    private void Update()
    {
        //UpdateMusic();
    }

    public void ToggleMusic()
    {
        _musicEnabled = !_musicEnabled;
        UpdateMusic();

        if (_musicIconToggle)
        {
            _musicIconToggle.ToggleIcon(_musicEnabled);
        }
    }

    public void ToggleSoundFX()
    {
        _fxEnabled = !_fxEnabled;

        if (_fxIconToggle)
        {
            _fxIconToggle.ToggleIcon(_fxEnabled);
        }
    }

}
