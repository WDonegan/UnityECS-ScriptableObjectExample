using UnityEngine;

public class LocationVisual : MonoBehaviour {

    [Range(0.1f, 1f)]
    public float size;
    public Color color;

    public void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, Vector3.one * size);
    }
}
