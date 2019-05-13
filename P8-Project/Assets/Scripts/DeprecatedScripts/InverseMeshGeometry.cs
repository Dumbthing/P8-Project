using UnityEngine;
using System.Linq;

public class InverseMeshGeometry : MonoBehaviour
{
    void Awake()
    {
        Mesh mesh = GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();
    }
}
