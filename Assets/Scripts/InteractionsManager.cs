using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InteractionsManager : MonoBehaviour
{
    [SerializeField]private float _checkSkip = 0.02f;
    public Vector2 mousePosition{get => _mousePosition;}
    private Vector2 _mousePosition;
    private Vector2 _lastCheckMousePosition;
    public UnityEvent OnClickUp;
    public UnityEvent OnClickDown;
    public UnityEvent<Transform> OnObjectClicked;
    public UnityEvent<Transform> OnObjectPointed;
    private bool _mouseHold = false;
    public UnityEvent OnVoidClicked;
    private Transform _pointedObject;
    public static InteractionsManager instance
    {
        get;
        private set;
        
    }
    private void Awake()
    {
        if(instance != null)
        {
            this.enabled = false;
            Debug.LogError("There can`t be more than one instance on the scene");
            return;
        }

        instance = this;
    }
    private void OnPoint(InputValue value)
    {
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());

        if((newPosition - _lastCheckMousePosition).magnitude >= _checkSkip)
        {
            _lastCheckMousePosition = newPosition;
            TryPointNewObject();
        }

        _mousePosition = newPosition;


    }
    private void TryPointNewObject()
    {
        if(!_mouseHold)
        {
            return;
        }
        Collider2D hit = Physics2D.OverlapPoint(mousePosition);
        if(hit == null)
        {
            _pointedObject = null;
            return;
        }
        if(hit.transform == _pointedObject)
        {
            return;
        }
        _pointedObject = hit.transform;
        OnObjectPointed.Invoke(_pointedObject);
        
    }
    private void OnDoubleClick(InputValue value)
    {
        
    }   
    private void OnDelete(InputValue value)
    {

    }
    private void OnRightClick(InputValue value)
    {

    }
    private void OnClick(InputValue value)
    {
        if(value.isPressed)
        {
            OnClickDown.Invoke();
            _mouseHold = true;
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);
            if(hit == null)
            {
                OnVoidClicked.Invoke();
                return;
            }
            if(hit.transform != _pointedObject)
            {
                _pointedObject = hit.transform;
                OnObjectPointed.Invoke(_pointedObject);
            }
            OnObjectClicked.Invoke(hit.transform);
        }
        else
        {
            _mouseHold = false;
            OnClickUp.Invoke();
        }
        
    }
}
