using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingLOD0 : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 5;
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
        GameObject parent = transform.parent.gameObject;
        BuildingLOD1 LOD1 = parent.GetComponentInChildren<BuildingLOD1>(true);
        if (LOD1 != null)
        {
            molaMeshes = LOD1.molaMeshes;
            Debug.Log("found");
        }
        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
