using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(MaskableGraphic))]
public class ScreenFader : MonoBehaviour
{


    public float _startAlpha = 1f;
    public float _targetAlpha = 0f;
    public float _delay = 0f;
    public float _timeToFade = 1f;

    float _inc;
    float _currentAlpha;
    MaskableGraphic _graphic;
    Color _originalColor;


    private void Start()
    {
        _graphic = GetComponent<MaskableGraphic>();
        _originalColor = _graphic.color;
        _currentAlpha = _startAlpha;
        Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
        _graphic.color = tempColor;

        _inc = (_targetAlpha - _startAlpha) / _timeToFade * Time.deltaTime;

        StartCoroutine("FadeRoutine");
    }

    IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(_delay);

        while (Mathf.Abs(_targetAlpha - _currentAlpha) > 0.01)
        {
            yield return new WaitForEndOfFrame();

            _currentAlpha = _currentAlpha + _inc;

            Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);

            _graphic.color = tempColor;
        }

        Debug.Log("Screen fader finished!");
    }
}