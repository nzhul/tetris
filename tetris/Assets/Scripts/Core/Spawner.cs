using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Shape[] _allShapes;

    public Transform[] _queuedXForms = new Transform[3];

    Shape[] _queueShapes = new Shape[3];

    float _queueScale = 0.5f;

    private void Start()
    {
        InitQueue();
    }

    Shape GetRandomShape()
    {
        int i = Random.Range(0, _allShapes.Length);
        if (_allShapes[i])
        {
            return _allShapes[i];
        }
        else
        {
            Debug.Log("WARNING! Invalid shape in spawner!");
            return null;
        }
    }

    public Shape SpawnShape()
    {
        Shape shape = null;
        shape = GetQueuedShape();
        shape.transform.position = transform.position;
        shape.transform.localScale = Vector3.one;

        if (shape)
        {
            return shape;
        }
        else
        {
            Debug.LogWarning("WARNING! Invalid shape in spawner!");
            return null;
        }
    }

    private void InitQueue()
    {
        for (int i = 0; i < _queueShapes.Length; i++)
        {
            _queueShapes[i] = null;
        }

        FillQueue();
    }

    private void FillQueue()
    {
        for (int i = 0; i < _queueShapes.Length; i++)
        {
            if (!_queueShapes[i])
            {
                _queueShapes[i] = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
                _queueShapes[i].transform.position = _queuedXForms[i].position + _queueShapes[i]._queueOffset;
                _queueShapes[i].transform.localScale = new Vector3(_queueScale, _queueScale, _queueScale);
            }
        }
    }

    Shape GetQueuedShape()
    {
        Shape firstShape = null;

        if (_queueShapes[0])
        {
            firstShape = _queueShapes[0];
        }

        for (int i = 1; i < _queueShapes.Length; i++)
        {
            _queueShapes[i - 1] = _queueShapes[i];
            _queueShapes[i - 1].transform.position = _queuedXForms[i - 1].position + _queueShapes[i]._queueOffset;
        }

        _queueShapes[_queueShapes.Length - 1] = null;

        FillQueue();

        return firstShape;
    }

}
