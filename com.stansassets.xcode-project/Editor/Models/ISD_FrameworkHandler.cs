using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Linq;

namespace SA.iOS.XCode
{
    public class ISD_FrameworkHandler : MonoBehaviour
    {
        static List<ISD_Framework> _DefaultFrameworks = null;

        public static List<ISD_Framework> AvailableFrameworks
        {
            get
            {
                var resultList = new List<ISD_Framework>();
                var strings = new List<string>(Enum.GetNames(typeof(ISD_iOSFramework)));
                foreach (var frmwrk in ISD_Settings.Instance.Frameworks)
                    if (strings.Contains(frmwrk.Type.ToString()))
                        strings.Remove(frmwrk.Type.ToString());
                foreach (var frmwrk in DefaultFrameworks)
                    if (strings.Contains(frmwrk.Type.ToString()))
                        strings.Remove(frmwrk.Type.ToString());
                foreach (ISD_iOSFramework v in Enum.GetValues(typeof(ISD_iOSFramework)))
                    if (strings.Contains(v.ToString()))
                        resultList.Add(new ISD_Framework((ISD_iOSFramework)v));
                return resultList;
            }
        }

        public static List<ISD_Framework> DefaultFrameworks
        {
            get
            {
                if (_DefaultFrameworks == null)
                {
                    _DefaultFrameworks = new List<ISD_Framework>();
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CoreText));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.AudioToolbox));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.AVFoundation));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CFNetwork));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CoreGraphics));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CoreMedia));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CoreMotion));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.CoreVideo));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.Foundation));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.OpenAL));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.OpenGLES));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.QuartzCore));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.SystemConfiguration));
                    _DefaultFrameworks.Add(new ISD_Framework(ISD_iOSFramework.UIKit));
                }

                return _DefaultFrameworks;
            }
        }

        public static string[] BaseFrameworksArray()
        {
            var array = new List<string>(AvailableFrameworks.Capacity);
            foreach (var framework in AvailableFrameworks) array.Add(framework.Type.ToString());
            return array.ToArray();
        }
    }
}
