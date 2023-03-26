using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingGeneratingCircle : MolaMonoBehaviour
{
    [Range(1, 10)]
    public int floors = 5;
    [Range(3, 10)]
    public int nSegments = 5;
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
        MolaMesh floor = MeshFactory.CreateCircle(0, 10, 0, 10, nSegments);

        MolaMesh wall = new MolaMesh();
        
        List<Vec3[]> result_faces_vertices = new List<Vec3[]>();
        for (int j = 0; j < floors; j++)
        {
            MolaMesh roof = new MolaMesh();
            for (int i = 0; i < floor.FacesCount(); i++)
            {
                Vec3[] face_vertices = floor.FaceVertices(i);
                result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, 3, true);

                wall.AddFace(result_faces_vertices[1]);
                roof.AddFace(result_faces_vertices[^1]);
            }
            floor = roof;
        }
        

        MolaMesh subWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
            subWall.AddFaces(result_faces_vertices);
        }
        wall = subWall;

        for (int k = 0; k < 3; k++)
        {
            MolaMesh newWall = new MolaMesh();
            for (int i = 0; i < wall.FacesCount(); i++)
            {
                result_faces_vertices = new List<Vec3[]>();
                Vec3[] face_vertices = wall.FaceVertices(i);

                if (Random.value < 0.2)
                {
                    result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, extrudeLength);
                    for (int j = 0; j < result_faces_vertices.Count; j++)
                    {
                        if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(result_faces_vertices[j])) < 0.1f)
                        {
                            newWall.AddFace(result_faces_vertices[j]);
                        }
                        else
                        {
                            floor.AddFace(result_faces_vertices[j]);
                        }
                    }
                }
                else
                {
                    newWall.AddFace(face_vertices);
                }

            }
            wall = newWall;
        }

        MolaMesh window = new MolaMesh();
        MolaMesh newWall2 = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            if (Random.value < 0.4)
            {
                result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
                window.AddFaces(result_faces_vertices);
            }
            else
            {
                newWall2.AddFace(face_vertices);
            }

        }
        wall = newWall2;

        MolaMesh glass = new MolaMesh();
        MolaMesh frame = new MolaMesh();
        for (int i = 0; i < window.FacesCount(); i++)
        {
            Vec3[] face_vertices = window.FaceVertices(i);
            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f);
            for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            {
                frame.AddFace(result_faces_vertices[j]);
            }
            glass.AddFace(result_faces_vertices[^1]);

        }

        List<MolaMesh> molaMeshes = new List<MolaMesh>() { wall, floor, frame, glass};

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
