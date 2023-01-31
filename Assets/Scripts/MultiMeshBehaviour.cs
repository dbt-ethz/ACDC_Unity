using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class MultiMeshBehaviour : MonoBehaviour
{
    public Material defaultMaterial;

    private Mesh mesh;
    private Mesh plotMesh;
    private Mesh roadMesh;
    private Mesh gardenMesh;
    private Mesh floorMesh;

    private void Start()
    {
        Debug.Log("start is called");
        //InitMesh();
        //Debug.Log(mesh.subMeshCount);
        //mesh.sets
    }

    private void InitMesh()
    {
        // init mesh filter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (null == meshFilter)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;

        // init mesh renderer
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        renderer.material = defaultMaterial;
    }
}
