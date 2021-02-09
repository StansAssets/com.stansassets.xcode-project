using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Foundation.Localization
{
    [System.Serializable]
    public class SA_ISOLanguagesList
    {
        [SerializeField]
        List<SA_ISOLanguage> m_lanuages = new List<SA_ISOLanguage>();

        List<string> m_names = null;

        /// <summary>
        /// Returns list of know IOS languages
        /// </summary>
        public List<SA_ISOLanguage> Languages => m_lanuages;

        /// <summary>
        /// Returns list of know IOS languages names
        /// </summary>
        public List<string> Names
        {
            get
            {
                if (m_names == null)
                {
                    m_names = new List<string>();
                    foreach (var lang in m_lanuages) m_names.Add(lang.Name);
                }

                return m_names;
            }
        }
    }
}
