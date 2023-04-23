using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD0_W : MolaMonoBehaviour
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

        // 01 get mola meshes from LOD1
        molaMeshes = GetMeshFromLOD();
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        if (molaMeshes.Count != 0)
        {
            wall = molaMeshes[0];
            roof = molaMeshes[1];
        }

        // 02 operate in the current level 
        wall = MeshSubdivision.SubdivideMeshSplitGridAbs(wall, 3, floorHeight);

        bool[] randomMask = new bool[wall.FacesCount()];
        randomMask = randomMask.Select(a => Random.value < windowRatio).ToArray();
        MolaMesh wallForWindow = new MolaMesh();
        wallForWindow = wall.CopySubMesh(randomMask);

        randomMask = randomMask.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(randomMask);

        wallForWindow = MeshSubdivision.SubdivideMeshExtrudeTapered(wallForWindow, 0, 0.2f);

        MolaMesh window = new MolaMesh();

        // seperate mesh into wall and window by index. every 5th face is window
        bool[] indexMusk = new bool[wallForWindow.FacesCount()];
        for (int i = 0; i < wallForWindow.FacesCount(); i++)
        {
            indexMusk[i] = (i + 1) % 5 == 0; // get every 5th item
        }
        window = wallForWindow.CopySubMesh(indexMusk);

        indexMusk = indexMusk.Select(a => !a).ToArray();
        wallForWindow = wallForWindow.CopySubMesh(indexMusk);

        wall.AddMesh(wallForWindow);

        molaMeshes = new List<MolaMesh>() { wall, roof, window };

        // visualize current 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();
    }
}
