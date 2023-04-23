using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD2_W : MolaMonoBehaviour
{
    [Range(5, 100)]
    public float dimX = 10;
    [Range(5, 100)]
    public float dimY = 10;
    [Range(5, 100)]
    public float dimZ = 10;

    [HideInInspector]
    public MolaMesh startMesh;

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
        MolaMesh molaMesh = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);
        MolaMesh floor = startMesh ?? molaMesh;

        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        floor = MeshSubdivision.SubdivideMeshExtrude(floor, dimZ);

        roof = floor.CopySubMesh(floor.FacesCount()-1, false);
        List<int> myList = Enumerable.Range(0, (floor.FacesCount() - 1)).ToList();
        wall = floor.CopySubMesh(myList);


        // store meshes in a list for next LOD level
        molaMeshes = new List<MolaMesh>() { wall, roof };

        // flip mesh YZ only for unity visualization

        // visualize mesh in current LOD level 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // 02 update LOD1 and LOD2
        UpdateLOD();
    }
}
