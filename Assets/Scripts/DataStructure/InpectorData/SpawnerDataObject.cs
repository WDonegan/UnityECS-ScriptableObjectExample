using System;
using UnityEngine;
using Unity.Mathematics;

[Serializable]
public struct MinMax
{
    public float Min, Max;
}

[Serializable]
public struct SpawnExtents
{
    public MinMax X_Axis;
    public MinMax Y_Axis;
    public MinMax Z_Axis;
}

[Serializable]
public struct SpawnData
{
    public int SpawnCountMax;
    public int SpawnBatchSize;
    
    public float3 SpawnOrigin;
    public SpawnExtents SpawnExtents;
    }

[CreateAssetMenu(fileName = "SpawnerData", menuName = "SpawnerData Object")]
public class SpawnerDataObject : ScriptableObject
{
    public int Seed;
    public float SpawnerCooldown;
    public SpawnData Value;
}