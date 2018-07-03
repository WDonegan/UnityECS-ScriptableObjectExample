using UnityEngine;
using Unity.Mathematics;

namespace SOExample.DataObjects
{
    [System.Serializable]
    public struct MinMax
    {
        public float Min, Max;
    }

    [System.Serializable]
    public struct EntityDetail
    {
        public MinMax MassRange;
        public float ScaleToMassRatio;
    }

    [System.Serializable]
    public struct SpawnArea
    {
        public float3 Origin;
        public MinMax Radius;
    }

    [System.Serializable]
    public struct SpawnAmount
    {
        public int MaxCount;
        public int BatchSize;
    }

    [System.Serializable]
    public struct SpawnData
    {
        public int Seed;
        public float Cooldown;
        public SpawnAmount SpawnAmount;
        public SpawnArea SpanwArea;
    }

    [CreateAssetMenu(fileName = "SpawnerData", menuName = "SpawnerSetup")]
    public class SpawnerData : ScriptableObject
    {
        public SpawnData SpawnData;
    }
}