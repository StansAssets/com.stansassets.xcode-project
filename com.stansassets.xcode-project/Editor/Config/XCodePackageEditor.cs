using StansAssets.Foundation.Editor;
using StansAssets.IOS.XCode;

namespace SA.iOS.XCode
{
    static class XCodePackageEditor
    {
        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(XCodePackage.PackageName);
    }
}
