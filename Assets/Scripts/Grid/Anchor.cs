using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Anchor : MonoBehaviour
{
    public UnityEvent OnPositionChanged;
    public Anchor pair;
    public int minToPairDistance = 1;
    public int intPosition{get => _intPosition;}
    private int _intPosition;
    [SerializeField]private Constraint _constraint;
    private Transform _transform;

    private Vector3 _mousePosition;
    private bool _dragging;
    private void Start()
    {
        _transform = transform;
        
        switch(_constraint)
        {
            case Constraint.OnlyY:
                _intPosition = Mathf.RoundToInt(_transform.position.y);
                break;
            case Constraint.OnlyX:
                _intPosition = Mathf.RoundToInt(_transform.position.x);
                break;
        }
        
        OnPositionChanged.Invoke();
    }
    public void MakePair(Anchor other)
    {
        this.pair = other;
        other.pair = this;
    }
    public void MakePair(Anchor other, int minToPairDistance)
    {
        this.pair = other;
        other.pair = this;
        this.minToPairDistance = minToPairDistance;
        other.minToPairDistance = minToPairDistance;
    }
    public void ChangePosition(Vector2 targetPos)
    {
        _transform.position = targetPos;
    }
    private void FixedUpdate()
    {
        if(_dragging)
        {
            Vector2Int mousePos = Vector2Int.RoundToInt(InteractionsManager.instance.mousePosition);
            int targetPos = _intPosition;
            switch(_constraint)
            {
                case Constraint.OnlyY:
                    targetPos = mousePos.y;
                    break;
                case Constraint.OnlyX:
                    targetPos = mousePos.x;
                    break;
            }
            if(targetPos != _intPosition)
            {
                Debug.Log(CanBeMoved(targetPos));
                if(!CanBeMoved(targetPos))
                    return;
                MoveByCurrentAxis(targetPos);
                OnPositionChanged.Invoke();
            };
        }
    }
    private void MoveByCurrentAxis(int targetPos)
    {
        _intPosition = targetPos;

        switch(_constraint)
        {
            case Constraint.OnlyY:
                _transform.position = new Vector3(_transform.position.x, targetPos);
                break;

            case Constraint.OnlyX:
                _transform.position = new Vector3(targetPos, _transform.position.y);
                break;
        }
    }
    private bool CanBeMoved(int position)
    {
        int difference = pair.intPosition - position;
        if(Mathf.Abs(difference) < minToPairDistance)
            return false;
        
        if(Mathf.Sign(difference) != Mathf.Sign(pair.intPosition - _intPosition))
            return false;
            
        return true;
    }
    private void OnEnable()
    {
        _transform = transform;
        InteractionsManager.instance.OnObjectClicked.AddListener(ObjectClickedhandler);
        InteractionsManager.instance.OnClickUp.AddListener(StopDragging);
    }
    private void OnDisable()
    {
        InteractionsManager.instance.OnObjectClicked.RemoveListener(ObjectClickedhandler);
        InteractionsManager.instance.OnClickUp.RemoveListener(StopDragging);
    }
    private void ObjectClickedhandler(Transform clicked)
    {
        if(clicked == _transform)
        {
            _dragging = true;
        }
        else
        {
            _dragging = false;
        }
    }
    private void StopDragging()
    {
        _dragging = false;
    }
    public enum Constraint{OnlyX, OnlyY};
}
