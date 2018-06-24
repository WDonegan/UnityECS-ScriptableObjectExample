using UnityEngine;

[CreateAssetMenu(fileName = "RenderData", menuName = "RenderData Object")]
public class RenderDataObject : ScriptableObject
{
    public MaterialDataObject Material;
    public MaterialArrayObject Materials;
    public MeshDataObject Mesh;
}
