using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubMeshBehaviour : MonoBehaviour
{
    [Range(0, 50)]
    public int floors = 5;
    [Range(1, 50)]
    public int nU = 5;
    [Range(1, 50)]
    public int nV = 3;
    [Range(0, 1)]
    public float windowRatio = 0.5f;

    private Mesh mesh;

    private void Start()
    {
        Debug.Log("start is called");
        InitMesh();
    }

    private void OnValidate()
    {
        List<MolaMesh> molaMeshes = ExecuteSubd();
        MolaMesh.FillUnitySubMeshes(mesh, molaMeshes);

        Debug.Log($"unity mesh triangle counts: {mesh.triangles.Length}");

        // assign materials
        Material[] mats = new Material[molaMeshes.Count];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = new Material(Shader.Find("Standard"));
            mats[i].color = UtilsMath.RandomColor();
        }
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.materials = mats;
    }

    private List<MolaMesh> ExecuteSubd()
    {
        MolaMesh block = new MolaMesh();
        MolaMesh garden = new MolaMesh();
        MolaMesh floor = new MolaMesh();
        MolaMesh road = new MolaMesh();
        MolaMesh plot = new MolaMesh();
        MolaMesh wall = new MolaMesh();
        MolaMesh window = new MolaMesh();
        MolaMesh panel = new MolaMesh();
        MolaMesh glass = new MolaMesh();
        MolaMesh frame = new MolaMesh();

        Color color = Color.white;
        MeshFactory.AddQuad(block, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);

        block = MeshSubdivision.subdivide_mesh_grid(block, nU, nV);

        foreach(var face in block.Faces)
        {
            List<Vector3[]> new_faces_vertices = MeshSubdivision.subdivide_face_extrude_tapered(block, face, 0, 0.2f, true);
            for (int i = 0; i < new_faces_vertices.Count - 1; i++)
            {
                road.AddFace(new_faces_vertices[i]);
            }
            plot.AddFace(new_faces_vertices[^1]);
        }

        foreach(var face in plot.Faces)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(plot, face);
            if (Random.value < 0.3) garden.AddFace(face_vertices);
            else floor.AddFace(face_vertices);
        }

        for (int i = 0; i < floors; i++)
        {
            MolaMesh newfloor = new MolaMesh();
            foreach (var face in floor.Faces)
            {
                if (Random.value < 0.2) newfloor.AddFace(UtilsVertex.face_vertices(floor, face));
                else
                {
                    List<Vector3[]> new_faces_vertices = MeshSubdivision.subdivide_face_extrude(floor, face, 3, true);
                    for (int j = 0; j < new_faces_vertices.Count - 1; j++)
                    {
                        wall.AddFace(new_faces_vertices[j]);
                    }
                    newfloor.AddFace(new_faces_vertices[^1]);
                }
            }
            floor = newfloor;
        }

        wall = MeshSubdivision.subdivide_mesh_grid(wall, 10, 1);

        foreach (var face in wall.Faces)
        {
            Vector3[] face_vertices = UtilsVertex.face_vertices(wall, face);
            if (Random.value > windowRatio) panel.AddFace(face_vertices);
            else window.AddFace(face_vertices);
        }

        foreach (var face in window.Faces)
        {
            List<Vector3[]> new_faces_vertices = MeshSubdivision.subdivide_face_extrude_tapered(window, face, 0, 0.2f, true);
            for (int j = 0; j < new_faces_vertices.Count - 1; j++)
            {
                frame.AddFace(new_faces_vertices[j]);
            }
            glass.AddFace(new_faces_vertices[^1]);
        }

        List<MolaMesh> molaMeshes = new List<MolaMesh>() {road, garden, floor, panel, glass, frame};
        return molaMeshes;
    }
    public MolaMesh InitMolaMesh()
    {
        MolaMesh newMesh = new MolaMesh();
        Color color = Color.white;
        //MeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        MeshFactory.AddQuad(newMesh, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);
        //MeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);

        return newMesh;
    }

    private void InitMesh()
    {
        // init mesh filter
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (null == meshFilter)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;
    }
}
