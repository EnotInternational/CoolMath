using TMPro;
using UnityEngine;

public class TextTranslator : MonoBehaviour
{
    private TextMeshProUGUI text;
    public string RuText;
    public string EngText;
    private TranslatorManager.Language _language;
    /// <summary>
    /// Принудительная проверка смены текста, не обязательно вызываеть её вручную
    /// </summary>
    public void UpdateLanguage()
    {
        SetLanguage(TranslatorManager.Instance.language);
    }
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        SetLanguage(TranslatorManager.Instance.language);
        TranslatorManager.Instance.OnLanguageSet.AddListener(SetLanguage);
    }
    void OnEnable()
    {
        SetLanguage(TranslatorManager.Instance.language);
        TranslatorManager.Instance.OnLanguageSet.AddListener(SetLanguage);
    }
    void OnDisable()
    {
        TranslatorManager.Instance.OnLanguageSet.RemoveListener(SetLanguage);
    }
    void SetLanguage(TranslatorManager.Language language)
    {
        _language = language;

        if(language == TranslatorManager.Language.RU)
        {
            text.text = RuText;
        }
        else
        {
            text.text = EngText;
        }
    }
    
}
