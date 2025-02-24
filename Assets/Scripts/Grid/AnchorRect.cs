using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class AnchorRect : MonoBehaviour
{
    public UnityEvent OnRectChanged;
    [SerializeField]private Anchor _up;
    [SerializeField]private Anchor _right;
    [SerializeField]private Anchor _left;
    [SerializeField]private Anchor _down;   
    [SerializeField]private RectInt _rect = new RectInt();   
    public RectInt rect{get => _rect;}
    [SerializeField]private int _minToPairDistance;
    private void Start()
    {
        _up.OnPositionChanged.AddListener(UpdateRect);
        _down.OnPositionChanged.AddListener(UpdateRect);
        _up.MakePair(_down, _minToPairDistance);

        _right.OnPositionChanged.AddListener(UpdateRect);
        _left.OnPositionChanged.AddListener(UpdateRect);
        _right.MakePair(_left, _minToPairDistance);

        Vector2Int position = new Vector2Int(_left.GetIntPosition(), _down.GetIntPosition());
        Vector2Int size = new Vector2Int(_right.GetIntPosition(), _up.GetIntPosition()) - position;
        _rect.position = position;
        _rect.size = size;


        UpdateRect();
    }
    private void UpdateRect()
    {
        Vector2Int position = new Vector2Int(_left.intPosition, _down.intPosition);
        Vector2Int size = new Vector2Int(_right.intPosition, _up.intPosition) - position;
        _rect.position = position;
        _rect.size = size;
        
        // Debug.Log("Upd rect "+ _rect);
        Vector2 center = ((Vector2)position + (Vector2) (position + size))/2;
        _up.ChangePosition(new Vector2(center.x, _up.intPosition));
        _down.ChangePosition(new Vector2(center.x, _down.intPosition));

        _right.ChangePosition(new Vector2(_right.intPosition, center.y));
        _left.ChangePosition(new Vector2(_left.intPosition, center.y));

        OnRectChanged.Invoke();
    }
}
