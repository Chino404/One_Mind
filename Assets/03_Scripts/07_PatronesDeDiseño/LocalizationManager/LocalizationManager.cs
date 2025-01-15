using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager instance;
    public SystemLanguage language;
    public DataLocalization[] data;

    public Dictionary<SystemLanguage, Dictionary<string, string>> _translate = new Dictionary<SystemLanguage, Dictionary<string, string>>();

    private void Awake()
    {
        if(instance!=null)
        {
            Destroy(gameObject);
        }
        else
        {
        instance = this;
        _translate = LanguageU.LoadTranslate(data);
            DontDestroyOnLoad(gameObject);
        }
    }

    public string GetTranslate(string ID)
    {
        if (!_translate.ContainsKey(language))
            return "No language";

        if (!_translate[language].ContainsKey(ID))
            return "No ID";

        return _translate[language][ID];
    }
}
