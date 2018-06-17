using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Bootstrap {

    public static EntityManager entityManager;

    public static EntityArchetype BasicArchetype;

    public static ObjectLook cubeLook;

    // Initialize a reference to the world's EntityManager and define Archtypes.
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();

        DefineArchetypes();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        cubeLook = Resources.Load("Data/CubeLook") as ObjectLook;

        NewGame();
    }

    public static void DefineArchetypes()
    {
        // THIS WORKS.
        BasicArchetype = entityManager.CreateArchetype(
            typeof(Position), typeof(TransformMatrix));

        // DOES NOT WORK THIS WAY!
        //BasicArchetype = entityManager.CreateArchetype(
        //  typeof(Position), typeof(TransformMatrix), typeof(MeshInstanceRenderer));
    }

    public static void NewGame()
    {
        var cube = entityManager.CreateEntity(BasicArchetype);

        entityManager.SetComponentData(cube, new Position { Value = new float3(0.0f, 0.0f, 0.0f) });

        // The line below adds a second MeshInstanceRenderer and will cause an error
        // if you also include it in the Archetype definition above.
        entityManager.AddSharedComponentData (cube, new MeshInstanceRenderer
            { mesh = cubeLook.Mesh, material = cubeLook.Material });

        Debug.LogError($"cubeLook is NULL: {cubeLook == null} | Mat: {cubeLook.Material.name} | Mesh: {cubeLook.Mesh.name}");
    }
}

