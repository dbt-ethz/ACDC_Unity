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
        // create floor
        MolaMesh floor = MeshFactory.CreateCircle(0, 10, 0, 10, nSegments);
        // vec3 list to always receive the subdivision result
        List<Vec3[]> result_faces_vertices = new List<Vec3[]>();

        // generating wall and roof for multiple layers
        MolaMesh wall = new MolaMesh();
        for (int j = 0; j < floors; j++)
        {
            MolaMesh roof = new MolaMesh();
            for (int i = 0; i < floor.FacesCount(); i++) // each circle is composed by multiple triangles, as a result we need to extrude each triangle
            {
                Vec3[] face_vertices = floor.FaceVertices(i);
                result_faces_vertices = FaceSubdivision.Extrude(face_vertices, 3, true); // extruing triangle gets 4 new faces

                wall.AddFace(result_faces_vertices[1]); // we know that the 2nd face is the one on the outer edge
                roof.AddFace(result_faces_vertices[^1]); // and the last one is the one on top
            }
            floor = roof; // always replace roof as floor for next iteration
        }

        // subdivide wall into smaller panels
        MolaMesh subWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            result_faces_vertices = FaceSubdivision.Grid(face_vertices, 3, 1);
            subWall.AddFaces(result_faces_vertices);
        }
        wall = subWall;

        // extrude wall panels to create dynamic volumes of the building
        for (int k = 0; k < 3; k++)
        {
            MolaMesh newWall = new MolaMesh();
            for (int i = 0; i < wall.FacesCount(); i++)
            {
                result_faces_vertices = new List<Vec3[]>();
                Vec3[] face_vertices = wall.FaceVertices(i);

                if (Random.value < 0.2) // select 20% of wall panels to extrude
                {
                    result_faces_vertices = FaceSubdivision.Extrude(face_vertices, extrudeLength);
                    for (int j = 0; j < result_faces_vertices.Count; j++)
                    {
                        if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(result_faces_vertices[j])) < 0.1f) // if the face is facing sideways 
                        {
                            newWall.AddFace(result_faces_vertices[j]);
                        }
                        else // if the face is facing up or down
                        {
                            floor.AddFace(result_faces_vertices[j]);
                        }
                    }
                }
                else // the rest 80% of wall panels remain the same
                {
                    newWall.AddFace(face_vertices);
                }
            }
            wall = newWall;
        }

        // generate windows
        MolaMesh window = new MolaMesh();
        MolaMesh newWall2 = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            if (Random.value < 0.4)
            {
                result_faces_vertices = FaceSubdivision.Grid(face_vertices, 3, 1);
                window.AddFaces(result_faces_vertices);
            }
            else
            {
                newWall2.AddFace(face_vertices);
            }

        }
        wall = newWall2;

        // glass and window frames
        MolaMesh glass = new MolaMesh();
        MolaMesh frame = new MolaMesh();
        for (int i = 0; i < window.FacesCount(); i++)
        {
            Vec3[] face_vertices = window.FaceVertices(i);
            result_faces_vertices = FaceSubdivision.ExtrudeTapered(face_vertices, 0, 0.2f);
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
