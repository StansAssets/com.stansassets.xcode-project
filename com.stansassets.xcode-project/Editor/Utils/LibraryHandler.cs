using UnityEngine;
using System.Collections.Generic;
using System;
using System.ComponentModel;

namespace StansAssets.IOS.XCode
{
    public class LibraryHandler : MonoBehaviour
    {
        public static List<XCodeLibrary> AvailableLibraries
        {
            get
            {
                var resultList = new List<XCodeLibrary>();
                var strings = new List<string>(Enum.GetNames(typeof(XCodeLibraryName)));
                foreach (var addedLibrary in XCodeProjectSettings.Instance.Libraries)
                    if (strings.Contains(addedLibrary.FullName))
                        strings.Remove(addedLibrary.FullName);

                foreach (XCodeLibraryName v in Enum.GetValues(typeof(XCodeLibraryName)))
                    if (strings.Contains(v.ToString()))
                        resultList.Add(new XCodeLibrary(v));
                return resultList;
            }
        }

        public static string[] BaseLibrariesArray()
        {
            var array = new List<string>(AvailableLibraries.Capacity);
            foreach (var library in AvailableLibraries) array.Add(library.FullName);
            return array.ToArray();
        }

        public static string StringValueOf(XCodeLibraryName value)
        {
#if !UNITY_WSA
            var fi = value.GetType().GetField(value.ToString());
            var attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
#else
			return string.Empty;
#endif
        }

        public static object EnumValueOf(string value)
        {
            var enumType = typeof(XCodeLibraryName);
            var names = Enum.GetNames(enumType);
            foreach (var name in names)
                if (StringValueOf((XCodeLibraryName)Enum.Parse(enumType, name)).Equals(value))
                    return Enum.Parse(enumType, name);

            throw new ArgumentException("The string is not a description or value of the specified enum...\n " + value + " is not right enum");
        }
    }
}
