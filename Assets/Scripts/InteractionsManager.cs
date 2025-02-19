using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

[RequireComponent(typeof(PlayerInput))]
public class InteractionsManager : MonoBehaviour
{
    [SerializeField]private float _checkSkip = 0.02f;
    [SerializeField]private int _mousePositionMargin = 50;
    [SerializeField]private LayerMask _uiMask;
    [SerializeField]private InputSystemUIInputModule inputModule;
    public Vector2 mousePosition{get => _mousePosition;}
    private Vector2 _mousePosition;
    
    private Vector2 _mousePxPosition;
    private Vector2 _lastCheckMousePosition;
    public UnityEvent OnClickUp;
    public UnityEvent OnClickDown;
    public UnityEvent OnDoubleClickPreformed;
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
    public bool IsOverUI()
    {
        RaycastResult raycastResult = inputModule.GetLastRaycastResult(0);
        if(!raycastResult.gameObject)
            return false;
        if(raycastResult.gameObject.layer == LayerMask.NameToLayer("UI"))
        {
            return true;
        }
        return false;
    }
    public bool CheckOverlapMousePos()
    {
        // Ray ray = Camera.main.ScreenPointToRay(_mousePxPosition);
        // Debug.Log(ray);
        // Debug.DrawRay(ray.origin, ray.direction);
        if(Physics2D.OverlapPoint(mousePosition) || Physics.Raycast(Camera.main.ScreenPointToRay(_mousePxPosition), 10f, _uiMask))
        {
            // Physics.Raycast(Camera.main.ScreenPointToRay(_mousePxPosition), out var hitInfo, 10f, _uiMask);
            
            // Debug.Log(hitInfo.transform.gameObject);
            return true;
        }
        return false;
    }
    private void OnPoint(InputValue value)
    {
        _mousePxPosition = value.Get<Vector2>();
        // Debug.Log(_mousePxPosition);
        _mousePxPosition =new Vector2
        (
            Mathf.Clamp(_mousePxPosition.x, _mousePositionMargin, Camera.main.pixelWidth-_mousePositionMargin), 
            Mathf.Clamp(_mousePxPosition.y, _mousePositionMargin, Camera.main.pixelHeight-_mousePositionMargin)
        );
        Vector2 newPosition = Camera.main.ScreenToWorldPoint(_mousePxPosition);

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
        OnDoubleClickPreformed.Invoke();
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
            if(hit.TryGetComponent<IClickable>(out IClickable component))
            {
                component.OnClickDown();
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
