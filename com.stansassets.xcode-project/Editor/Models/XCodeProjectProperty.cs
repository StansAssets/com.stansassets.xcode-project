using UnityEngine;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// Represents XCode project build property.
    /// </summary>
    [System.Serializable]
    public class XCodeProjectProperty
    {
        [SerializeField]
        string m_Name;
        [SerializeField]
        string m_Value;
        [SerializeField]
        string[] m_Options = { "YES", "NO" };

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="name">The name of the build property.</param>
        /// <param name="value">The value of the build property.</param>
        public XCodeProjectProperty(string name, string value)
        {
            m_Name = name;
            m_Value = value;
        }

        /// <summary>
        /// The name of the build property.
        /// </summary>
        public string Name => m_Name;

        /// <summary>
        /// The value of the build property.
        /// </summary>
        public string Value
        {
            get
            {
                if (string.IsNullOrEmpty(m_Value)) m_Value = m_Options[0];

                return m_Value;
            }

            set => m_Value = value;
        }

        /// <summary>
        /// An array of suited values for this build property
        /// </summary>
        /// <value>The options.</value>
        public string[] Options => m_Options;
    }
}
