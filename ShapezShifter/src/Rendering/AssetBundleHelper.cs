using System;
using System.IO;
using UnityEngine;
using Object = UnityEngine.Object;

namespace ShapezShifter.Rendering
{
    /// <remarks>
    /// Simple wrapper around asset bundles that allow modders to interact with them without importing the
    /// UnityEngine.AssetBundleModule assembly
    /// </remarks>
    public class AssetBundleHelper : IDisposable
    {
        public static AssetBundleHelper CreateForAssetBundleEmbeddedWithMod<TMod>(string relativePath = "Resources")
            where TMod : IMod
        {
            return new AssetBundleHelper(Path.Combine(ModDirectoryLocator.GetDirectoryLocation<TMod>(), relativePath));
        }

        private readonly AssetBundle AssetBundle;

        public AssetBundleHelper(string assetBundlePath)
        {
            AssetBundle = AssetBundle.LoadFromFile(assetBundlePath);
        }

        public T LoadAsset<T>(string name)
            where T : Object
        {
            return AssetBundle.LoadAsset<T>(name);
        }

        public void Dispose()
        {
            AssetBundle.Unload(true);
        }
    }
}