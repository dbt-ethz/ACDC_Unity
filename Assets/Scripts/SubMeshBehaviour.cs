using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubMeshBehaviour : MonoBehaviour
{
    [Range(0, 10)]
    public int floors = 5;
    [Range(1, 10)]
    public int nU = 5;
    [Range(1, 10)]
    public int nV = 3;
    [Range(0, 1)]
    public float windowRatio = 0.5f;

    private Mesh mesh;

    private void Start()
    {
        Debug.Log("start is called");
        InitMesh();

        //// create multiple HDMesh
        //HDMesh hdMesh1 = InitHDMesh();
        //HDMesh hdMesh2 = InitHDMesh();
        //for (int i = 0; i < hdMesh2.VertexCount(); i++)
        //{
        //    hdMesh2.Vertices[i] += new Vector3(2, 0, 0);
        //}
        //List<HDMesh> hdMeshes = new List<HDMesh>() { hdMesh1, hdMesh2 };

        //// fill list of HDMesh as unity sub mesh
        //HDMesh.FillUnitySubMeshes(mesh, hdMeshes);

        //List<HDMesh> hdMeshes = ExecuteSubd();
        //HDMesh.FillUnitySubMeshes(mesh, hdMeshes);

        //// assign materials
        //Material[] mats = new Material[hdMeshes.Count];
        //for(int i = 0; i < mats.Length; i++)
        //{
        //    mats[i] = new Material(Shader.Find("Particles/Standard Surface"));
        //    mats[i].color = Utils.RandomColor();
        //}
        //MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        //renderer.materials = mats;
    }

    private void OnValidate()
    {
        List<HDMesh> hdMeshes = ExecuteSubd();
        HDMesh.FillUnitySubMeshes(mesh, hdMeshes);

        Debug.Log($"unity mesh triangle counts: {mesh.triangles.Length}");

        // assign materials
        Material[] mats = new Material[hdMeshes.Count];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = new Material(Shader.Find("Particles/Standard Surface"));
            mats[i].color = Utils.RandomColor();
        }
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.materials = mats;
    }

    private List<HDMesh> ExecuteSubd()
    {
        HDMesh block = new HDMesh();
        HDMesh garden = new HDMesh();
        HDMesh floor = new HDMesh();
        HDMesh road = new HDMesh();
        HDMesh plot = new HDMesh();
        HDMesh wall = new HDMesh();
        HDMesh window = new HDMesh();
        HDMesh panel = new HDMesh();
        HDMesh glass = new HDMesh();
        HDMesh frame = new HDMesh();

        Color color = Color.white;
        HDMeshFactory.AddQuad(block, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);

        block = HDMeshSubdivision.subdivide_mesh_grid(block, nU, nV);

        foreach(var face in block.Faces)
        {
            List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude_tapered(block, face, 0, 0.2f, true);
            for (int i = 0; i < new_faces_vertices.Count - 1; i++)
            {
                road.AddFace(new_faces_vertices[i]);
            }
            plot.AddFace(new_faces_vertices[^1]);
        }

        foreach(var face in plot.Faces)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(plot, face);
            if (Random.value < 0.3) garden.AddFace(face_vertices);
            else floor.AddFace(face_vertices);
        }

        for (int i = 0; i < floors; i++)
        {
            HDMesh newfloor = new HDMesh();
            foreach (var face in floor.Faces)
            {
                if (Random.value < 0.2) newfloor.AddFace(HDUtilsVertex.face_vertices(floor, face));
                else
                {
                    List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude(floor, face, 3, true);
                    for (int j = 0; j < new_faces_vertices.Count - 1; j++)
                    {
                        wall.AddFace(new_faces_vertices[j]);
                    }
                    newfloor.AddFace(new_faces_vertices[^1]);
                }
            }
            floor = newfloor;
        }

        wall = HDMeshSubdivision.subdivide_mesh_grid(wall, 10, 1);

        foreach (var face in wall.Faces)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(wall, face);
            if (Random.value > windowRatio) panel.AddFace(face_vertices);
            else window.AddFace(face_vertices);
        }

        foreach (var face in window.Faces)
        {
            List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude_tapered(window, face, 0, 0.2f, true);
            for (int j = 0; j < new_faces_vertices.Count - 1; j++)
            {
                frame.AddFace(new_faces_vertices[j]);
            }
            glass.AddFace(new_faces_vertices[^1]);
        }

        List<HDMesh> hdMeshes = new List<HDMesh>() {road, garden, floor, panel, glass, frame};
        return hdMeshes;
    }
    public HDMesh InitHDMesh()
    {
        HDMesh newMesh = new HDMesh();
        Color color = Color.white;
        //HDMeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        HDMeshFactory.AddQuad(newMesh, 0, 0, 0, 100, 0, 0, 100, 0, 50, 0, 0, 50, color, true);
        //HDMeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);

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
