using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class IconToggle : MonoBehaviour
{

    public Sprite _iconTrue;
    public Sprite _iconFalse;

    public bool _defaultIconState = true;

    Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _image.sprite = (_defaultIconState) ? _iconTrue : _iconFalse;
    }

    public void ToggleIcon(bool state)
    {
        if (!_image || !_iconTrue || !_iconFalse)
        {
            Debug.LogWarning("WARNING!: IconToggle missing iconTrue or iconFalse!");
            return;
        }

        _image.sprite = (state) ? _iconTrue : _iconFalse;
    }
}