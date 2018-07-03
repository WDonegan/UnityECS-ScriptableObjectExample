using UnityEngine;
using Unity.Mathematics;
using Unity.Rendering;
using SOExample.Components.Transform;

namespace SOExample
{
    public partial class Main
    {
        /// <summary>
        /// Spawn a large group of objects using the given parameters.
        /// </summary>
        private static void SpawnTestEntities(int count, int positionRange, int materialCount, int minScale, int maxScale)
        {
            var mat = GetNewMaterial();

            for (int i = 0; i < count; i++)
            {
                var Cube = entityManager.CreateEntity(Managers.Archtype.Sphere);

                entityManager.SetComponentData(Cube, new Pos
                {
                    Value = new float3
                    {
                        x = Random.value * positionRange - positionRange / 2,
                        y = Random.value * positionRange - positionRange / 2,
                        z = Random.value * positionRange - positionRange / 2
                    }
                });
                entityManager.SetComponentData(Cube, new Rot { Value = quaternion.identity });

                var scl = Random.Range(minScale, maxScale);

                entityManager.SetComponentData(Cube, new Scl
                {

                    Value = new float3
                    {
                        x = scl,
                        y = scl,
                        z = scl
                    }
                });

                entityManager.SetComponentData(Cube, new ModelMatrix { Value = float4x4.identity });

                if (i % ((int)(count / materialCount)) == 0)
                    mat = GetNewMaterial();

                entityManager.SetSharedComponentData(Cube, new MeshInstanceRenderer
                {
                    mesh = (Resources.Load("CubeData") as DataObjects.MeshData).Value,
                    material = mat,
                    castShadows = UnityEngine.Rendering.ShadowCastingMode.On,
                    receiveShadows = true
                });
            }
        }

        /// <summary>
        /// Create a new material with a random color and instancing enabled.
        /// </summary>
        private static Material GetNewMaterial()
        {
            return new Material(Shader.Find("Standard")) { color = Random.ColorHSV(), enableInstancing = true };
        }
    }
}
