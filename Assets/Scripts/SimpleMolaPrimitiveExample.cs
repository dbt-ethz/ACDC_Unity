using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class SimpleMolaPrimitiveExample : MonoBehaviour
{
    [Range(0, 10)]
    public float length;
    private Mesh mesh;

    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        // 01 create unity object
        // GameObject myMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);

        MolaMesh molaMesh = new MolaMesh();
        //molaMesh = MeshFactory.createBox(0, 0, 0, 1, 1, 1);

        molaMesh = MeshFactory.CreateBox(0, 0, 0, length, length, length);

        InitMesh();

        molaMesh.FillUnityMesh(mesh);
    }

    private void InitMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        mesh = new Mesh();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material = new Material(Shader.Find("Standard"));
    }
}
