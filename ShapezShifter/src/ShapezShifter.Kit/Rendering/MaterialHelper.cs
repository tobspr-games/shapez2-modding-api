using UnityEngine;

namespace ShapezShifter.Kit
{
    public static class MaterialHelper
    {
        public static Material LoadMaterial(string assetBundlePath, string materialName)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            return assetBundle.LoadAsset<Material>(materialName);
        }
    }

    public static class ShaderHelper
    {
        public static Shader LoadShader(string assetBundlePath, string shaderName)
        {
            AssetBundle assetBundle = AssetBundle.LoadFromFile(assetBundlePath);

            return assetBundle.LoadAsset<Shader>(shaderName);
        }
    }
}