using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;

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
                SpawnArea = sData.SpawnData.SpanwArea
            });

            var oldState = Random.state;
            Random.InitState(sData.SpawnData.Seed);

            entityManager.SetComponentData<Components.SpawnerState>(Spawner, new Components.SpawnerState
            {
                CurrentCount = 0,
                RandomState = Random.state
            });

            Random.state = oldState;
        }
        
    }
}