using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA.iOS.XCode
{
    [System.Serializable]
    public class ISD_BuildProperty
    {
        [SerializeField]
        string m_name;
        [SerializeField]
        string m_value;
        [SerializeField]
        string[] m_options = new string[] { "YES", "NO" };

        /// <summary>
        /// Initializes a new instance of the <see cref="T:SA.iOS.XCode.ISD_BuildProperty"/> class.
        /// </summary>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public ISD_BuildProperty(string name, string value)
        {
            m_name = name;
            m_value = value;
        }

        /// <summary>
        /// The name of the build property.
        /// </summary>
        public string Name => m_name;

        /// <summary>
        /// The value of the build property.
        /// </summary>
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(m_value)) m_value = m_options[0];

                return m_value;
            }

            set => m_value = value;
        }

        /// <summary>
        /// An array of suited values for this build property
        /// </summary>
        /// <value>The options.</value>
        public string[] Options => m_options;
    }
}
