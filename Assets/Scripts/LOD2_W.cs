using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD2_W : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 0;
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
        // create mola mesh for current LOD level
        // take startMesh as starting point. if there is no startMesh, create default mesh with dimX, Y, Z
        MolaMesh defaultMesh = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);
        MolaMesh floor = startMesh ?? defaultMesh;

        MolaMesh wall; 
        MolaMesh roof;

        floor = MeshSubdivision.SubdivideMeshExtrude(floor, dimZ);

        roof = floor.CopySubMesh(floor.FacesCount()-1, false);
        List<int> myList = new List<int>();
        for (int i = 0; i < floor.FacesCount() - 1; i++)
        {
            myList.Add(i);
        }

        wall = floor.CopySubMesh(myList);

        // store meshes in a list for next LOD level
        molaMeshes = new List<MolaMesh>() { wall, roof };

        // visualize mesh in current LOD level 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // 02 update LOD1 and LOD2
        UpdateLOD();
    }
}
