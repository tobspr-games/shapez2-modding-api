using System.Collections.Generic;
using UnityEngine;

namespace ShapezShifter.Rendering
{
    public static class MeshLod
    {
        public static IEmptyMeshBuilder Create()
        {
            return new LodMeshBuilder();
        }
    }

    public class LodMeshBuilder : IEmptyMeshBuilder,
        ILodMesh1Builder,
        ILodMesh2Builder,
        ILodMesh3Builder,
        ILodMesh4Builder,
        ILodMesh5Builder
    {
        private readonly List<Mesh> Meshes = new();

        public ILodMesh1Builder AddLod0Mesh(Mesh mesh)
        {
            EnsureCapacity(1);
            Meshes[0] = mesh;
            return this;
        }

        public ILodMesh2Builder AddLod1Mesh(Mesh mesh)
        {
            EnsureCapacity(2);
            Meshes[1] = mesh;
            return this;
        }

        public ILodMesh2Builder UseLod0AsLod1()
        {
            AddLod1Mesh(MeshAt(0));
            return this;
        }

        public ILodMesh3Builder AddLod2Mesh(Mesh mesh)
        {
            EnsureCapacity(3);
            Meshes[2] = mesh;
            return this;
        }

        public ILodMesh3Builder UseLod1AsLod2()
        {
            AddLod2Mesh(MeshAt(1));
            return this;
        }

        public ILodMesh4Builder AddLod3Mesh(Mesh mesh)
        {
            EnsureCapacity(4);
            Meshes[3] = mesh;
            return this;
        }

        public ILodMesh4Builder UseLod2AsLod3()
        {
            AddLod3Mesh(MeshAt(2));
            return this;
        }

        public ILodMesh5Builder AddLod4Mesh(Mesh mesh)
        {
            EnsureCapacity(5);
            Meshes[4] = mesh;
            return this;
        }

        public ILodMesh5Builder UseLod3AsLod4()
        {
            AddLod4Mesh(MeshAt(3));
            return this;
        }


        public ILodMeshBuilder AddLod5Mesh(Mesh mesh)
        {
            EnsureCapacity(6);
            Meshes[5] = mesh;
            return this;
        }

        public ILodMeshBuilder UseLod4AsLod5()
        {
            AddLod5Mesh(MeshAt(4));
            return this;
        }

        public LOD2Mesh BuildLOD2Mesh()
        {
            return new LOD2Mesh
            {
                LODClose = CreateRef(MeshAt(0)),
                LODNormal = CreateRef(MeshAt(1))
            };
        }

        public LOD3Mesh BuildLod3Mesh()
        {
            return new LOD3Mesh
            {
                LODClose = CreateRef(MeshAt(0)),
                LODNormal = CreateRef(MeshAt(1)),
                LODFar = CreateRef(MeshAt(2))
            };
        }

        public LOD4Mesh BuildLod4Mesh()
        {
            return new LOD4Mesh
            {
                LODClose = CreateRef(MeshAt(0)),
                LODNormal = CreateRef(MeshAt(1)),
                LODFar = CreateRef(MeshAt(2)),
                LODMinimal = CreateRef(MeshAt(3))
            };
        }

        public LOD5Mesh BuildLod5Mesh()
        {
            return new LOD5Mesh
            {
                LODClose = CreateRef(MeshAt(0)),
                LODNormal = CreateRef(MeshAt(1)),
                LODFar = CreateRef(MeshAt(2)),
                LODMinimal = CreateRef(MeshAt(3)),
                LODOverview = CreateRef(MeshAt(4))
            };
        }

        public LOD6Mesh BuildLod6Mesh()
        {
            return new LOD6Mesh
            {
                LODClose = CreateRef(MeshAt(0)),
                LODNormal = CreateRef(MeshAt(1)),
                LODFar = CreateRef(MeshAt(2)),
                LODMinimal = CreateRef(MeshAt(3)),
                LODOverview = CreateRef(MeshAt(4)),
                LODReduced = CreateRef(MeshAt(5))
            };
        }

        private UnityMeshReference CreateRef(Mesh mesh)
        {
            return new UnityMeshReference
            {
                _Mesh = mesh,
                _Handle = new UnityMeshReference.InitializedHandle(mesh),
                _Initialized = true
            };
        }

        private Mesh MeshAt(int lod)
        {
            return Meshes.Count <= lod ? Meshes[^1] : Meshes[lod];
        }

        private void EnsureCapacity(int capacity)
        {
            while (Meshes.Count < capacity)
            {
                Meshes.Add(null);
            }
        }
    }

    public interface ILodMeshBuilder
    {
        LOD2Mesh BuildLOD2Mesh();
        LOD3Mesh BuildLod3Mesh();
        LOD4Mesh BuildLod4Mesh();
        LOD5Mesh BuildLod5Mesh();
        LOD6Mesh BuildLod6Mesh();
    }


    public interface IEmptyMeshBuilder
    {
        ILodMesh1Builder AddLod0Mesh(Mesh mesh);
    }

    public interface ILodMesh1Builder : ILodMeshBuilder
    {
        ILodMesh2Builder AddLod1Mesh(Mesh mesh);
        ILodMesh2Builder UseLod0AsLod1();
    }

    public interface ILodMesh2Builder : ILodMeshBuilder
    {
        ILodMesh3Builder AddLod2Mesh(Mesh mesh);
        ILodMesh3Builder UseLod1AsLod2();
    }

    public interface ILodMesh3Builder : ILodMeshBuilder
    {
        ILodMesh4Builder AddLod3Mesh(Mesh mesh);
        ILodMesh4Builder UseLod2AsLod3();
    }

    public interface ILodMesh4Builder : ILodMeshBuilder
    {
        ILodMesh5Builder AddLod4Mesh(Mesh mesh);
        ILodMesh5Builder UseLod3AsLod4();
    }

    public interface ILodMesh5Builder : ILodMeshBuilder
    {
        ILodMeshBuilder AddLod5Mesh(Mesh mesh);
        ILodMeshBuilder UseLod4AsLod5();
    }
}