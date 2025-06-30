using Global.Patching;
using Semver;

public static class ModSignatureExtensionMethods
{
    public static SemVersion SemVer(this ModSignature signature)
    {
        return new SemVersion(signature.Major, signature.Minor, signature.Patch);
    }
}