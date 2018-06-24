using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Rendering;


[UpdateBefore(typeof(MeshInstanceRenderer))]
class InitialEntitySpawnerSystem : ComponentSystem
{
    struct State
    {
        public int Length;
        public ComponentDataArray<BigBangSpawnCooldown> Cooldown;
        public ComponentDataArray<BigBangSpawnData> Data;
        public ComponentDataArray<BigBangSpawnerState> S;
    }

    [Inject] State m_State;

    public static void SetupComponentData(EntityManager entityManager, SpawnerDataObject spawnData)
    {
        var arch = entityManager.CreateArchetype(
            typeof(BigBangSpawnCooldown),
            typeof(BigBangSpawnData),
            typeof(BigBangSpawnerState));
        var stateEntity = entityManager.CreateEntity(arch);
        var oldState = Random.state;
        Random.InitState(spawnData.Seed);

        entityManager.SetComponentData(stateEntity, new BigBangSpawnCooldown { Value = 0.0f });
        entityManager.SetComponentData(stateEntity, new BigBangSpawnData { Value = spawnData.Value });
        entityManager.SetComponentData(stateEntity, new BigBangSpawnerState
        {
            SpawnedEntitiesCount = 0,
            CooldownInterval = spawnData.SpawnerCooldown,
            RandomState = Random.state
        });
        Random.state = oldState;
    }

    protected override void OnUpdate()
    {
        // Process Cooldown
        float cooldown = m_State.Cooldown[0].Value;
        cooldown = Mathf.Max(0.0f, m_State.Cooldown[0].Value - Time.deltaTime);
        bool spawn = cooldown <= 0.0f;

        if (spawn)
        {
            cooldown = ComputeCooldown();
        }

        m_State.Cooldown[0] = new BigBangSpawnCooldown { Value = cooldown };
        // End Cooldown Processing

        // Process Spawn Batch
        if (spawn)
        {
            var state = m_State.S[0];
            SpawnData data = m_State.Data[0].Value;

            // Exit earily logic
            bool spawnsRemaining = state.SpawnedEntitiesCount < data.SpawnCountMax;

            if (spawnsRemaining)
            {
                var spawnsLeft = data.SpawnCountMax - state.SpawnedEntitiesCount;
 
                
                for (var i = 0; i < Mathf.Min(data.SpawnBatchSize, spawnsLeft); i++)
                {
                    SpawnEntity();
                }
            }
        }
        // End Spawn Processing
    }

    void SpawnEntity()
    {
        var state = m_State.S[0];
        var oldState = Random.state;
        Random.state = state.RandomState;

        state.SpawnedEntitiesCount++;

        PostUpdateCommands.CreateEntity(Bootstrap.MovableArchetype);

        var Heading = ComputeHeading();

        PostUpdateCommands.SetComponent(new Position { Value = Heading * (ComputeSpeed() + 8) });

        var massColor = Random.Range(0, Bootstrap.cubeRenderData.Materials.Value.Length);

        PostUpdateCommands.SetSharedComponent(new MeshInstanceRenderer {
            mesh = Bootstrap.cubeRenderData.Mesh.Value,
            material = Bootstrap.cubeRenderData.Materials.Value[ massColor ]
        });

        PostUpdateCommands.SetComponent(new Mass { Value = massColor + 1f });
        PostUpdateCommands.SetComponent(new Velocity { Value = Heading * (massColor + 8f) });

        state.RandomState = Random.state;

        m_State.S[0] = state;
        Random.state = oldState;
    }

    float ComputeCooldown()
    {
        return m_State.S[0].CooldownInterval;
    }

    float3 ComputeHeading()
    {
        const float min = -2.0f * Mathf.PI;
        const float max =  2.0f * Mathf.PI;


        return new float3
        {
            x = math.cos(Random.Range(min, max)),
            y = math.sin(Random.Range(min, max)),
            z = Random.Range(-1f, 1f)
        };
    }

    float ComputeSpeed()
    {
        return 4f; //Random.Range(4f, 4f);
    }
}
