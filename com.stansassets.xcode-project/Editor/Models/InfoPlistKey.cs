////////////////////////////////////////////////////////////////////////////////
//
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets)
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using StansAssets.Foundation;

namespace StansAssets.IOS.XCode
{
    /// <summary>
    /// Represents XCode project info.plist key.
    /// </summary>
    [System.Serializable]
    public class InfoPlistKey
    {
        //Editor Use Only
        internal bool IsOpen = true;
        internal bool IsListOpen = true;

        /// <summary>
        /// Info.plist Key name
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Info.plist key type
        /// </summary>
        public InfoPlistKeyType Type = InfoPlistKeyType.String;

        /// <summary>
        /// Info.plist key string value
        /// </summary>
        public string StringValue = string.Empty;

        /// <summary>
        /// Info.plist key int value
        /// </summary>
        public int IntegerValue = 0;

        /// <summary>
        /// Info.plist key float value
        /// </summary>
        public float FloatValue = 0;

        /// <summary>
        /// Info.plist key bool value
        /// </summary>
        public bool BooleanValue = true;

        /// <summary>
        /// Id's of nested keys
        /// </summary>
        public List<string> ChildrenIds = new List<string>();

        /// <summary>
        /// Adds child key
        /// </summary>
        /// <param name="childKey"></param>
        public void AddChild(InfoPlistKey childKey)
        {
            if (Type.Equals(InfoPlistKeyType.Dictionary))
                foreach (var childId in ChildrenIds)
                {
                    var var = XCodeProjectSettings.Instance.GetVariableById(childId);
                    if (var.Name.Equals(childKey.Name))
                    {
                        XCodeProjectSettings.Instance.RemoveVariable(var, ChildrenIds);
                        break;
                    }
                }

            var keyId = IdFactory.RandomString;
            XCodeProjectSettings.Instance.AddVariableToDictionary(keyId, childKey);
            ChildrenIds.Add(keyId);
        }

        public void RemoveChild(InfoPlistKey childKey)
        {
            XCodeProjectSettings.Instance.RemoveVariable(childKey, ChildrenIds);
        }

        public InfoPlistKey GetChildByStringValue(string val)
        {
            foreach (var child in Children)
                if (child.StringValue.Equals(val))
                    return child;

            return null;
        }

        public List<InfoPlistKey> Children
        {
            get
            {
                var children = new List<InfoPlistKey>();

                foreach (var keyId in ChildrenIds)
                {
                    var key = XCodeProjectSettings.Instance.GetVariableById(keyId);
                    children.Add(key);
                }

                return children;
            }
        }

        public void Clear()
        {
            ChildrenIds.Clear();
        }
    }
}
