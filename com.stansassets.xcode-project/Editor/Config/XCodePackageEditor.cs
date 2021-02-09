using StansAssets.Foundation.Editor;

namespace StansAssets.IOS.XCode
{
    static class XCodePackageEditor
    {
        public static readonly string RootPath = PackageManagerUtility.GetPackageRootPath(XCodePackage.PackageName);
    }
}
