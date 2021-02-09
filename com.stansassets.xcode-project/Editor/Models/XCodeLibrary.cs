////////////////////////////////////////////////////////////////////////////////
//
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets)
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// XCode project library.
    /// </summary>
    [System.Serializable]
    public class XCodeLibrary
    {
        XCodeLibraryName m_LibraryName;
        bool m_IsOptional;

        /// <summary>
        /// Library Name.
        /// </summary>
        public XCodeLibraryName Name
        {
            get => m_LibraryName;
            internal set => m_LibraryName = value;
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
        /// Full library name
        /// </summary>
        /// <value>The name.</value>
        public string FullName => LibraryHandler.StringValueOf(m_LibraryName);

        public XCodeLibrary(XCodeLibraryName lib, bool optional = false)
        {
            m_LibraryName = lib;
            m_IsOptional = optional;
        }
    }
}
