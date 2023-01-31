using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

[ExecuteInEditMode]
public class SubdBehaviourOld : MonoBehaviour
{
    public Material material;
    public float extrudeHeight = 1f;
    [Range(0,10)]
    public int iteration = 2;
    private Mesh mesh;
    void Start()
    {
        InitMesh();

        //HDMesh hdMesh = InitHDMesh();

        // add customized behaviours
        //hdMesh = CustomizedBehaviour(hdMesh);
        //hdMesh.FillUnityMesh(mesh);

    }
    private void Update()
    {
        Debug.Log("Editor causes this Update");
        HDMesh hdMesh = InitHDMesh();
        for (int i = 0; i < iteration; i++)
        {
            hdMesh = CustomizedBehaviour(hdMesh);
        }
        Debug.Log(hdMesh.FacesCount());
        //mesh.MarkDynamic();
        hdMesh.FillUnityMesh(mesh);
    }

    public HDMesh InitHDMesh()
    {
        HDMesh hdMesh = new HDMesh();
        Color color = Color.white;
        HDMeshFactory.AddBox(hdMesh, 0, 0, 0, 1, 1, 1, color);

        return hdMesh;
    }

    public HDMesh CustomizedBehaviour(HDMesh hdMesh)
    {
        HDMesh newMesh = new HDMesh();
        foreach(var face in hdMesh.Faces) //list of index
        {
            List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude(hdMesh, face, extrudeHeight);
            foreach(var face_vertices in new_faces_vertices)
            {
                newMesh.AddFace(face_vertices);
            }
        }

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

        // init mesh renderer
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        if (renderer == null)
        {
            renderer = this.gameObject.AddComponent<MeshRenderer>();
        }
        renderer.material = material;
    }

}
