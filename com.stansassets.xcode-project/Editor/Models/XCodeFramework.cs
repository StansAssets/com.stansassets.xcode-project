using StansAssets.Foundation;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// XCode project library.
    /// </summary>
    [System.Serializable]
    public class XCodeFramework
    {
        bool m_IsOptional;
        bool m_IsEmbeded;

        public XCodeFrameworkName FrameworkName;

        /// <summary>
        /// Library Name.
        /// </summary>
        public XCodeFrameworkName Name
        {
            get => FrameworkName;
            internal set => FrameworkName = value;
        }

        /// <summary>
        /// Defines if dependency is optional or not.
        /// </summary>
        public bool IsOptional
        {
            get => m_IsOptional;
            internal set => m_IsOptional = value;
        }

        /// <summary>
        /// Defines if framework is embeded or not.
        /// </summary>
        public bool IsEmbeded
        {
            get => m_IsEmbeded;
            internal set => m_IsEmbeded = value;
        }

        /// <summary>
        /// Gets the full framework name.
        /// </summary>
        public string FullName => FrameworkName + ".framework";

        /// <summary>
        /// Initializes a new instance of the class.
        /// </summary>
        /// <param name="frameworkName">Type.</param>
        /// <param name="optional">If set to <c>true</c> optional.</param>
        public XCodeFramework(XCodeFrameworkName frameworkName, bool optional = false)
        {
            FrameworkName = frameworkName;
            m_IsOptional = optional;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:StansAssets.IOS.XCode.ISD_Framework"/> class.
        /// </summary>
        /// <param name="frameworkName">Framework name.</param>
        /// <param name="optional">If set to <c>true</c> optional.</param>
        public XCodeFramework(string frameworkName, bool optional = false)
        {
            frameworkName = frameworkName.Replace(".framework", string.Empty);
            FrameworkName = EnumUtility.ParseEnum<XCodeFrameworkName>(frameworkName);

            m_IsOptional = optional;
        }
    }
}
