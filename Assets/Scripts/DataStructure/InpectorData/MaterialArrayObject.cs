using UnityEngine;

[CreateAssetMenu(fileName = "MaterialArrayData", menuName = "MaterialArray Object")]
public class MaterialArrayObject : ScriptableObject
{
    public Material[] Value;
}