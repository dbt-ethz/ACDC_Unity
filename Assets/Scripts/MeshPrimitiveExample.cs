using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class MeshPrimitiveExample : MonoBehaviour
{
    // 05 public parameter
    [Range(0, 10)]
    public float length;
    private Mesh mesh;
    
    //void Start()
    // 06 on validate
    private void OnValidate()
    {
        // 06 
        Debug.Log("Inspector causes this Update");
        // 01 create unity object
        // GameObject myMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 02 create mola object
        MolaMesh molaMesh = new MolaMesh();
        //molaMesh = MeshFactory.createBox(0, 0, 0, 1, 1, 1);

        // 05 public parameter
        molaMesh = MeshFactory.CreateBox(0, 0, 0, length, length, length);

        // 03 prepare unity mesh and mesh filter
        InitMesh();

        // 04 convert mola mesh into unity mesh
        molaMesh.FillUnityMesh(mesh);
    }

    // 03 prepare unity mesh and mesh filter
    private void InitMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));
    }
}
