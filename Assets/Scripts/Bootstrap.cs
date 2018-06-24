using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Bootstrap {

    public static EntityManager entityManager;

    #region Archetype Def
    
    public static EntityArchetype RenderableArchetype;
    public static EntityArchetype MovableArchetype;


    public static void DefineArchetypes()
    {
        RenderableArchetype = entityManager.CreateArchetype(
            typeof(Position), 
            typeof(TransformMatrix), 
            typeof(MeshInstanceRenderer));

        MovableArchetype = entityManager.CreateArchetype(
            typeof(Position),
            typeof(TransformMatrix),
            typeof(MeshInstanceRenderer),
            typeof(Mass),
            typeof(Velocity));

    }

    #endregion

    #region Load External Data
    
    public static SpawnerDataObject BigBangData;
    public static RenderDataObject cubeRenderData;

    private static void LoadExternalDataAssets()
    {
        BigBangData = Resources.Load("Data/SpawnerData") as SpawnerDataObject;
        cubeRenderData = Resources.Load("Data/RenderData/SphereRenderData") as RenderDataObject;
    }

    #endregion

    #region Initialization 

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        entityManager = World.Active.GetOrCreateManager<EntityManager>();

        DefineArchetypes();
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    public static void InitializeWithScene()
    {
        LoadExternalDataAssets();

        InitialEntitySpawnerSystem.SetupComponentData(entityManager, BigBangData);

        NewGame();
    }

    #endregion
    
    public static void NewGame()
    {
        //CreateMovableEntities();
    }

    #region Create Entities
    /*
    private static void CreateMovableEntity ()
    {
        var movingObject = entityManager.CreateEntity(MovableArchetype);
        
        entityManager.SetComponentData(movingObject, new Position {
            Value = new float3(0, 0, 0)
        });
        
        SetSharedMeshInstaceData(movingObject, CubeRenderData.Mesh, CubeRenderData.Material);

        entityManager.SetComponentData(movingObject, new Heading
        {
            Value = new float3(Random.Range(-2.0f * Mathf.PI, 2.0f * Mathf.PI), Random.Range(-2.0f * Mathf.PI, 2.0f * Mathf.PI), Random.Range(-2.0f * Mathf.PI, 2.0f * Mathf.PI))
        });

        entityManager.SetComponentData(movingObject, new MoveSpeed
        {
            speed = Random.Range(10.0f, 100.0f)
        });
    }

    private static void CreateMovableEntities ()
    {
        for (var i = 0; i < 50000; i++)
        {
            CreateMovableEntity();
        }
    }
    */
    #endregion

    #region Helpers/Utilities
    
    ///  Wrapper function to simplify the setting of Mesh and
    ///  Material data to the MeshInstanceRenderer component. 
    private static void SetSharedMeshInstaceData (Entity entity, MeshDataObject meshData, MaterialDataObject materialData)
    {
        entityManager.SetSharedComponentData(entity, new MeshInstanceRenderer
        {
            mesh = meshData.Value,
            material = materialData.Value
        });
    }

    #endregion
}



