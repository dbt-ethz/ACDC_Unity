using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;
using System.Reflection;
using System;

[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubdBehaviour : MonoBehaviour
{
    [Range(0.1f,5)]
    public float extrudeHeight = 1f;
    [Range(0,10)]
    public int iteration = 2;

    private Mesh mesh;

    void Start()
    {
        //InitMesh();
    }

    // this will be called everytime something changes in inspector
    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        InitMesh();

        HDMesh hdMesh = InitHDMesh();

        List<string> subdivideMethods = new List<string>() { "BehaviourA", "BehaviourB" };
        for (int i = 0; i < iteration; i++)
        {
            MethodInfo theMethod = GetType().GetMethod(subdivideMethods[i % subdivideMethods.Count]);
            hdMesh = (HDMesh)theMethod.Invoke(this, new object[] { hdMesh });
        }

        Debug.Log($"face count: {hdMesh.FacesCount()}");
        hdMesh.FillUnityMesh(mesh);
    }

    public HDMesh InitHDMesh()
    {
        HDMesh newMesh = new HDMesh();
        Color color = Color.white;
        HDMeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        //HDMeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);
        return newMesh;
    }

    public HDMesh BehaviourA(HDMesh hdMesh)
    {
        return HDMeshSubdivision.subdivide_mesh_extrude(hdMesh, extrudeHeight);
    }
    public HDMesh BehaviourB(HDMesh hdMesh)
    {
        return HDMeshSubdivision.subdivide_mesh_extrude_tapered(hdMesh, extrudeHeight, 0.5f);
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
        renderer.material = new Material(Shader.Find("Particles/Standard Surface"));
    }

}
