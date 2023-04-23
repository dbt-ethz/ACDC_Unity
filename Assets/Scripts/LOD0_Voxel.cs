using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD0_Voxel : MolaMonoBehaviour
{
    [Range(3, 6)]
    public float floorHeight = 4;
    [Range(0, 1)]
    public float windowRatio = 0.8f;


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
        // get mola meshes from previous lever: LOD1
        molaMeshes = GetMeshFromLOD();
        MolaMesh floor = new MolaMesh();
        //MolaMesh wall = new MolaMesh();
        //MolaMesh newWall = new MolaMesh();
        //MolaMesh roof = new MolaMesh();

        if (molaMeshes.Any())
        {
            floor = molaMeshes[0];
        }

        //// extrude wall with height which is related to the y cooridante of face
        //List<float> paramList = new List<float>();
        //List<bool> boolList = new List<bool>();
        //for (int i = 0; i < wall.FacesCount(); i++)
        //{
        //    paramList.Add(UtilsFace.FaceCenterY(wall.FaceVertices(i)));
        //    boolList.Add(true);
        //}
        //if (paramList.Any()) paramList = Mola.Mathf.MapList(paramList, 0, 2);
        //wall = MeshSubdivision.SubdivideMeshExtrude(wall, paramList, boolList);

        //// split wall faces to smaller panels
        //newWall = MeshSubdivision.SubdivideMeshGrid(newWall, 3, 1);

        //// get window
        //newWall = MeshSubdivision.SubdivideMeshExtrudeTapered(newWall, 0, 0.2f);
        //bool[] indexMask = new bool[newWall.FacesCount()];
        //for (int i = 0; i < newWall.FacesCount(); i++)
        //{
        //    indexMask[i] = (i + 1) % 5 == 0; // get every 5th item
        //}
        //MolaMesh window = newWall.CopySubMesh(indexMask);
        //indexMask = indexMask.Select(a => !a).ToArray();
        //newWall = newWall.CopySubMesh(indexMask);

        //// extrude random roof to create garden
        //bool[] randomMask = new bool[roof.FacesCount()];
        //for (int i = 0; i < randomMask.Length; i++)
        //{
        //    if (UnityEngine.Random.value < 0.5f) randomMask[i] = true;
        //}
        //MolaMesh newRoof = roof.CopySubMesh(randomMask);
        //randomMask = randomMask.Select(a => !a).ToArray();
        //roof = roof.CopySubMesh(randomMask);

        ////
        //newRoof = MeshSubdivision.SubdivideMeshExtrudeTapered(newRoof, 0, 0.2f);
        //indexMask = new bool[newRoof.FacesCount()];
        //for (int i = 0; i < newRoof.FacesCount(); i++)
        //{
        //    indexMask[i] = (i + 1) % 5 == 0; // get every 5th item
        //}
        //MolaMesh garden = newRoof.CopySubMesh(indexMask);
        //indexMask = indexMask.Select(a => !a).ToArray();
        //newRoof = newRoof.CopySubMesh(indexMask);

        //MolaMesh balustrade = garden.Copy();
        //balustrade = MeshSubdivision.SubdivideMeshExtrude(balustrade, 1.2f, false);

        //roof.AddMesh(newRoof);

        //molaMeshes = new List<MolaMesh>() { floor, wall, newWall, window, roof, garden, balustrade };
        molaMeshes = new List<MolaMesh>() { floor };
        // visualize current 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();
    }
}
