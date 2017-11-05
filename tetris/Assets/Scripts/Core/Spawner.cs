using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public Shape[] _allShapes;


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
        shape = Instantiate(GetRandomShape(), transform.position, Quaternion.identity) as Shape;
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

}
