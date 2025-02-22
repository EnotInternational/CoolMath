using System.Collections;
using TMPro;
using UnityEngine;

public class UILogger : MonoBehaviour
{
    [SerializeField]private float _hideTextDelay = 2f;
    public static UILogger Instance { get; private set;}
    private TextMeshProUGUI _textMesh;
    private Coroutine _hideCoroutine;
    private void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning($"There can`t be more than one {nameof(UILogger)} at the scene");
            this.enabled = false;
            return;
        }
        _textMesh = GetComponent<TextMeshProUGUI>();
        Instance = this;
    }
    public void WriteLog(string text, Color color)
    {
        if(_hideCoroutine != null)
            StopCoroutine(_hideCoroutine);
        _hideCoroutine = StartCoroutine(HideText());
        _textMesh.color = color;
        _textMesh.text = text;
    }
    public void WriteLog(string text)
    {
        WriteLog(text,Color.white);
    }
    public static void Log(string ruText, string enText, Color color)
    {
        bool isEnglish = TranslatorManager.Instance.language == TranslatorManager.Language.EN;
        Instance.WriteLog(isEnglish ? enText :ruText, color);
    }
    public static void Log(string ruText, string enText)
    {
        bool isEnglish = TranslatorManager.Instance.language == TranslatorManager.Language.EN;
        Instance.WriteLog(isEnglish ? enText :ruText);
    }
    /// <summary>
    /// Writes log in format: Error(Ошибка): <text>
    /// </summary>
    public static void LogWarning(string ruText, string enText)
    {
        bool isEnglish = TranslatorManager.Instance.language == TranslatorManager.Language.EN;
        string error = isEnglish ? $"Error: {enText}" : $"Ошибка: {ruText}";
        Instance.WriteLog(error, Color.red);
    }
    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(_hideTextDelay);
        _textMesh.text = "";
    }
}
