using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NumberOnlyInputFieldValidator : MonoBehaviour
{
    [SerializeField]private float _maxNumber;
    private TMP_InputField _inputField;
    public UnityEvent<float> OnValueChanged = new UnityEvent<float>();
    private void Awake()
    {
        _inputField = GetComponent<TMP_InputField>();
    }
    private void OnEnable()
    {
        if(!_inputField)
        {
            _inputField = GetComponent<TMP_InputField>();
        }
        _inputField.onValueChanged.AddListener(ValidateInput);
    }
    private void OnDisable()
    {
        _inputField.onValueChanged.RemoveListener(ValidateInput);
    }
    private void ValidateInput(string text)
    {
        float number;
        if(!float.TryParse(text, out number))
        {
            string result = new string(text.Where(t => char.IsDigit(t)).ToArray());
            _inputField.text = result;
            number = (result == string.Empty)? 0 : float.Parse(result);
 
        }
        if(number > _maxNumber)
        {
            number = _maxNumber;
            _inputField.text = number.ToString();
        }
        OnValueChanged.Invoke(number);
    }
}
