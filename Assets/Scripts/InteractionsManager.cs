using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InteractionsManager : MonoBehaviour
{
    public Vector2 mousePosition{get => _mousePosition;}
    private Vector2 _mousePosition;
    public UnityEvent OnClickUp;
    public UnityEvent OnClickDown;
    public UnityEvent<Transform> OnObjectClicked;
    public UnityEvent OnVoidClicked;
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
        _mousePosition = Camera.main.ScreenToWorldPoint(value.Get<Vector2>());
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
            Collider2D hit = Physics2D.OverlapPoint(mousePosition);
            if(hit == null)
            {
                OnVoidClicked.Invoke();
                return;
            }
            OnObjectClicked.Invoke(hit.transform);
        }
        else
        {
            OnClickUp.Invoke();
        }
        
    }
}
