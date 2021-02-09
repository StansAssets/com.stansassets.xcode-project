using System.Collections.Generic;
using System;

namespace StansAssets.IOS.XCode
{
    public static class FrameworkHandler
    {
        static List<XCodeFramework> s_DefaultFrameworks = null;

        public static List<XCodeFramework> AvailableFrameworks
        {
            get
            {
                var resultList = new List<XCodeFramework>();
                var strings = new List<string>(Enum.GetNames(typeof(XCodeFrameworkName)));
                foreach (var frmwrk in XCodeProjectSettings.Instance.Frameworks)
                    if (strings.Contains(frmwrk.FrameworkName.ToString()))
                        strings.Remove(frmwrk.FrameworkName.ToString());
                foreach (var frmwrk in DefaultFrameworks)
                    if (strings.Contains(frmwrk.FrameworkName.ToString()))
                        strings.Remove(frmwrk.FrameworkName.ToString());
                foreach (XCodeFrameworkName v in Enum.GetValues(typeof(XCodeFrameworkName)))
                    if (strings.Contains(v.ToString()))
                        resultList.Add(new XCodeFramework(v));
                return resultList;
            }
        }

        public static List<XCodeFramework> DefaultFrameworks
        {
            get
            {
                if (s_DefaultFrameworks == null)
                {
                    s_DefaultFrameworks = new List<XCodeFramework>();
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CoreText));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.AudioToolbox));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.AVFoundation));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CFNetwork));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CoreGraphics));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CoreMedia));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CoreMotion));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.CoreVideo));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.Foundation));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.OpenAL));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.OpenGLES));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.QuartzCore));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.SystemConfiguration));
                    s_DefaultFrameworks.Add(new XCodeFramework(XCodeFrameworkName.UIKit));
                }

                return s_DefaultFrameworks;
            }
        }

        public static string[] BaseFrameworksArray()
        {
            var array = new List<string>(AvailableFrameworks.Capacity);
            foreach (var framework in AvailableFrameworks) array.Add(framework.FrameworkName.ToString());
            return array.ToArray();
        }
    }
}
