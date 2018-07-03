using UnityEngine;
using Unity.Entities;

namespace SOExample.Managers
{
    public static class Archtype
    {
        public static EntityArchetype Spawner;
        public static EntityArchetype Sphere;
        public static EntityArchetype GravitySphere;
        
        public static void Initialize(EntityManager entityManager)
        {
            Spawner = entityManager.CreateArchetype(
                typeof(Components.SpawnerCooldown),
                typeof(Components.SpawnerData),
                typeof(Components.SpawnerState));

            Sphere = entityManager.CreateArchetype(
                typeof(Components.Transform.Pos),
                typeof(Components.Transform.Rot),
                typeof(Components.Transform.Scl),
                typeof(Components.Transform.ModelMatrix),
                typeof(Unity.Rendering.MeshInstanceRenderer));

            GravitySphere = entityManager.CreateArchetype(
                typeof(Components.Transform.Pos),
                typeof(Components.Transform.Rot),
                typeof(Components.Transform.Scl),
                typeof(Components.Transform.Mass),
                typeof(Components.Transform.Velocity),
                typeof(Components.Transform.ModelMatrix),
                typeof(Unity.Rendering.MeshInstanceRenderer));
        }
    }

}
