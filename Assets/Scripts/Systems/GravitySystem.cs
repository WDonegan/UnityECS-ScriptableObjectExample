﻿using UnityEngine;
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
            [ReadOnly] public ComponentDataArray<Mass> Mass;
            public ComponentDataArray<Pos> Pos;
            public ComponentDataArray<Velocity> Velocity;
        }

        [Inject] Data m_Data;

        private const float G = 6.67f;
        private const float D = 0.9987f;

        [BurstCompile]
        struct CalculateAttractions : IJobParallelFor
        {
            [ReadOnly] public float DeltaTime;
            [ReadOnly] public ComponentDataArray<Mass> Masses;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Pos> Positions;

            [NativeDisableParallelForRestriction]
            public ComponentDataArray<Velocity> Velocities;

            public void Execute(int index)
            {
                float massSelf = Masses[index].Value;
                var positionSelf = Positions[index];
                var velocitySelf = Velocities[index];

                float3 attractionSelf = new float3();

                float mass;
                float3 position;
                float3 velocity;

                float3 deltaPos;
                float3 direction;
                float attraction;

                for (int j = 0; j < Positions.Length; ++j)
                {
                    if (j == index)
                        continue;

                    mass = Masses[j].Value;
                    position = Positions[j].Value;
                    velocity = Velocities[j].Value;

                    deltaPos = (position + velocity) - positionSelf.Value;
                    direction = math.normalize(position - positionSelf.Value);
                    attraction = G * ((massSelf * mass) / math.dot(deltaPos, deltaPos));

                    attractionSelf += direction * attraction;
                }

                velocitySelf.Value += attractionSelf;
                velocitySelf.Value = velocitySelf.Value * D;
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

