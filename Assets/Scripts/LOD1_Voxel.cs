using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD1_Voxel : MolaMonoBehaviour
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
        MolaMesh volume = new MolaMesh();
        if (molaMeshes.Count != 0)
        {
            volume = molaMeshes[0];
        }
        //MolaMesh wall = new MolaMesh();
        //MolaMesh floor = new MolaMesh();
        //MolaMesh roof = new MolaMesh();

        //// seperate volume to wall, floor and roof
        //bool[] orientationMask = new bool[volume.FacesCount()];
        //for (int i = 0; i < orientationMask.Length; i++)
        //{
        //    if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(volume.FaceVertices(i))) < 1)
        //    {
        //        orientationMask[i] = true;
        //    }
        //}
        //wall = volume.CopySubMesh(orientationMask);
        //orientationMask = orientationMask.Select(a => !a).ToArray();
        //floor = volume.CopySubMesh(orientationMask);

        //orientationMask = new bool[floor.FacesCount()];
        //for (int i = 0; i < orientationMask.Length; i++)
        //{
        //    if (UtilsFace.FaceAngleVertical(floor.FaceVertices(i)) > 0)
        //    {
        //        orientationMask[i] = true;
        //    }
        //}
        //roof = floor.CopySubMesh(orientationMask);
        //orientationMask = orientationMask.Select(a => !a).ToArray();
        //floor = floor.CopySubMesh(orientationMask);

        //// subdivide walls into 2 types
        //wall = MeshSubdivision.SubdivideMeshSplitRelative(wall, 1, 0.1f, 0.1f, 0.1f, 0.9f);
        //MolaMesh newWall = new MolaMesh();
        //bool[] indexMask = new bool[wall.FacesCount()];
        //for (int i = 0; i < wall.FacesCount(); i++)
        //{
        //    indexMask[i] = i % 2 == 0; // get every second item
        //}

        //newWall = wall.CopySubMesh(indexMask);
        //indexMask = indexMask.Select(a => !a).ToArray();
        //wall = wall.CopySubMesh(indexMask);

        //molaMeshes = new List<MolaMesh>() { floor, wall, newWall, roof };
        molaMeshes = new List<MolaMesh>() { volume };
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // 03 update LOD2
        UpdateLOD();
    }
}
