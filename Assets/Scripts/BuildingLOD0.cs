using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;
using System;

public class BuildingLOD0 : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 5;
    [Range(0, 3)]
    public float extrudingHeight = 0.5f;
    [Range(0, 1)]
    public float fraction = 0.2f;

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
        // inherit from previous level
        molaMeshes = GetMeshFromLOD();
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();
        if (molaMeshes.Any())
        {
            wall = molaMeshes[0];
            roof = molaMeshes[1];
        }

        // operation in current level
        MolaMesh window = new MolaMesh();
        wall = MeshSubdivision.SubdivideMeshExtrudeTapered(wall, extrudingHeight, fraction);

        // seperate mesh into wall and window by index. every 5th face is window
        bool[] indexMusk = new bool[wall.FacesCount()];
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            indexMusk[i] = (i+1) % 5 == 0; // get every 5th item
        }
        window = wall.CopySubMesh(indexMusk);

        indexMusk = indexMusk.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(indexMusk);

        molaMeshes = new List<MolaMesh>() { wall, roof, window };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
