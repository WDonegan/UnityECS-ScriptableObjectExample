﻿using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

namespace SOExample.Systems
{

    class CameraRigSystem : ComponentSystem
    {
        public struct Data
        {
            public int Length;
            public ComponentArray<CameraRigComponent> CameraRig;
            public ComponentArray<Transform> Transform;
        }
        [Inject] Data m_data;
        
        protected override void OnUpdate()
        {
            var rotation = m_data.Transform[0].rotation;
            var camRig = m_data.CameraRig[0];
            
            camRig.CurrentRotation = camRig.CurrentRotation + camRig.RotationAmount * Time.deltaTime;
            rotation = euler(camRig.CurrentRotation);

            m_data.CameraRig[0].CurrentRotation = camRig.CurrentRotation;
            m_data.Transform[0].rotation = rotation;
        }

        public static quaternion euler(float3 xyz)
        {
            // return mul(rotateY(xyz.y), mul(rotateX(xyz.x), rotateZ(xyz.z)));
            float3 s, c;
            math.sincos(0.5f * xyz, out s, out c);
            
            return new quaternion (math.float4(s.xyz, c.x) * c.yxxy * c.zzyz + s.yxxy * s.zzyz * math.float4(c.xyz, s.x) * math.float4(1.0f, -1.0f, -1.0f, 1.0f));
        }
    }
}