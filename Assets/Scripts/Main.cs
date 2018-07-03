using SOExample.Components.Transform;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
using Unity.Transforms2D;
using UnityEngine;

namespace SOExample {
    public partial class Main {

        /// <summary>
        /// Static reference to the active world's EntityManager instance.
        /// </summary>
        public static EntityManager entityManager;

        /// <summary>
        /// Bootstrap entry point. Initializes entityManager and calls DefineArchetypes().
        /// </summary>
        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Initialize ()
        {
            entityManager = World.Active.GetOrCreateManager<EntityManager> ();
            Managers.Archtype.Initialize (entityManager);
        }

        /// <summary>
        /// Initialization to occur after the scene has loaded. 
        /// TODO: check if this is called only once or everytime
        /// a scene is loaded.
        /// </summary>
        [RuntimeInitializeOnLoadMethod (RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void InitializeWithScene ()
        {
            //SpawnTestEntities(10000, 120, 10, 1, 3);
            Managers.SpawnManager.InitializeSpawnSystem(entityManager);
        }
    }
}