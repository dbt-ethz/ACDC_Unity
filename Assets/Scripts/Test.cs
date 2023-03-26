using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class Test : MolaMonoBehaviour
{
    public void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    public void OnValidate()
    {
        UpdateGeometry();
    }
    public void UpdateGeometry()
    {
        MolaMesh molaMesh = MeshFactory.CreateBox(0, 0, 0, 1, 1, 1);
        molaMesh = MeshSubdivision.SubdivideMeshExtrude(molaMesh, 1);
        FillUnityMesh(molaMesh);

        // or
        List<MolaMesh> molaMeshes = new List<MolaMesh>();
        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
