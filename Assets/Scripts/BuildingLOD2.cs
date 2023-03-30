using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingLOD2 : MolaMonoBehaviour
{
    [Range(3, 20)]
    public float length = 10;
    [Range(3, 20)]
    public float width = 8;
    [Range(3, 5)]
    public float height = 4;

    public List<MolaMesh> molaMeshes;
    public BuildingLOD1 LOD1;
    public BuildingLOD0 LOD0;
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    private void OnValidate()
    {
        UpdateGeometry();
    }
    public void UpdateGeometry()
    {
        MolaMesh floor = MeshFactory.CreateSingleQuad(-length/2, 0, -width/2, length/2, 0, -width / 2, length/2, 0, width/2, -length / 2, 0, width/2, true);

        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        floor = MeshSubdivision.SubdivideMeshExtrude(floor, height);
        roof = floor.CopySubMesh(4, false);
        wall = floor.CopySubMesh(new List<int>() { 0, 1, 2, 3 });


        molaMeshes = new List<MolaMesh>() { wall, roof };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();

        if (LOD1 != null)
        {
            LOD1.UpdateGeometry();
        }
        if (LOD0 != null)
        {
            LOD0.UpdateGeometry();
        }
    }
}
