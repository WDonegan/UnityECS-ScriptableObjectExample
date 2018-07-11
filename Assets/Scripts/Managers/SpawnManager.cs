using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

using SOExample.Components;
using SOExample.Components.Transform;

namespace SOExample.Managers
{
    public static class SpawnManager
    {
        public static Entity Spawner;

        public static void InitializeSpawnSystem(EntityManager entityManager)
        {
            Spawner = entityManager.CreateEntity(Managers.Archtype.Spawner);
            
            DataObjects.SpawnerData sData = Resources.Load("SpawnerData") as DataObjects.SpawnerData;

            entityManager.SetComponentData<Components.SpawnerCooldown>(Spawner, new Components.SpawnerCooldown
            {
                Value = 0.0f
            });

            entityManager.SetComponentData<Components.SpawnerData>(Spawner, new Components.SpawnerData
            {
                Seed = sData.SpawnData.Seed,
                CooldownInterval = sData.SpawnData.Cooldown,
                SpawnAmount = sData.SpawnData.SpawnAmount,
                SpawnArea = sData.SpawnData.SpawnArea
            });

            var oldState = Random.state;
            Random.InitState(sData.SpawnData.Seed);

            entityManager.SetComponentData<Components.SpawnerState>(Spawner, new Components.SpawnerState
            {
                CurrentCount = 0,
                RandomState = Random.state
            });

            Random.state = oldState;

            SpawnSingleStar();
        }

        public static void SpawnSingleStar()
        {
            
            var SUN = Main.entityManager.CreateEntity(Archtype.GravitySphere);

            Main.entityManager.SetComponentData(SUN, new Pos
            {
                Value = new float3 {  x = 0, y = 0, z = 0 }
            });

            Main.entityManager.SetComponentData(SUN, new Rot
            {
                Value = quaternion.identity
            });
            
            Main.entityManager.SetComponentData(SUN, new Scl
            {
                Value = new float3 { x = 1, y = 1, z = 1 } * 10
            });

            Main.entityManager.SetComponentData(SUN, new Mass
            {
                Value = 5000
            });

            Main.entityManager.SetComponentData(SUN, new Velocity
            {
                Value = new float3 { x = 0, y = 0, z = 0 }
            });

            Main.entityManager.SetComponentData(SUN, new ModelMatrix
            {
                Value = float4x4.identity
            });

            Main.entityManager.SetSharedComponentData(SUN, new Unity.Rendering.MeshInstanceRenderer
            {
                material = new Material(Shader.Find("Standard")) {
                    color = Color.Lerp(Color.red, Color.yellow, 0.5f),
                    enableInstancing = true
                },
                mesh = (Resources.Load("CubeData") as DataObjects.MeshData).Value,

                receiveShadows = true,
                castShadows = UnityEngine.Rendering.ShadowCastingMode.On
            });
        }
        
    }
}