using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageManager : SingleInstance<LanguageManager>
{
    public static LanguageData languageData;
    public static LanguageChinese Chinese;
    public static LanguageEnglish English;
    private void Awake()
    {
        if(languageData == null)
        {
            InitLanguage();
        }
    }
    void Start()
    {
        
    }
    void InitLanguage()
    {
        if(LanguageIsChinese())
        {
            Chinese = new LanguageChinese();
            languageData = Chinese;
        }
        else
        {
            English = new LanguageEnglish();
            languageData = Chinese;
        }
    }

    /// <summary>
    /// 获取设备设置的语言是否是中文
    /// </summary>
    public static bool LanguageIsChinese()
    {
        if (Application.systemLanguage == SystemLanguage.Chinese || Application.systemLanguage == SystemLanguage.ChineseSimplified || Application.systemLanguage == SystemLanguage.ChineseTraditional)
        {
            return true;
        }
        return false;
    }
}
public class LanguageData
{
    public string Level = "关卡 ";
}
public class LanguageChinese : LanguageData
{

}
public class LanguageEnglish : LanguageData
{
    public LanguageEnglish()
    {
        Level = "level ";
    }
}
