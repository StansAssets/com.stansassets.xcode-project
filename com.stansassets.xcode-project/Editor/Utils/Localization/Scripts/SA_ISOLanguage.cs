using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.Foundation.Localization
{
    [System.Serializable]
    public class SA_ISOLanguage
    {
        [SerializeField]
        string m_code = string.Empty;
        [SerializeField]
        string m_name = string.Empty;
        [SerializeField]
        string m_nativeName = string.Empty;

        /// <summary>
        /// ISO Language code
        /// </summary>
        public string Code => m_code;

        /// <summary>
        /// Full Languages name. Example: Russian
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// Full Languages name. Example: Русский
        /// </summary>
        public string NativeName => m_nativeName;
    }
}
