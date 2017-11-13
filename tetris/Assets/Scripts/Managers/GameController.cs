using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{

    Board _gameBoard;

    Spawner _spawner;

    Shape _activeShape;

    public float _dropInterval = .7f;
    float _dropIntervalModded;

    float _timeToDrop;

    float _timeToNextKeyLeftRight;
    [Range(.02f, 1f)]
    public float _keyRepeatRateLeftRight = .15f;

    float _timeToNextKeyDown;
    [Range(.01f, 1f)]
    public float _keyRepeatRateDown = .01f;

    float _timeToNextKeyRotate;
    [Range(.02f, 1f)]
    public float _keyRepeatRateRotate = .25f;

    bool _gameOver = false;

    public GameObject _gameOverPanel;

    SoundManager _soundManager;

    ScoreManager _scoreManager;

    public IconToggle _rotIconToggle;

    bool _clockWise = true;

    public bool _isPaused = false;

    public GameObject _pausePanel;

    private void Start()
    {
        _gameBoard = FindObjectOfType<Board>();
        _spawner = FindObjectOfType<Spawner>();
        _soundManager = FindObjectOfType<SoundManager>();
        _scoreManager = FindObjectOfType<ScoreManager>();
        _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;
        _timeToNextKeyDown = Time.time + _keyRepeatRateDown;
        _timeToNextKeyRotate = Time.time + _keyRepeatRateRotate;

        if (!_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
        }

        if (!_soundManager)
        {
            Debug.LogWarning("WARNING! There is no sound manager defined!");
        }

        if (!_scoreManager)
        {
            Debug.LogWarning("WARNING! There is no score manager!");
        }

        if (!_spawner)
        {
            Debug.LogWarning("WARNING! There is no spawner defined!");
        }
        else
        {
            _spawner.transform.position = Vectorf.Round(_spawner.transform.position);

            if (_activeShape == null)
            {
                _activeShape = _spawner.SpawnShape();
            }
        }

        if (_gameOverPanel)
        {
            _gameOverPanel.SetActive(false);
        }

        if (_pausePanel)
        {
            _pausePanel.SetActive(false);
        }

        _dropIntervalModded = _dropInterval;
    }


    private void PlayerInput()
    {
        if ((Input.GetButton("MoveRight") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveRight"))
        {
            _activeShape.MoveRight();
            _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveLeft();
                PlaySound(_soundManager._errorSound, .5f);
            }
            else
            {
                PlaySound(_soundManager._moveSound, .5f);
            }
        }
        else if ((Input.GetButton("MoveLeft") && (Time.time > _timeToNextKeyLeftRight)) || Input.GetButtonDown("MoveLeft"))
        {
            _activeShape.MoveLeft();
            _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveRight();
                PlaySound(_soundManager._errorSound, .5f);
            }
            else
            {
                PlaySound(_soundManager._moveSound, .5f);
            }
        }
        else if (Input.GetButtonDown("Rotate") && (Time.time > _timeToNextKeyRotate))
        {
            //_activeShape.RotateRight();
            _activeShape.RotateClockwise(_clockWise);
            _timeToNextKeyRotate = Time.time + _keyRepeatRateRotate;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                //_activeShape.RotateLeft();
                _activeShape.RotateClockwise(!_clockWise);
                PlaySound(_soundManager._errorSound, .5f);
            }
            else
            {
                PlaySound(_soundManager._moveSound, .5f);
            }
        }
        else if ((Input.GetButton("MoveDown") && (Time.time > _timeToNextKeyDown)) || (Time.time > _timeToDrop))
        {
            _timeToDrop = Time.time + _dropIntervalModded;
            _timeToNextKeyDown = Time.time + _keyRepeatRateDown;

            _activeShape.MoveDown();

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                if (_gameBoard.IsOverLimit(_activeShape))
                {
                    GameOver();
                }
                else
                {
                    LandShape();
                }
            }
        }
        else if (Input.GetButtonDown("ToggleRot"))
        {
            ToggleRotDirection();
        }
        else if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }

        if (!_gameBoard || !_spawner)
        {
            return;
        }
    }

    private void PlaySound(AudioClip clip, float volMultiplier)
    {
        if (_soundManager._fxEnabled && clip)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, Mathf.Clamp(_soundManager._fxVolume * volMultiplier, 0.05f, 1f));
        }
    }

    private void GameOver()
    {
        _activeShape.MoveUp();
        _gameOver = true;
        Debug.LogWarning(_activeShape.name + " is over the limit");

        if (_gameOverPanel)
        {
            _gameOverPanel.SetActive(true);
        }

        PlaySound(_soundManager._gameOverSound, 5f);
        PlaySound(_soundManager._gameOverVocalClip, 5f);
    }

    private void LandShape()
    {
        _timeToNextKeyLeftRight = Time.time;
        _timeToNextKeyDown = Time.time;
        _timeToNextKeyRotate = Time.time;

        _activeShape.MoveUp();
        _gameBoard.StoreShapeInGrid(_activeShape);
        _activeShape = _spawner.SpawnShape();

        _gameBoard.ClearAllRows();

        PlaySound(_soundManager._dropSound, .5f);

        if (_gameBoard._completedRows > 0)
        {
            _scoreManager.ScoreLines(_gameBoard._completedRows);
            if (_scoreManager._didLevelUp)
            {
                PlaySound(_soundManager._levelUpVocalClip, 1f);
                _dropIntervalModded = Mathf.Clamp(_dropInterval - ((float)_scoreManager._level - 1) * 0.05f, 0.05f, 1f);
            }
            else
            {
                if (_gameBoard._completedRows > 1)
                {
                    AudioClip randomVocal = _soundManager.GetRandomClip(_soundManager._vocalClips);
                    PlaySound(randomVocal, 1f);
                }
            }

            PlaySound(_soundManager._clearRowSound, .5f);
        }


    }

    private void Update()
    {
        if (!_gameBoard || !_soundManager || !_spawner || !_activeShape || !_scoreManager || _gameOver)
        {
            return;
        }

        PlayerInput();
    }

    public void Restart()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleRotDirection()
    {
        _clockWise = !_clockWise;
        if (_rotIconToggle)
        {
            _rotIconToggle.ToggleIcon(_clockWise);
        }
    }

    public void TogglePause()
    {
        if (_gameOver)
        {
            return;
        }

        _isPaused = !_isPaused;

        if (_pausePanel)
        {
            _pausePanel.SetActive(_isPaused);

            if (_soundManager)
            {
                _soundManager._musicSource.volume = (_isPaused) ? _soundManager._musicVolume * .25f : _soundManager._musicVolume;
            }

            Time.timeScale = (_isPaused) ? 0 : 1;
        }
    }
}