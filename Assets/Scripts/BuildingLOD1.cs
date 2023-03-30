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
    public BuildingLOD2 LOD2;
    public List<MolaMesh> molaMeshes;
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
        // inherit from previous level
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();
        if (LOD2 != null)
        {
            molaMeshes = LOD2.molaMeshes;
            if(molaMeshes != null)
            {
                wall = molaMeshes[0];
                roof = molaMeshes[1];
            } 
        }

        // operation in current level
        wall = MeshSubdivision.SubdivideMeshGrid(wall, 3, 3);
 
        MolaMesh newWall = new MolaMesh();
        bool[] randomMusk = new bool[wall.FacesCount()];
        for (int i = 0; i < randomMusk.Length; i++)
        {
            if (UnityEngine.Random.value > 0.5) randomMusk[i] = true;
            else randomMusk[i] = false;
        }
        newWall = wall.CopySubMesh(randomMusk);
        newWall = MeshSubdivision.SubdivideMeshExtrude(newWall, 3);

        MolaMesh floor = new MolaMesh();
        floor = newWall.CopySubMesh(face => Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(UtilsVertex.face_vertices(newWall, face))) >= 1);
        newWall = newWall.CopySubMesh(face => Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(UtilsVertex.face_vertices(newWall, face))) <1);

        randomMusk = randomMusk.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(randomMusk);
        wall.AddMesh(newWall);

        roof.AddMesh(floor);

        molaMeshes = new List<MolaMesh>() { wall, roof };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();

        // update next level
        if (LOD0 != null)
        {
            LOD0.UpdateGeometry();
        }
    }
}
