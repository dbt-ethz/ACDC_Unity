using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubMeshBehaviour : MonoBehaviour
{
    private Mesh mesh;

    private void Start()
    {
        Debug.Log("start is called");
        InitMesh();

        // create multiple HDMesh
        HDMesh hdMesh1 = InitHDMesh();
        HDMesh hdMesh2 = InitHDMesh();
        for (int i = 0; i < hdMesh2.VertexCount(); i++)
        {
            hdMesh2.Vertices[i] += new Vector3(2, 0, 0);
        }
        List<HDMesh> hdMeshes = new List<HDMesh>() { hdMesh1, hdMesh2 };

        // fill list of HDMesh as unity sub mesh
        HDMesh.FillUnitySubMeshes(mesh, hdMeshes);

        // assign materials
        Material[] mats = new Material[2];
        mats[0] = new Material(Shader.Find("Particles/Standard Surface"));
        mats[0].color = Color.red;

        mats[1] = new Material(Shader.Find("Particles/Standard Surface"));
        mats[1].color = Color.blue;

        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.materials = mats;


    }
    private void ExecuteSubd()
    {
        HDMesh block = new HDMesh();
        Color color = Color.white;
        HDMeshFactory.AddQuad(block, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);

        block = HDMeshSubdivision.subdivide_mesh_grid(block, 5, 3);
        HDMeshSubdivision.subdivide_mesh_extrude(block, 0);

    }
    public HDMesh InitHDMesh()
    {
        HDMesh newMesh = new HDMesh();
        Color color = Color.white;
        //HDMeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        HDMeshFactory.AddQuad(newMesh, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);
        //HDMeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);

        return newMesh;
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
    }
}
