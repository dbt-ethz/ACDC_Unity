using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD1_W : MolaMonoBehaviour
{
    [Range(0, 10)]
    public float extrudeHeight = 4;

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
        // 01 get mola meshes from LOD2
        molaMeshes = GetMeshFromLOD();
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();
        if (molaMeshes.Count != 0)
        {
            wall = molaMeshes[0];
            roof = molaMeshes[1];
        }

        // 02 operate in the current level  
        wall = MeshSubdivision.SubdivideMeshGrid(wall, 3, 3);

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
        newWall = MeshSubdivision.SubdivideMeshExtrude(newWall, extrudeHeight);

        // seperate result mesh into floor and wall by orientation
        MolaMesh newRoof = new MolaMesh();
        bool[] orientationMask = new bool[newWall.FacesCount()];
        for (int i = 0; i < orientationMask.Length; i++)
        {
            if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(newWall.FaceVertices(i))) > 1)
            {
                orientationMask[i] = true;
            }
        }
        newRoof = newWall.CopySubMesh(orientationMask);
        orientationMask = orientationMask.Select(a => !a).ToArray();
        newWall = newWall.CopySubMesh(orientationMask);

        wall.AddMesh(newWall); // put wall together
        roof.AddMesh(newRoof); // put roof together

        molaMeshes = new List<MolaMesh>() { wall, roof };

        // visualize current 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // 03 update LOD2
        UpdateLOD();
    }
}
