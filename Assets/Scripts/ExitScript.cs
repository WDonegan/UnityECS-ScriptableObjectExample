using UnityEngine;
using UnityEngine.UI;
using Unity.Entities;

public class ExitScript : MonoBehaviour {

    public void OnDisable() 
    {
        //Bootstrap.entityManager.CompleteAllJobs();
        //Bootstrap.entityManager.DestroyEntity(Bootstrap.entityManager.GetAllEntities());
        //World.Active.DestroyManager(Bootstrap.entityManager);
        //World.Active.Dispose();
    }
}
