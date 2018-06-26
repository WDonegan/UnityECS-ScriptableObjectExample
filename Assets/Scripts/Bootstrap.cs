using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Bootstrap {

    public static EntityManager entityManager;

    #region Archetype Def
    
    public static EntityArchetype MovableArchetype;
    
    public static void DefineArchetypes()
    {
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
        
    }
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