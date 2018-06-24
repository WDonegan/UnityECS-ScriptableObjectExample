using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;



#region Physics Components

public struct Mass : IComponentData
{
    public float Value;
}

public struct Velocity : IComponentData
{
    public float3 Value;
}
#endregion

#region BigBang Spawn Components

public struct BigBangSpawnCooldown : IComponentData
{
    public float Value;
}

public struct BigBangSpawnData : IComponentData
{
    public SpawnData Value;
}

public struct BigBangSpawnerState : IComponentData
{
    public int SpawnedEntitiesCount;
    public float CooldownInterval;
    public Random.State RandomState;
}

#endregion