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
    

    public List<MolaMesh> molaMeshes;
    public BuildingLOD2 LOD2;
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
                Debug.Log("inherit roof: " + roof.FacesCount());
            } 
        }

        // operation in current level
        wall = MeshSubdivision.SubdivideMeshGrid(wall, 3, 3);
 
        MolaMesh newWall = new MolaMesh();
        bool[] randomMask = new bool[wall.FacesCount()];
        for (int i = 0; i < randomMask.Length; i++)
        {
            if (UnityEngine.Random.value > 0.5) randomMask[i] = true;
        }
        newWall = wall.CopySubMesh(randomMask);
        newWall = MeshSubdivision.SubdivideMeshExtrude(newWall, extrudeLength);

        MolaMesh floor = new MolaMesh();
        floor = newWall.CopySubMesh(face => Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(UtilsVertex.face_vertices(newWall, face))) >= 1);
        newWall = newWall.CopySubMesh(face => Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(UtilsVertex.face_vertices(newWall, face))) <1);

        randomMask = randomMask.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(randomMask);
        wall.AddMesh(newWall);
        Debug.Log("roof " + roof.FacesCount());
        Debug.Log("floor " + floor.FacesCount());
        

        roof.AddMesh(floor);
        Debug.Log("roof after" + roof.FacesCount());

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
