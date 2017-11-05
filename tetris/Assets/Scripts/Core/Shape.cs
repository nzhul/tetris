using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour {


    public bool _canRotate = true;

    void Move(Vector3 moveDirection)
    {
        transform.position += moveDirection;
    }

    public void MoveLeft()
    {
        Move(Vector3.left);
    }

    public void MoveRight()
    {
        Move(Vector3.right);
    }

    public void MoveDown()
    {
        Move(Vector3.down);
    }

    public void MoveUp()
    {
        Move(Vector3.up);
    }

    public void RotateRight()
    {
        if (_canRotate)
        {
            transform.Rotate(0, 0, -90);
        }
    }

    public void RotateLeft()
    {
        if (_canRotate)
        {
            transform.Rotate(0, 0, 90);
        }
    }

    private void Start()
    {

    }
}