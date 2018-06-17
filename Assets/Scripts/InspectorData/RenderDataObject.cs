using UnityEngine;

[CreateAssetMenu(fileName = "RenderData", menuName = "RenderDataObject")]
public class RenderDataObject : ScriptableObject
{
    public MeshDataObject Mesh;
    public MaterialDataObject Material;
}

