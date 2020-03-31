using System;
using UnityEngine;

namespace EnliStandardAssets.Localization
{
    public interface ILocalizationProvider
    {
        /// <summary>
        /// Returns localized string defined by key
        /// </summary>
        /// <param name="defaultString">Returns this if translation was not found</param>
        /// <returns></returns>
        string Translate(string key, string defaultString);

        /// <summary>
        /// Returns whether translation was successful, localized string is set to parameter "translation"
        /// </summary>
        bool TryGetTranslation(string key, out string translation);

        event Action LanguageChanged;

        string CurrentLanguage { get; }
        string CurrentLanguageCode { get; }
    }
}