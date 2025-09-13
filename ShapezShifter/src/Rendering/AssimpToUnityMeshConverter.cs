using System.Collections.Generic;
using Assimp;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using Mesh = UnityEngine.Mesh;

namespace ShapezShifter.Rendering
{
    public static class AssimpToUnityMeshConverter
    {
        public static Mesh ConvertStaticMesh(Assimp.Mesh source)
        {
            Mesh target = new()
            {
                name = source.m_name
            };

            ConvertVertices(source, target);

            ConvertFaces(source, target);

            ConvertNormals(source, target);

            ConvertUVs(source, target);

            return target;
        }


        private static void ConvertVertices(Assimp.Mesh source, Mesh target)
        {
            // TODO: this can be drastically optimized using the native pointers, but careful with the X axis flipping
            var meshVertices = new NativeArray<float3>(source.m_vertices.Count, Allocator.Temp);
            for (int i = 0; i < source.m_vertices.Count; i++)
            {
                Vector3D v = source.m_vertices[i];
                meshVertices[i] = new float3(-v.X, v.Y, v.Z);
            }

            target.SetVertices(meshVertices);
        }

        private static void ConvertFaces(Assimp.Mesh source, Mesh target)
        {
            using var indices = new NativeList<int>(Allocator.Temp);

            for (int i = 0; i < source.m_faces.Count; i++)
            {
                Face f = source.m_faces[i];
                if (f.IndexCount != 3)
                {
                    // Ignore anything that is not a triangle
                    continue;
                }

                indices.Add(f.Indices[2]);
                indices.Add(f.Indices[1]);
                indices.Add(f.Indices[0]);
            }

            target.SetIndices(indices.AsArray(), MeshTopology.Triangles, 0);
        }

        private static void ConvertNormals(Assimp.Mesh source, Mesh target)
        {
            var meshNormals = new NativeArray<float3>(source.m_normals.Count, Allocator.Temp);
            for (int i = 0; i < source.m_normals.Count; i++)
            {
                Vector3D n = source.m_normals[i];
                meshNormals[i] = new float3(-n.X, n.Y, n.Z);
            }

            target.SetNormals(meshNormals);
        }

        private static void ConvertUVs(Assimp.Mesh source, Mesh target)
        {
            for (int channel = 0; channel < source.m_texCoords.Length; channel++)
            {
                if (!source.HasTextureCoords(channel))
                {
                    continue;
                }

                List<Vector3D> sourceUv = source.TextureCoordinateChannels[channel];
                var uvs = new NativeArray<float2>(source.m_normals.Count, Allocator.Temp);

                for (int i = 0; i < source.m_normals.Count; i++)
                {
                    Vector3D n = sourceUv[i];
                    uvs[i] = new float2(n.X, n.Y);
                }

                target.SetUVs(channel, uvs);
            }
        }
    }
}