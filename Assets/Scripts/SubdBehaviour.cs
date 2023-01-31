using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;
using System.Reflection;
using System;

[ExecuteInEditMode]
public class SubdBehaviour : MonoBehaviour
{
    public Material material;
    [Range(0.1f,5)]
    public float extrudeHeight = 1f;
    [Range(0,50)]
    public int iteration = 2;
    private Mesh mesh;
    private HDMesh hdMesh;

    void Start()
    {
        Debug.Log("start is called");
        InitMesh();
        List<string> functionStrings = new List<string>(){ "CustomizedBehaviour, CustomizedB" };
        //mesh.MarkDynamic();
    }

    // this will be called everytime something changes in inspector
    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        InitHDMesh();
        for (int i = 0; i < iteration; i++)
        {
            MethodInfo theMethod = GetType().GetMethod("BehaviourB");
            theMethod.Invoke(this, null);
            //CustomizedBehaviour();
        }

        Debug.Log(hdMesh.FacesCount());
        hdMesh.FillUnityMesh(mesh);
    }

    public void InitHDMesh()
    {
        HDMesh newMesh = new HDMesh();
        Color color = Color.white;
        HDMeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        //HDMeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);
        hdMesh = newMesh;
        //return hdMesh;
    }

    public void BehaviourA()
    {
        hdMesh = HDMeshSubdivision.subdivide_mesh_extrude(hdMesh, extrudeHeight);
    }
    public void BehaviourB()
    {
        hdMesh = HDMeshSubdivision.subdivide_mesh_grid(hdMesh, 2, 1);
    }

    public void CustomizedBehaviour()
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

        hdMesh = newMesh;
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
