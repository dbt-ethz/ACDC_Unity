using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class BuildingGeneratingFacade : MolaMonoBehaviour
{
    [Range(1, 10)]
    public int floors = 5;
    [Range(1, 10)]
    public int seed = 5;

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
        MolaMesh floor = MeshFactory.CreateSingleQuad(0, 0, 0, 10, 0, 0, 10, 0, 8, 0, 0, 8, true);
        List<Vec3[]> result_faces_vertices = new List<Vec3[]>();

        MolaMesh wall = new MolaMesh();
        for (int i = 0; i < floors; i++)
        {
            MolaMesh roof = new MolaMesh();
            Vec3[] face_vertices = floor.FaceVertices(0);
            float height = Random.Range(1, 5);
            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(face_vertices, height);

            for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            {
                wall.AddFace(result_faces_vertices[j]);
            }
            roof.AddFace(result_faces_vertices[^1]);
            floor = roof;
        }

        MolaMesh oldWall = new MolaMesh();
        
        for (int j = 0; j < 4; j++)
        {
            MolaMesh subWall = new MolaMesh();
            List<float> compactness = wall.FaceProperties(UtilsFace.FaceCompactness);
            compactness = Mola.Mathf.MapList(compactness);
            for (int i = 0; i < wall.FacesCount(); i++)
            {
                Vec3[] face_vertices = wall.FaceVertices(i);
                if (compactness[i] < 0.8)
                {
                    if (j % 2 == 0) result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);
                    else result_faces_vertices = MeshSubdivision.SubdivideFaceGrid(face_vertices, 3, 1);

                    subWall.AddFaces(result_faces_vertices);
                }
                else
                {
                    oldWall.AddFace(face_vertices);
                }
            }
            wall = subWall;
        }
        wall.AddMesh(oldWall);

        MolaMesh window = new MolaMesh();
        MolaMesh newWall = new MolaMesh();
        List<float> extrudingHeights = wall.FaceProperties(UtilsFace.FaceAreaTriOrQuad);
        extrudingHeights = Mola.Mathf.MapList(extrudingHeights, 1, 0.2f);
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            Vec3[] face_vertices = wall.FaceVertices(i);
            result_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, extrudingHeights[i], 0.2f);
            for (int j = 0; j < result_faces_vertices.Count - 1; j++)
            {
                newWall.AddFace(result_faces_vertices[j]);
            }
            window.AddFace(result_faces_vertices[^1]);
        }
        wall = newWall;

        List<MolaMesh> molaMeshes = new List<MolaMesh>() { floor, wall, window};

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
