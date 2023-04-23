using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class LOD2_GroupName : MolaMonoBehaviour
{
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    private void OnValidate()
    {
        UpdateGeometry();
    }

    public override void UpdateGeometry()
    {
        #region REPLACE THIS PART WITH YOUR OWN DESIGN
        // create mola mesh for current LOD level
        MolaMesh floor = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);

        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        floor = MeshSubdivision.SubdivideMeshExtrude(floor, dimZ);

        roof = floor.CopySubMesh(4, false);
        wall = floor.CopySubMesh(new List<int>() { 0, 1, 2, 3 });

        // store meshes in a list for next LOD level
        molaMeshes = new List<MolaMesh>() { wall, roof };
        #endregion

        // flip mesh YZ only for unity visualization

        // visualize mesh in current LOD level 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // 02 update LOD1 and LOD2
        UpdateLOD();
    }
}
