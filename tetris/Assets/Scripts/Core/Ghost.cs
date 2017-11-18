using UnityEngine;

public class Ghost : MonoBehaviour
{

    Shape _ghostShape = null;
    bool _hitBottom = false;
    public Color _color = new Color(1f, 1f, 1f, 0.1f);

    public void DrawGhost(Shape originalShape, Board gameBoard)
    {
        if (!_ghostShape)
        {
            _ghostShape = Instantiate(originalShape, originalShape.transform.position, originalShape.transform.rotation) as Shape;
            _ghostShape.gameObject.name = "GhostShape";

            SpriteRenderer[] allRendererds = _ghostShape.GetComponentsInChildren<SpriteRenderer>();

            foreach (SpriteRenderer r in allRendererds)
            {
                r.color = _color;
            }
        }
        else
        {
            _ghostShape.transform.position = originalShape.transform.position;
            _ghostShape.transform.rotation = originalShape.transform.rotation;
        }

        _hitBottom = false;

        while (!_hitBottom)
        {
            _ghostShape.MoveDown();
            if (!gameBoard.IsValidPosition(_ghostShape))
            {
                _ghostShape.MoveUp();
                _hitBottom = true;
            }
        }
    }

    public void Reset()
    {
        Destroy(_ghostShape.gameObject);
    }


}
