using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    int _score = 0;
    int _lines;
    public int _level = 1;

    public int _linesPerLevel = 5;

    const int _minLines = 1;
    const int _maxLines = 4;

    public Text _linesText;
    public Text _levelText;
    public Text _scoreText;

    public bool _didLevelUp = false;

    public void ScoreLines(int n)
    {
        _didLevelUp = false;

        n = Mathf.Clamp(n, _minLines, _maxLines);

        switch (n)
        {
            case 1:
                _score += 40 * _level;
                break;
            case 2:
                _score += 100 * _level;
                break;
            case 3:
                _score += 300 * _level;
                break;
            case 4:
                _score += 1200 * _level;
                break;
            default:
                break;
        }


        _lines -= n;

        if (_lines <= 0)
        {
            LevelUp();
        }

        UpdateUIText();
    }

    public void Reset()
    {
        _level = 1;
        _lines = _linesPerLevel * _level;
        UpdateUIText();
    }

    private void Start()
    {
        Reset();
    }

    void UpdateUIText()
    {
        if (_linesText)
        {
            _linesText.text = _lines.ToString();
        }

        if (_levelText)
        {
            _levelText.text = _level.ToString();
        }

        if (_scoreText)
        {
            _scoreText.text = PadZero(_score, 5);
        }
    }

    string PadZero(int n, int padDigits)
    {
        string nStr = n.ToString();

        while (nStr.Length < padDigits)
        {
            nStr = "0" + nStr;
        }

        return nStr;
    }

    public void LevelUp()
    {
        _level++;
        _lines = _linesPerLevel * _level;
        _didLevelUp = true;
    }
}
