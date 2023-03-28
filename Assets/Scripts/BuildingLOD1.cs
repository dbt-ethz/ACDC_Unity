using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingLOD1 : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 5;
    public BuildingLOD2 LOD2;
    public List<MolaMesh> molaMeshes;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    private void UpdateGeometry()
    {
        if (LOD2 != null)
        {
            molaMeshes = LOD2.molaMeshes;
        }

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
