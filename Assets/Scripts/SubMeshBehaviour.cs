using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using Color = Mola.Color;

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

    private Mesh unityMesh;

    private void Start()
    {
        unityMesh = InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    private void UpdateGeometry()
    {
        List<MolaMesh> molaMeshes = ExecuteSubd();
        if (unityMesh != null)
        {
            HDMeshToUnity.FillUnitySubMeshes(unityMesh, molaMeshes);
            Debug.Log($"unity mesh triangle counts: {unityMesh.triangles.Length}");
        }
        
        // assign materials
        Material[] mats = new Material[molaMeshes.Count];
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = new Material(Shader.Find("Standard"));
            mats[i].color = RandomColor();
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

        block = MeshSubdivision.SubdivideMeshGrid(block, nU, nV);

        for (int i = 0; i < block.FacesCount(); i++)
        {
            Vec3[] face_vertices = block.FaceVertices(i);
            List<Vec3[]> new_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(face_vertices, 0, 0.2f, true);
            for (int j = 0; j < new_faces_vertices.Count - 1; j++)
            {
                road.AddFace(new_faces_vertices[j]);
            }
            plot.AddFace(new_faces_vertices[^1]);
        }

        foreach(var face in plot.Faces)
        {
            Vec3[] face_vertices = UtilsVertex.face_vertices(plot, face);
            if (Random.value < 0.3) garden.AddFace(face_vertices);
            else floor.AddFace(face_vertices);
        }
        // select by orientation / normal
        // show weld vertices, updatedtopology, catmullclark.
        // one heigh rise building
        // one house with roof

        for (int i = 0; i < floors; i++)
        {
            MolaMesh newfloor = new MolaMesh();
            foreach (var face in floor.Faces)
            {
                if (Random.value < 0.2) newfloor.AddFace(UtilsVertex.face_vertices(floor, face));
                else
                {
                    List<Vec3[]> new_faces_vertices = MeshSubdivision.SubdivideFaceExtrude(floor, face, 3, true);
                    for (int j = 0; j < new_faces_vertices.Count - 1; j++)
                    {
                        wall.AddFace(new_faces_vertices[j]);
                    }
                    newfloor.AddFace(new_faces_vertices[^1]);
                }
            }
            floor = newfloor;
        }

        wall = MeshSubdivision.SubdivideMeshGrid(wall, 10, 1);

        foreach (var face in wall.Faces)
        {
            Vec3[] face_vertices = UtilsVertex.face_vertices(wall, face);
            if (Random.value > windowRatio) panel.AddFace(face_vertices);
            else window.AddFace(face_vertices);
        }

        foreach (var face in window.Faces)
        {
            List<Vec3[]> new_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(window, face, 0, 0.2f, true);
            for (int j = 0; j < new_faces_vertices.Count - 1; j++)
            {
                frame.AddFace(new_faces_vertices[j]);
            }
            glass.AddFace(new_faces_vertices[^1]);
        }

        List<MolaMesh> molaMeshes = new List<MolaMesh>() {road, garden, floor, panel, glass, frame};
        return molaMeshes;
    }
    public static UnityEngine.Color RandomColor()
    {
        return new UnityEngine.Color(Random.value, Random.value, Random.value);
    }

    private Mesh InitMesh()
    {
        Mesh mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = this.gameObject.AddComponent<MeshFilter>();
        }
        meshFilter.mesh = mesh;

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));

        return mesh;
    }
}
