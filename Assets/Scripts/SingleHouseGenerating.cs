using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using Color = Mola.Color;

public class SingleHouseGenerating : MolaMonoBehaviour
{
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
        // create floor
        MolaMesh floor = MeshFactory.CreateSingleQuad(0, 0, 0, 10, 0, 0, 10, 0, 8, 0, 0, 8, true);
        List<Vec3[]> result_faces_vertices = new List<Vec3[]>();

        // subdivide to get wall and roof
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();
        Vec3[] floor_face_vertices = floor.FaceVertices(0);
        result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(floor_face_vertices, 4);

        for (int i = 0; i < result_faces_vertices.Count -1; i++)
        {
            wall.AddFace(result_faces_vertices[i]);
        }
        roof.AddFace(result_faces_vertices[^1]);
        // here show example of making list and fill unity submesh

        // 01 subwall
        MolaMesh subWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
            subWall.AddFaces(result_faces_vertices);
        }
        wall = subWall;

        // subdivide to get window
        MolaMesh window = new MolaMesh();
        MolaMesh newWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);

            //// 02 only one direction
            //if (UtilsFace.FaceAngleHorizontal(face_vertices) == 0)
            //{
            //    result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f);
            //    for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            //    {
            //        newWall.AddFace(result_faces_vertices[j]);
            //    }
            //    window.AddFace(result_faces_vertices[^1]);
            //}
            //else
            //{
            //    newWall.AddFace(face_vertices);
            //}

            ////03 add random choice
            //if (Random.value > 0.5)
            //{
            //    result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f);
            //    for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            //    {
            //        newWall.AddFace(result_faces_vertices[j]);
            //    }
            //    window.AddFace(result_faces_vertices[^1]);
            //}
            //else
            //{
            //    newWall.AddFace(face_vertices);
            //}
            //00
            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f);
            for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            {
                newWall.AddFace(result_faces_vertices[j]);
            }
            window.AddFace(result_faces_vertices[^1]);
        }
        wall = newWall;

        //00 subdivide to extrude roof
        MolaMesh newRoof = new MolaMesh();
        Vec3[] roof_face_vertices = roof.FaceVertices(0);
        result_faces_vertices = MeshSubdivision.SubdivideFaceSplitRoof(roof_face_vertices, 2);
        newRoof.AddFaces(result_faces_vertices);
        roof = newRoof;

        //// 04 add details to roof
        //MolaMesh newRoof = new MolaMesh();
        //Vec3[] roof_face_vertices = roof.FaceVertices(0);
        //result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(roof_face_vertices, 3, 1);
        //newRoof.AddFaces(result_faces_vertices);
        //roof = newRoof;

        //newRoof = new MolaMesh();
        //for (int i = 0; i < roof.FacesCount(); i++)
        //{
        //    Vec3[] face_vertices = roof.FaceVertices(i);
        //    result_faces_vertices = MeshSubdivision.SubdivideFaceSplitRoof(face_vertices, 2);
        //    newRoof.AddFaces(result_faces_vertices);
        //}
        //roof = newRoof;

        // put all mola mesh into a list and fill unity sub mesh
        List<MolaMesh> molaMeshes = new List<MolaMesh>() {wall, window, roof };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
