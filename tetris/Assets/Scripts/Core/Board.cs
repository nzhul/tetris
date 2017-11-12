using UnityEngine;

public class Board : MonoBehaviour
{
    public Transform _emptySprite;
    public int _height = 30;
    public int _width = 10;
    public int _header = 8;

    Transform[,] _grid;

    public int _completedRows = 0;

    private void Awake()
    {
        _grid = new Transform[_width, _height];
    }

    void Start()
    {
        DrawEmptyCells();
    }

    bool IsWithinBoard(int x, int y)
    {
        return (x >= 0 && x < _width && y >= 0);
    }

    bool IsOccupied(int x, int y, Shape shape)
    {
        return (_grid[x, y] != null && _grid[x, y].parent != shape.transform);
    }

    public bool IsValidPosition(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            if (!IsWithinBoard((int)pos.x, (int)pos.y))
            {
                return false;
            }

            if (IsOccupied((int)pos.x, (int)pos.y, shape))
            {
                return false;
            }
        }

        return true;
    }

    void DrawEmptyCells()
    {
        if (_emptySprite != null)
        {
            for (int y = 0; y < _height - _header; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    Transform clone;
                    clone = Instantiate(_emptySprite, new Vector3(x, y, 0), Quaternion.identity) as Transform;
                    clone.name = "Board Space ( x = " + x.ToString() + " , y = " + y.ToString() + " )";
                    clone.transform.parent = transform;

                }
            }
        }
        else
        {
            Debug.Log("WARNING! Please assign the emptySprite object!");
        }
    }

    public void StoreShapeInGrid(Shape shape)
    {
        if (shape == null)
        {
            return;
        }

        foreach (Transform child in shape.transform)
        {
            Vector2 pos = Vectorf.Round(child.position);
            _grid[(int)pos.x, (int)pos.y] = child;
        }
    }

    bool IsComplete(int y)
    {
        for (int x = 0; x < _width; x++)
        {
            if (_grid[x, y] == null)
            {
                return false;
            }
        }

        return true;
    }

    void ClearRow(int y)
    {
        for (int x = 0; x < _width; x++)
        {
            if (_grid[x, y] != null)
            {
                Destroy(_grid[x, y].gameObject);
            }

            _grid[x, y] = null;
        }
    }

    void ShiftOneRowDown(int y)
    {
        for (int x = 0; x < _width; x++)
        {
            if (_grid[x, y] != null)
            {
                _grid[x, y - 1] = _grid[x, y];
                _grid[x, y] = null;
                _grid[x, y - 1].position += Vector3.down;
            }
        }
    }

    void ShiftRowsDown(int startY)
    {
        for (int i = startY; i < _height; i++)
        {
            ShiftOneRowDown(i);
        }
    }

    public void ClearAllRows()
    {
        _completedRows = 0;

        for (int y = 0; y < _height; y++)
        {
            if (IsComplete(y))
            {
                _completedRows++;

                ClearRow(y);

                ShiftRowsDown(y + 1);

                y--;
            }
        }
    }

    public bool IsOverLimit(Shape shape)
    {
        foreach (Transform child in shape.transform)
        {
            if (child.transform.position.y >= (_height - _header - 1))
            {
                return true;
            }
        }

        return false;
    }
}