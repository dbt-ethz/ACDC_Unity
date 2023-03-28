using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class SingleHouseGenerating : MolaMonoBehaviour
{
    [Range(3, 20)]
    public float length = 10;
    [Range(3, 20)]
    public float width = 8;
    [Range(3, 6)]
    public float height = 4;
    [Range(0, 5)]
    public float roofHeight = 2;
    [Range(0, 1)]
    public float fraction = 0.3f;
    [Range(1, 10)]
    public int floors = 3;

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
        MolaMesh floor = MeshFactory.CreateSingleQuad(0, 0, 0, length, 0, 0, length, 0, width, 0, 0, width, true);
        List<Vec3[]> result_faces_vertices = new List<Vec3[]>(); // this is used to receive subdivision result

        // create wall and roof
        MolaMesh wall = new MolaMesh();
        MolaMesh roof;
        for (int j = 0; j < floors; j++)
        {
            roof = new MolaMesh(); // prepare for receiving new roof
            Vec3[] floor_face_vertices = floor.FaceVertices(0);
            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(floor_face_vertices, height);
            for (int i = 0; i < result_faces_vertices.Count; i++) // fist 4 items in the list
            {
                if (i == 4)
                {
                    roof.AddFace(result_faces_vertices[i]); // last item in the list
                }
                else
                {
                    wall.AddFace(result_faces_vertices[i]);
                }
            }
            floor = roof; // current roof become next floor
        }
        roof = floor; // final floor become roof

        // subdivide walls into smaller panels
        MolaMesh subWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 3);
            subWall.AddFaces(result_faces_vertices);
        }
        wall = subWall;

        // create windows
        MolaMesh window = new MolaMesh();
        MolaMesh newWall = new MolaMesh();
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            // select a portion of walls to generate windows
            if(Random.value > 0.5)
            {
                result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, fraction);
                for (int j = 0; j < result_faces_vertices.Count - 1; j++)
                {
                    newWall.AddFace(result_faces_vertices[j]);
                }
                window.AddFace(result_faces_vertices[^1]);
            }
            else // the remaining part stay the same
            {
                newWall.AddFace(face_vertices);
            }
        }
        wall = newWall;

        MolaMesh newRoof = new MolaMesh();
        Vec3[] roof_face_vertices = roof.FaceVertices(0);
        result_faces_vertices = MeshSubdivision.SubdivideFaceSplitRoof(roof_face_vertices, roofHeight);
        newRoof.AddFaces(result_faces_vertices);
        roof = newRoof;

        List<MolaMesh> molaMeshes = new List<MolaMesh>() { wall, roof, window };
        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
