using Game.Core.Coordinates;
using UnityEngine;

namespace ShapezShifter.Rendering
{
    public class BoundingBoxHelper
    {
        /// <remarks>
        /// Consider setting colliders manually for more complicated shapes or more accuracy
        /// </remarks>
        public static BuildingCollisionBox[] CreateBasicCollider(Mesh mesh)
        {
            return new[]
            {
                new BuildingCollisionBox(new SerializedBuildingCollisionBox
                {
                    Center_L = new LocalVector(mesh.bounds.center),
                    Dimensions_L = mesh.bounds.size
                })
            };
        }
    }
}