using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using SOExample.Components.Transform;

namespace SOExample.Systems
{
    [UpdateBefore(typeof(UpdateGroups.RenderingGroup))]
    public class GravitySystem : JobComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentDataArray<Mass> Mass;
            public ComponentDataArray<Pos> Pos;
            public ComponentDataArray<Velocity> Velocity;
        }

        [Inject] Data m_Data;

        private const float G = 6.67f;
        private const float D = 0.999987f;

        [BurstCompile]
        struct CalculateAttractions : IJobParallelFor
        {
            [ReadOnly] public float DeltaTime;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Mass> Masses;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Pos> Positions;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Velocity> Velocities;

            public void Execute(int index)
            {
                var massSelf = Masses[index];
                var positionSelf = Positions[index];
                var velocitySelf = Velocities[index];

                float3 attractionSelf = new float3();

                float mass;
                float3 position;

                float3 deltaPos;
                float3 direction;
                float attraction;


                for (int j = 0; j < Positions.Length; ++j)
                {
                    if (j == index)
                        continue;

                    mass = Masses[j].Value;
                    position = Positions[j].Value;

                    deltaPos = position - positionSelf.Value;

                    direction = math.normalize(position - positionSelf.Value);
                    attraction = G * (massSelf.Value * mass) / math.dot(deltaPos, deltaPos);
                    attractionSelf += direction * attraction;
                }

                //Masses[index] = massSelf;

                velocitySelf.Value += (attractionSelf / massSelf.Value *2f);
                velocitySelf.Value *= D;
                Velocities[index] = velocitySelf;

                positionSelf.Value += velocitySelf.Value * DeltaTime;
                Positions[index] = positionSelf;
            }
        }

        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            if (m_Data.Length == 0)
                return inputDeps;

            var jobHandle = new CalculateAttractions
            {
                DeltaTime = Time.deltaTime,
                Masses = m_Data.Mass,
                Positions = m_Data.Pos,
                Velocities = m_Data.Velocity,
            }.Schedule(m_Data.Length, 64, inputDeps);

            return jobHandle;
        }
    }
}

