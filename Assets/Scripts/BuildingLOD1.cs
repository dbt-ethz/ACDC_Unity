using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;
using System;

public class BuildingLOD1 : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 5;
    [Range(0, 8)]
    public float extrudeLength = 2;
    
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
        // operate in current level
        #region operation in current level
        // operation in current level
        wall = MeshSubdivision.SubdivideMeshGrid(wall, 3, 3);
        
        // seperate wall mesh into 2 meshes by random
        MolaMesh newWall = new MolaMesh();
        bool[] randomMask = new bool[wall.FacesCount()];
        for (int i = 0; i < randomMask.Length; i++)
        {
            if (UnityEngine.Random.value > 0.5) randomMask[i] = true;
        }
        newWall = wall.CopySubMesh(randomMask);
        randomMask = randomMask.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(randomMask);

        // operate subdivision on one mesh
        newWall = MeshSubdivision.SubdivideMeshExtrude(newWall, extrudeLength);

        // seperate result mesh into floor and wall by orientation
        MolaMesh floor = new MolaMesh();
        bool[] orientationMask = new bool[newWall.FacesCount()];
        for (int i = 0; i < orientationMask.Length; i++)
        {
            if(Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(newWall.FaceVertices(i))) > 1)
            {
                orientationMask[i] = true;
            }
        }
        floor = newWall.CopySubMesh(orientationMask);
        orientationMask = orientationMask.Select(a => !a).ToArray();
        newWall = newWall.CopySubMesh(orientationMask);

        wall.AddMesh(newWall); // put wall together
        roof.AddMesh(floor); // add floor to previous roof

        molaMeshes = new List<MolaMesh>() { wall, roof};

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
        #endregion

        // update other level
        UpdateLOD();
    }
}
