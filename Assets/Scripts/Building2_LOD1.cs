using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class Building2_LOD1 : MolaMonoBehaviour
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
        // get mola meshes from previous lever: LOD2
        molaMeshes = GetMeshFromLOD();
        MolaMesh volume = new MolaMesh();
        if (molaMeshes.Any())
        {
            volume = molaMeshes[0];
        }

        MolaMesh wall = new MolaMesh();
        MolaMesh floor = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        // seperate volume to wall, floor and roof
        bool[] orientationMask = new bool[volume.FacesCount()];
        for (int i = 0; i < orientationMask.Length; i++)
        {
            if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(volume.FaceVertices(i))) < 1)
            {
                orientationMask[i] = true;
            }
        }
        wall = volume.CopySubMesh(orientationMask);
        orientationMask = orientationMask.Select(a => !a).ToArray();
        floor = volume.CopySubMesh(orientationMask);

        orientationMask = new bool[floor.FacesCount()];
        for (int i = 0; i < orientationMask.Length; i++)
        {
            if (UtilsFace.FaceAngleVertical(floor.FaceVertices(i)) > 0)
            {
                orientationMask[i] = true;
            }
        }
        roof = floor.CopySubMesh(orientationMask);
        orientationMask = orientationMask.Select(a => !a).ToArray();
        floor = floor.CopySubMesh(orientationMask);

        // subdivide walls into 2 types
        wall = MeshSubdivision.SubdivideMeshSplitRelative(wall, 1, 0.1f, 0.1f, 0.1f, 0.9f);
        MolaMesh newWall = new MolaMesh();
        bool[] indexMask = new bool[wall.FacesCount()];
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            indexMask[i] = i % 2 == 0; // get every second item
        }

        newWall = wall.CopySubMesh(indexMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(indexMask);

        molaMeshes = new List<MolaMesh>() {floor, wall, newWall, roof};
        FillUnitySubMesh(molaMeshes);

        // create color 
        UnityEngine.Color[] colors = new UnityEngine.Color[molaMeshes.Count];
        colors[0] = new UnityEngine.Color(0.1f, 0.1f, 0.1f);
        colors[1] = new UnityEngine.Color((float)106 / 255, (float)164 / 255, (float)222 / 255);
        colors[2] = new UnityEngine.Color((float)100 / 255, (float)100 / 255, (float)100 / 255);
        colors[3] = new UnityEngine.Color((float)124 / 255, (float)222 / 255, (float)106 / 255);

        // color sub meshes following previous LOD level
        ColorSubMesh(colors);

        UpdateLOD();
    }

}
