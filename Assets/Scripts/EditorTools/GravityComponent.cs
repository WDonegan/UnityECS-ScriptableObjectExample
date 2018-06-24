using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;

[System.Serializable]
[ExecuteInEditMode]
public class GravityComponent : MonoBehaviour {

    [System.Serializable]
    struct Planet
    {
        public float Attraction;
        public float Distance;
        public float3 Direction;
        public float3 Acceleration;
        
    }

    [ReadOnly] static           float               G = 6.6730f;
    
    [SerializeField]            float3              position;
    [SerializeField, ReadOnly]  float               scale;
    [SerializeField, ReadOnly]  float               mass;
    [SerializeField]            float3              velocity;
    [SerializeField, ReadOnly]  GravityComponent    Planet1;
    [SerializeField, ReadOnly]  GravityComponent    Planet2;

    [SerializeField]            Planet              p1, p2;
    [SerializeField]            float3              Heading;

    [SerializeField, ReadOnly]  bool                active = false;
    [SerializeField, ReadOnly]  float3              initialHeading;

    void Start()
    {
        //initialHeading = new float3
        //{
        //    x = Random.value - 0.5f,
        //    y = Random.value - 0.5f,
        //    z = Random.value - 0.5f
        //};
    }

    void Update ()
    {
        transform.position = position;
        transform.localScale = Vector3.one * scale;

        #region Planet 1

        p1.Direction.x = Planet1.transform.position.x - position.x;
        p1.Direction.y = Planet1.transform.position.y - position.y;
        p1.Direction.z = Planet1.transform.position.z - position.z;

        p1.Distance = math.distance(Planet1.transform.position, position);

        p1.Attraction = G * ((mass * Planet1.mass) / math.pow(p1.Distance, 2));

        p1.Direction = math.normalize(p1.Direction);

        p1.Acceleration = p1.Direction * math.clamp(p1.Attraction, 0f, 1f);

        #endregion

        #region Planet 2

        p2.Direction.x = Planet2.transform.position.x - position.x;
        p2.Direction.y = Planet2.transform.position.y - position.y;
        p2.Direction.z = Planet2.transform.position.z - position.z;

        p2.Distance = math.distance(Planet2.transform.position, position);

        p2.Attraction = G * ((mass * Planet2.mass) / math.pow(p2.Distance, 2));

        p2.Direction = math.normalize(p2.Direction);

        p2.Acceleration = p2.Direction * math.clamp(p2.Attraction, 0f, 1f);

        #endregion

        var p1mass = mass > Planet1.mass ? mass : Planet1.mass;
        var p2mass = mass > Planet2.mass ? mass : Planet2.mass;

        Heading = p1.Acceleration;

        Heading = (p1.Distance > p1mass ? p1.Acceleration : new float3 { x = 0, y = 0, z = 0 })
                + (p2.Distance > p2mass ? p2.Acceleration : new float3 { x = 0, y = 0, z = 0 });

        if (active)
        {
            velocity += p1.Acceleration;
            position += velocity;
            transform.position = position;
        }
    }
    
    private void OnDrawGizmos()
    {
        var pos = ToVec3(position);
        var p1_Direction = ToVec3(p1.Direction);
        var p2_Direction = ToVec3(p2.Direction);

        Gizmos.color = Color.magenta;

        Gizmos.DrawRay(pos + (p1_Direction * mass), (p1_Direction * p1.Attraction));
       
        Gizmos.color = Color.cyan;

        Gizmos.DrawRay(pos + (p2_Direction * mass), (p2_Direction * p2.Attraction));

        Gizmos.color = Color.yellow;

        //Gizmos.DrawRay(pos, initialHeading);

        if (active)
        {
            var Direction = ToVec3(velocity);

            Gizmos.color = Color.white;

            Gizmos.DrawRay(pos + (Direction * 0.5f), Direction);
        }
        
    }

    Vector3 ToVec3(float3 f3)
    {
        return new Vector3
        {
            x = f3.x,
            y = f3.y,
            z = f3.z
        };
    }
}
