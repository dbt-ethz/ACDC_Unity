using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;
using System;

public class Building2_LOD0 : MolaMonoBehaviour
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
        // get mola meshes from previous lever: LOD1
        molaMeshes = GetMeshFromLOD();
        MolaMesh floor = new MolaMesh();
        MolaMesh wall = new MolaMesh();
        MolaMesh newWall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        if (molaMeshes.Any())
        {
            floor = molaMeshes[0];
            wall = molaMeshes[1];
            newWall = molaMeshes[2];
            roof = molaMeshes[3];
        }

        // extrude wall with height which is related to the y cooridante of face
        List<float> paramList = new List<float>();
        List<bool> boolList = new List<bool>();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            paramList.Add(UtilsFace.FaceCenterY(wall.FaceVertices(i)));
            boolList.Add(true);
        }
        if(paramList.Any()) paramList = Mola.Mathf.MapList(paramList, 0, 2);
        wall = MeshSubdivision.SubdivideMeshExtrude(wall, paramList, boolList);

        // split wall faces to smaller panels
        newWall = MeshSubdivision.SubdivideMeshGrid(newWall, 3, 1);

        // get window
        newWall = MeshSubdivision.SubdivideMeshExtrudeTapered(newWall, 0, 0.2f);
        bool[] indexMask = new bool[newWall.FacesCount()];
        for (int i = 0; i < newWall.FacesCount(); i++)
        {
            indexMask[i] = (i + 1) % 5 == 0; // get every 5th item
        }
        MolaMesh window = newWall.CopySubMesh(indexMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        newWall = newWall.CopySubMesh(indexMask);

        // extrude random roof to create garden
        bool[] randomMask = new bool[roof.FacesCount()];
        for (int i = 0; i < randomMask.Length; i++)
        {
            if (UnityEngine.Random.value < 0.5f) randomMask[i] = true;
        }
        MolaMesh newRoof = roof.CopySubMesh(randomMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        roof = roof.CopySubMesh(randomMask);

        //
        newRoof = MeshSubdivision.SubdivideMeshExtrudeTapered(newRoof, 0, 0.2f);
        indexMask = new bool[newRoof.FacesCount()];
        for (int i = 0; i < newRoof.FacesCount(); i++)
        {
            indexMask[i] = (i + 1) % 5 == 0; // get every 5th item
        }
        MolaMesh garden = newRoof.CopySubMesh(indexMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        newRoof = newRoof.CopySubMesh(indexMask);

        MolaMesh balustrade = garden.Copy();
        balustrade = MeshSubdivision.SubdivideMeshExtrude(balustrade, 1.2f, false);

        roof.AddMesh(newRoof);

        molaMeshes = new List<MolaMesh>() { floor, wall, newWall, window, roof, garden, balustrade };
        FillUnitySubMesh(molaMeshes);
        // create color 
        UnityEngine.Color[] colors = new UnityEngine.Color[molaMeshes.Count];
        colors[0] = new UnityEngine.Color(0.1f, 0.1f, 0.1f);
        colors[1] = new UnityEngine.Color((float)106 / 255, (float)164 / 255, (float)222 / 255); //blue
        colors[2] = new UnityEngine.Color(0, 0, 0); //white
        colors[3] = new UnityEngine.Color((float)48 / 255, (float)68 / 255, (float)104 / 255); //grey
        colors[4] = new UnityEngine.Color((float)180 / 255, (float)180 / 255, (float)180 / 255); //light grey
        colors[5] = new UnityEngine.Color((float)67 / 255, (float)148 / 255, (float)32 / 255); //green
        colors[6] = new UnityEngine.Color((float)222 / 255, (float)133 / 255, (float)106 / 255); //orange
        ColorSubMesh(colors);
        //ColorSubMeshRandom();
    }
}
