using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SwitcherBlock : MonoBehaviour
{
    [SerializeField]private UIColorSwitch _basic;
    private UIColorSwitch[] _switches = new UIColorSwitch[]{};
    private void Awake()
    {
        _switches = GetComponentsInChildren<UIColorSwitch>();
        foreach(UIColorSwitch colorSwitch in _switches)
        {
            colorSwitch.OnChange.AddListener(OnChangeHandler);
        }
    }
    private void Start()
    {
        if(_basic != null)
            _basic.ToEnabled();
    }
    private void OnChangeHandler(bool enabled, UIColorSwitch sender)
    {
        if(enabled)
        {
            foreach(UIColorSwitch colorSwitch in _switches)
            {
                if(colorSwitch == sender)
                    continue;
                colorSwitch.ToDefault();
            }
        }
    }
}
