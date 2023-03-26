using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingGeneratingII : MolaMonoBehaviour
{
    [Range(1, 10)]
    public int floors = 5;
    [Range(1, 10)]
    public int seed = 5;
    [Range(1, 10)]
    public int extrudeLength = 2;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    private void UpdateGeometry()
    {
        //MolaMesh volume = MeshFactory.CreateCone(0, 5, 4, 3, 6, true, false);
        MolaMesh volume = MeshFactory.CreateBox(0, 0, 0, 30, 0.8f, 20);
        MolaMesh roof = new MolaMesh();
        for (int i = 0; i < volume.FacesCount(); i++)
        {
            Vec3[] face_vertices = volume.FaceVertices(i);
            if(UtilsFace.FaceAngleVertical(face_vertices) > 1)
            {
                roof.AddFace(face_vertices);
            }
        }


        //MolaMesh roof = MeshFactory.CreateSingleQuad(0, 0, 0, 10, 0, 0, 10, 0, 8, 0, 0, 8, true);
        roof = MeshSubdivision.SubdivideMeshGrid(roof, 15, 10);

        List<Vec3[]> result_faces_vertices = new List<Vec3[]>();

        MolaMesh column = new MolaMesh();

        for (int i = 0; i < roof.FacesCount(); i++)
        {
            Vec3[] face_vertices = roof.FaceVertices(i);
            if (Random.value < 0.1)
            {
                result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, 8, false);
                column.AddFaces(result_faces_vertices);
            }
            else
            {
                column.AddFace(face_vertices);
            }
        }
        column.WeldVertices();
        column.UpdateTopology();
        column = MeshSubdivision.SubdivideMeshCatmullClark(column);
        column.UpdateTopology();
        column = MeshSubdivision.SubdivideMeshCatmullClark(column);
        //List<Vec3[]> result_faces_vertices = new List<Vec3[]>();
        //for (int i = 0; i < floors; i++)
        //{
        //    Vec3[] face_vertices = floor.FaceVertices(0);
        //    result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, 3);

        //    for (int j = 0; j < result_faces_vertices.Count - 1; j++)
        //    {
        //        wall.AddFace(result_faces_vertices[j]);
        //    }
        //    floor.Clear();
        //    floor.AddFace(result_faces_vertices[^1]);
        //}

        //MolaMesh subWall = new MolaMesh();
        //for (int i = 0; i < wall.FacesCount(); i++)
        //{
        //    Vec3[] face_vertices = wall.FaceVertices(i);
        //    result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
        //    subWall.AddFaces(result_faces_vertices);
        //}
        //wall = subWall;

        //for (int k = 0; k < 3; k++)
        //{
        //    MolaMesh newWall = new MolaMesh();
        //    for (int i = 0; i < wall.FacesCount(); i++)
        //    {
        //        result_faces_vertices = new List<Vec3[]>();
        //        Vec3[] face_vertices = wall.FaceVertices(i);

        //        if (Random.value < 0.2)
        //        {
        //            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, extrudeLength);
        //            for (int j = 0; j < result_faces_vertices.Count; j++)
        //            {
        //                if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(result_faces_vertices[j])) < 0.1f)
        //                {
        //                    newWall.AddFace(result_faces_vertices[j]);
        //                }
        //                else
        //                {
        //                    floor.AddFace(result_faces_vertices[j]);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            newWall.AddFace(face_vertices);
        //        }

        //    }
        //    wall = newWall;
        //}

        //MolaMesh window = new MolaMesh();
        //MolaMesh newWall2 = new MolaMesh();
        //for (int i = 0; i < wall.FacesCount(); i++)
        //{
        //    Vec3[] face_vertices = wall.FaceVertices(i);
        //    if (Random.value < 0.4)
        //    {
        //        result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
        //        window.AddFaces(result_faces_vertices);
        //    }
        //    else
        //    {
        //        newWall2.AddFace(face_vertices);
        //    }

        //}
        //wall = newWall2;

        //MolaMesh glass = new MolaMesh();
        //MolaMesh frame = new MolaMesh();
        //for (int i = 0; i < window.FacesCount(); i++)
        //{
        //    Vec3[] face_vertices = window.FaceVertices(i);
        //    result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f);
        //    for (int j = 0; j < result_faces_vertices.Count - 1; j++)
        //    {
        //        frame.AddFace(result_faces_vertices[j]);
        //    }
        //    glass.AddFace(result_faces_vertices[^1]);

        //}

        List<MolaMesh> molaMeshes = new List<MolaMesh>() { volume, column };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
