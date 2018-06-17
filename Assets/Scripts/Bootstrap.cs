using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Bootstrap {

    public static EntityManager entityManager;

    public static EntityArchetype LevelMapArchetype;

    public static RenderDataObject LevelRenderData;

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
        LevelRenderData = Resources.Load("Data/LevelRenderData") as RenderDataObject;

        NewGame();
    }

    public static void DefineArchetypes()
    {
        // THIS WORKS.
        LevelMapArchetype = entityManager.CreateArchetype(
            typeof(Position), typeof(TransformMatrix));

        // DOES NOT WORK THIS WAY!
        //LevelMapArchetype = entityManager.CreateArchetype(
        //  typeof(Position), typeof(TransformMatrix), typeof(MeshInstanceRenderer));
    }

    public static void NewGame()
    {
        var cube = entityManager.CreateEntity(LevelMapArchetype);

        entityManager.SetComponentData(cube, new Position { Value = new float3(0.0f, 0.0f, 0.0f) });

        // The line below adds a second MeshInstanceRenderer and will cause an error
        // if you also include it in the Archetype definition above.
        entityManager.AddSharedComponentData (cube, new MeshInstanceRenderer
        {
            mesh = LevelRenderData.Mesh.Value,
            material = LevelRenderData.Material.Value
        });
        
    }
}

