using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace SOExample.Components
{
    public struct Active : IComponentData
    {
        float Value;
    }

    public struct SpawnerCooldown : IComponentData
    {
        public float Value;
    }

    public struct SpawnerData : IComponentData
    {
        public int Seed;
        public float CooldownInterval;
        public DataObjects.SpawnAmount SpawnAmount;
        public DataObjects.SpawnArea SpawnArea;
    }

    public struct SpawnerState : IComponentData
    {
        public float CurrentCount;
        public Random.State RandomState;
    }
}
