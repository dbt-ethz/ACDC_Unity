using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class MeshPrimitiveExample : MonoBehaviour
{
    private Mesh mesh;
    
    void Start()
    {
        // 01 create unity object
        GameObject myMesh = GameObject.CreatePrimitive(PrimitiveType.Cube);

        // 02 create mola object
        MolaMesh molaMesh = new MolaMesh();
        molaMesh = MeshFactory.createBox(0, 0, 0, 1, 1, 1);

        Debug.Log(molaMesh);

    }

    private void InitMesh()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //meshRenderer.material
    }
}
