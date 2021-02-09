////////////////////////////////////////////////////////////////////////////////
//  
// @module IOS Deploy
// @author Stanislav Osipov (Stan's Assets) 
// @support support@stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using System.Reflection;
using System.ComponentModel;

namespace SA.iOS.XCode
{
    [System.Serializable]
    public class ISD_Library
    {
        public ISD_iOSLibrary Type;
        public bool IsOptional;

        public ISD_Library(ISD_iOSLibrary lib, bool optional = false)
        {
            Type = lib;
            IsOptional = optional;
        }

        /// <summary>
        /// Full library name
        /// </summary>
        /// <value>The name.</value>
        public string Name => ISD_LibHandler.stringValueOf(Type);
    }
}
