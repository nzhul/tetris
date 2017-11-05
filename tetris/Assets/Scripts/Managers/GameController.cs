using UnityEngine;

public class GameController : MonoBehaviour
{

    Board _gameBoard;

    Spawner _spawner;

    Shape _activeShape;

    float _dropInterval = .7f;

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

    private void Start()
    {
        _gameBoard = FindObjectOfType<Board>();
        _spawner = FindObjectOfType<Spawner>();
        _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;
        _timeToNextKeyDown = Time.time + _keyRepeatRateDown;
        _timeToNextKeyRotate = Time.time + _keyRepeatRateRotate;

        if (!_gameBoard)
        {
            Debug.LogWarning("WARNING! There is no game board defined!");
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
    }


    private void PlayerInput()
    {
        if (Input.GetButton("MoveRight") && (Time.time > _timeToNextKeyLeftRight) || Input.GetButtonDown("MoveRight"))
        {
            _activeShape.MoveRight();
            _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveLeft();
            }
        }
        else if (Input.GetButton("MoveLeft") && (Time.time > _timeToNextKeyLeftRight) || Input.GetButtonDown("MoveLeft"))
        {
            _activeShape.MoveLeft();
            _timeToNextKeyLeftRight = Time.time + _keyRepeatRateLeftRight;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.MoveRight();
            }
        }
        else if (Input.GetButtonDown("Rotate") && (Time.time > _timeToNextKeyRotate))
        {
            _activeShape.RotateRight();
            _timeToNextKeyRotate = Time.time + _keyRepeatRateRotate;

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                _activeShape.RotateLeft();
            }
        }
        else if (Input.GetButton("MoveDown") && (Time.time > _timeToNextKeyDown) || (Time.time > _timeToDrop))
        {
            _timeToDrop = Time.time + _dropInterval;
            _timeToNextKeyDown = Time.time + _keyRepeatRateDown;

            _activeShape.MoveDown();

            if (!_gameBoard.IsValidPosition(_activeShape))
            {
                LandShape();
            }
        }

        if (!_gameBoard || !_spawner)
        {
            return;
        }
    }

    private void LandShape()
    {
        _timeToNextKeyLeftRight = Time.time;
        _timeToNextKeyDown = Time.time;
        _timeToNextKeyRotate = Time.time;

        _activeShape.MoveUp();
        _gameBoard.StoreShapeInGrid(_activeShape);
        _activeShape = _spawner.SpawnShape();
    }

    private void Update()
    {
        if (!_gameBoard || !_spawner || !_activeShape)
        {
            return;
        }

        PlayerInput();
    }
}