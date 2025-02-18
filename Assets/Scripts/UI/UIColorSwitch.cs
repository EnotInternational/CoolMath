using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIColorSwitch : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]private Color _color;
    [SerializeField]private SwitchColorSet _colors;
    private Graphic _graphicToChange;

    private Button _button;
    private bool _enabled = false;
    public UnityEvent<bool, UIColorSwitch> OnChange = new();
    /// <summary>
    /// bool enabled, UIColorSwitch sender
    /// </summary>
    private void Awake()
    {
        _button = GetComponent<Button>();
        _graphicToChange = _button.targetGraphic;
        _button.onClick.AddListener(OnButtonClick);
        ToDefault();
        // _button.colors.
    }
    public void ToDefault()
    {
        _graphicToChange.color = _color;
        _enabled = false;
    }

    public void ToEnabled()
    {
        _graphicToChange.color = _colors.enabledColor;
        _enabled = true;
    }
    private void OnMouseEnter()
    {
    }
    private void OnMouseExit()
    {
        _graphicToChange.color = _color;
        if(_enabled)
        {
            ToDefault();
        }
        else
        {
            ToEnabled();
        }
    }
    private void OnButtonClick()
    {
        if(_enabled)
        {
            // ToDefault();
        }
        else
        {
            ToEnabled();
        }
        OnChange.Invoke(_enabled, this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _graphicToChange.color = Color.Lerp(_colors.pointedColor, _graphicToChange.color, 0.6f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _graphicToChange.color = _color;
        if(_enabled)
        {
            _graphicToChange.color = _colors.enabledColor;
        }
        else
        {
            _graphicToChange.color = _color;
        }
    }
}
