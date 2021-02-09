using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Foundation.Localization
{
    public static class SA_LanguagesUtil
    {
        static SA_ISOLanguagesList m_isoLanguages = null;

        /// <summary>
        /// Returns list of know IOS languages
        /// </summary>
        public static SA_ISOLanguagesList ISOLanguagesList
        {
            get
            {
                if (m_isoLanguages == null)
                {
                    var langs = Resources.Load("iso_languages") as TextAsset;
                    m_isoLanguages = JsonUtility.FromJson<SA_ISOLanguagesList>(langs.text);
                }

                return m_isoLanguages;
            }
        }
    }
}
