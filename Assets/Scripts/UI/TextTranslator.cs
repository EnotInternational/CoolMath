using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextTranslator : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI textTMP;
    [SerializeField]private Text text;
    public string RuText;
    public string EngText;
    [SerializeField]private TranslatorManager.Language _language;
    /// <summary>
    /// Принудительная проверка смены текста, не обязательно вызываеть её вручную
    /// </summary>
    public void UpdateLanguage()
    {
        SetLanguage(TranslatorManager.Instance.language);
    }
    void Awake()
    {
        if(!TryGetComponent<TextMeshProUGUI>(out textTMP))
        {
            text =  GetComponent<Text>();
        }
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
            SetText(RuText);
        }
        else
        {
            SetText(EngText);
        }
    }
    private void SetText(string message)
    {
        if(textTMP)
        {
            textTMP.text = message;
        }
        else
        {
            text.text = message;
        }

    }
    
}
