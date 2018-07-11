using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Jobs;

using SOExample;
using SOExample.Components;
using SOExample.Components.Transform;
using SOExample.Managers;

namespace SOExample.Systems
{
    [UpdateBefore(typeof(UpdateGroups.RenderingGroup))]
    public class SpawnSystem : ComponentSystem
    {
        struct State
        {
            public int Length;
            public ComponentDataArray<SpawnerCooldown> Cooldown;
            public ComponentDataArray<SpawnerData> D;
            public ComponentDataArray<SpawnerState> S;
        }

        [Inject] State m_State;

        protected override void OnUpdate()
        {
           if ( ProcessCooldown() )
            {
                var state = m_State.S[0];
                var spawnAmount = m_State.D[0].SpawnAmount;
                var spawnArea = m_State.D[0].SpawnArea;
                    
                var oldState = Random.state;
                Random.state = state.RandomState;

                var spawnsLeft = spawnAmount.MaxCount - state.CurrentCount;
                bool spawnsRemaining = spawnsLeft > 0;

                if (spawnsRemaining)
                {
                    var spawnMats = (Resources.Load("ColorLerp") as DataObjects.ColorLerp).Materials;

                    for (var i = 0; i < math.min(spawnAmount.BatchSize, spawnsLeft); ++i)
                    {
                        state.CurrentCount++;
                        SpawnEntity(spawnArea.Origin, spawnArea.Radius, spawnMats);
                    }
                }
                else
                    this.Enabled = false;

                state.RandomState = Random.state;
                m_State.S[0] = state;
                Random.state = oldState;
            }
        }

        bool ProcessCooldown()
        {
            float cooldown = math.max(0.0f, m_State.Cooldown[0].Value - Time.deltaTime);
            bool spawn = cooldown <= 0.0f;

            if (spawn)
            {
                cooldown = m_State.D[0].CooldownInterval;
            }

            m_State.Cooldown[0] = new SpawnerCooldown
            {
                Value = cooldown
            };

            return spawn;
        }

        
        
        /// <summary>
        /// A random radian value from -2 * <see cref="Mathf.PI"/> to 2 * <see cref="Mathf.PI"/> 
        /// </summary>
        static float randomRadian => Random.Range(-2.0f * Mathf.PI, 2.0f * Mathf.PI);

        ///<summary>
        /// A shortened version of the Cross product equation
        /// using a single point, and an extrapolated second point. 
        /// Returns a normalized unit vector perpendicular to 
        /// the point and the origin.
        /// </summary>
        /// <param name="point">A normalized point.</param>
        /// 
        /// Where b.y = 0;
        /// c.x = a.y * b.z − a.z * b.y
        /// c.y = a.z * b.x − a.x * b.z
        /// c.z = a.x * b.y − a.y * b.x
        /// 
        static float3 perpDirection (float3 point) =>  new float3
        {
            x = point.y * point.z - 0,
            y = point.z * point.x - point.x * point.z,
            z = 0 - point.y * point.x
        };

        /// <summary>
        /// Spawns an entitiy of of the GravitySphere archetype in a random spherical position.
        /// </summary>
        /// <param name="origin">The origin of the spawn sphere.</param>
        /// <param name="range">The minimun and maximum range from the origin.</param>
        void SpawnEntity(float3 origin, DataObjects.MinMax range, Material[] materials)
        {
            var pos = new float3
            {
                x = math.cos(randomRadian),
                y = math.sin(randomRadian),
                z = math.cos(randomRadian)
            };

            PostUpdateCommands.CreateEntity(Archtype.GravitySphere);

            PostUpdateCommands.SetComponent(new Pos
            {
                Value = (pos * Random.Range(range.Min, range.Max)) + origin
            });

            PostUpdateCommands.SetComponent(new Rot
            {
                Value = Random.rotationUniform
            });

            var massScale = Random.value + 0.05f;

            PostUpdateCommands.SetComponent(new Scl
            {
                Value = new float3 { x = 1, y = 1, z = 1 } * math.round(massScale * 3 + 1)
            });

            PostUpdateCommands.SetComponent(new Mass
            {
                Value = (massScale * 3 + 1) * 3f
            });

            PostUpdateCommands.SetComponent(new Velocity
            {
                Value = perpDirection(pos) * range.Min
            });

            PostUpdateCommands.SetComponent(new ModelMatrix
            {
                Value = float4x4.identity
            });

            var index = (int) math.clamp(math.round(materials.Length * massScale), 0, materials.Length - 1);

            PostUpdateCommands.SetSharedComponent(new MeshInstanceRenderer
            {
                material = materials[index],
                mesh = (Resources.Load("CubeData") as DataObjects.MeshData).Value,

                receiveShadows = true,
                castShadows = UnityEngine.Rendering.ShadowCastingMode.On
            });
        }
    }
}
