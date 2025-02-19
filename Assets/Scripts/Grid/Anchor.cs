using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class Anchor : MonoBehaviour, IClickable
{
    public UnityEvent OnPositionChanged;
    public Anchor pair;
    [SerializeField]private Transform _derivePositionFrom;
    public int minToPairDistance = 1;
    public int intPosition{get => _intPosition;}
    private int _intPosition;
    [SerializeField]private Constraint _constraint;
    [SerializeField]private Transform _transform;

    private Vector3 _mousePosition;
    private bool _dragging;
    private void Start()
    {
        
        switch(_constraint)
        {
            case Constraint.OnlyY:
                _intPosition = Mathf.FloorToInt(_derivePositionFrom.position.y);
                break;
            case Constraint.OnlyX:
                _intPosition = Mathf.FloorToInt(_derivePositionFrom.position.x);
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
    public int GetIntPosition()
    {
        switch(_constraint)
        {
            case Constraint.OnlyY:
                _intPosition = Mathf.FloorToInt(_derivePositionFrom.position.y);
                break;
            case Constraint.OnlyX:
                _intPosition = Mathf.FloorToInt(_derivePositionFrom.position.x);
                break;
        }
        return _intPosition;
    }
    public void ChangePosition(Vector2 targetPos)
    {
        _derivePositionFrom.position = targetPos;
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
                // Debug.Log(CanBeMoved(targetPos));
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
                _derivePositionFrom.position = new Vector3(_derivePositionFrom.position.x, targetPos);
                break;

            case Constraint.OnlyX:
                _derivePositionFrom.position = new Vector3(targetPos, _derivePositionFrom.position.y);
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
        InteractionsManager.instance.OnClickUp.AddListener(StopDragging);
    }
    private void OnDisable()
    {
        InteractionsManager.instance.OnClickUp.RemoveListener(StopDragging);
    }
    public void OnClickDown()
    {
        _dragging = true;
        GridToolManager.instance.block = true;
    }
    private void StopDragging()
    {
        _dragging = false;
        GridToolManager.instance.block = false;
    }
    public enum Constraint{OnlyX, OnlyY};
}
