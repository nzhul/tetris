using UnityEngine;

public class Holder : MonoBehaviour
{

    public Transform _holderXForm;
    public Shape _heldShape = null;
    float _scale = 0.5f;

    public void Catch(Shape shape)
    {
        if (_heldShape)
        {
            Debug.LogWarning("HOLDER WARNING! Release a shape before trying to hold!");
            return;
        }

        if (!shape)
        {
            Debug.LogWarning("HOLDER WARNING! Invalid shape!");
            return;
        }

        if (_holderXForm)
        {
            shape.transform.position = _holderXForm.position + shape._queueOffset;
            shape.transform.localScale = new Vector3(_scale, _scale, _scale);
            _heldShape = shape;
        }
        else
        {
            Debug.LogWarning("HOLDER WARNING! Holder has no transform assigned!");
        }
    }
}
