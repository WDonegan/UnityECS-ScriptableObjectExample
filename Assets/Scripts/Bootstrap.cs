using UnityEngine;
using Unity.Mathematics;
using Unity.Entities;
using Unity.Rendering;
using Unity.Transforms;

public class Bootstrap {

    public static EntityManager entityManager;

    #region Archetype Def

    public static EntityArchetype RenderableArchetype;

    #endregion

    #region RenderData Def

    public static RenderDataObject LevelRenderData;


    #endregion


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
        RenderableArchetype = entityManager.CreateArchetype(
           typeof(Position), typeof(TransformMatrix), typeof(MeshInstanceRenderer));


    }

    public static void NewGame()
    {
        var level = entityManager.CreateEntity(RenderableArchetype);

        entityManager.SetComponentData(level, new Position { Value = new float3(0.0f, 0.0f, 0.0f) });

        entityManager.SetSharedComponentData (level, new MeshInstanceRenderer
        {
            mesh = LevelRenderData.Mesh.Value,
            material = LevelRenderData.Material.Value
        });
    }
}



