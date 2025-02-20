using UnityEngine;
using UnityEngine.Events;

public class TranslatorManager : MonoBehaviour
{
    public static TranslatorManager Instance {get; private set;}
    private Language _language = Language.RU;
    public Language language
    {
        get => _language;
    }
    public UnityEvent<Language> OnLanguageSet = new UnityEvent<Language>();
    private void Awake()
    {
        Instance = this;
    }
    public void SetRU()
    {
        _language = Language.RU;
        OnLanguageSet.Invoke(Language.RU);
    }
    public void SetEN()
    {
        _language = Language.EN;
        OnLanguageSet.Invoke(Language.EN);
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public enum Language{ RU, EN}
}
