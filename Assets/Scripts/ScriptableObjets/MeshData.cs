using UnityEngine;

namespace SOExample.DataObjects
{
    [CreateAssetMenu(fileName = "MeshData", menuName = "MeshData Object")]
    public class MeshData : ScriptableObject
    {
        public Mesh Value;
    }
}
