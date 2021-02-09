////////////////////////////////////////////////////////////////////////////////
//
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets)
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using StansAssets.Foundation;

namespace SA.iOS.XCode
{
    [System.Serializable]
    public class ISD_PlistKey
    {
        //Editor Use Only
        public bool IsOpen = true;
        public bool IsListOpen = true;

        /// <summary>
        /// Info.plist Key name
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Info.plist key type
        /// </summary>
        public ISD_PlistKeyType Type = ISD_PlistKeyType.String;

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
        public List<string> ChildrensIds = new List<string>();

        /// <summary>
        /// Add's child key
        /// </summary>
        /// <param name="childKey"></param>
        public void AddChild(ISD_PlistKey childKey)
        {
            if (Type.Equals(ISD_PlistKeyType.Dictionary))
                foreach (var ChildsId in ChildrensIds)
                {
                    var var = ISD_Settings.Instance.getVariableById(ChildsId);
                    if (var.Name.Equals(childKey.Name))
                    {
                        ISD_Settings.Instance.RemoveVariable(var, ChildrensIds);
                        break;
                    }
                }

            var keyId = IdFactory.RandomString;
            ISD_Settings.Instance.AddVariableToDictionary(keyId, childKey);
            ChildrensIds.Add(keyId);
        }

        public void RemoveChild(ISD_PlistKey childKey)
        {
            ISD_Settings.Instance.RemoveVariable(childKey, ChildrensIds);
        }

        public ISD_PlistKey GetChildByStringValue(string val)
        {
            foreach (var child in Children)
                if (child.StringValue.Equals(val))
                    return child;

            return null;
        }

        public List<ISD_PlistKey> Children
        {
            get
            {
                var children = new List<ISD_PlistKey>();

                foreach (var keyId in ChildrensIds)
                {
                    var key = ISD_Settings.Instance.getVariableById(keyId);
                    children.Add(key);
                }

                return children;
            }
        }

        public void Clear()
        {
            ChildrensIds.Clear();
        }
    }
}
