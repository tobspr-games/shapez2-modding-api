using System.Linq;
using Assimp;
using Mesh = UnityEngine.Mesh;

namespace ShapezShifter.Kit
{
    public static class FileMeshLoader
    {
        /// <summary>
        /// Convenient single method for loading a FBX that contains exactly one mesh and converting it to Unity's
        /// format. If you need more flexibility, check this method implementation and the <see cref="AssimpToUnityMeshConverter"/>
        /// </summary>
        public static Mesh LoadSingleMeshFromFile(string file)
        {
            using AssimpContext importer = new();

            //This is how we add a logging callback 
            LogStream logStream = new(delegate(string msg, string _) { Debugging.Logger.Info?.Log(msg); });
            logStream.Attach();

            Scene model = importer.ImportFile(file,
                PostProcessPreset.TargetRealTimeMaximumQuality);

            return AssimpToUnityMeshConverter.ConvertStaticMesh(model.Meshes.Single());
        }
    }
}