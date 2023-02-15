using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Reflection;
using System;

//[ExecuteInEditMode]
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SubdBehaviour : MonoBehaviour
{
    [Range(0.1f,5)]
    public float extrudeHeight = 1f;
    [Range(0,10)]
    public int iteration = 2;
    private Mesh mesh;

    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        InitMesh();

        MolaMesh molaMesh = InitMolaMesh();

        List<string> subdivideMethods = new List<string>() { "BehaviourA", "BehaviourB" };
        for (int i = 0; i < iteration; i++)
        {
            MethodInfo theMethod = GetType().GetMethod(subdivideMethods[i % subdivideMethods.Count]);
            molaMesh = (MolaMesh)theMethod.Invoke(this, new object[] { molaMesh });
        }

        Debug.Log($"face count: {molaMesh.FacesCount()}");
        molaMesh.FillUnityMesh(mesh);
    }
    public MolaMesh InitMolaMesh()
    {
        MolaMesh newMesh = new MolaMesh();
        Color color = Color.white;
        MeshFactory.AddBox(newMesh, 0, 0, 0, 1, 1, 1, color);
        //MeshFactory.AddTriangle(newMesh, 0, 0, 0, 1, 0, 0, 0, 1, 0, color);
        return newMesh;
    }
    public MolaMesh BehaviourA(MolaMesh molaMesh)
    {
        return MeshSubdivision.subdivide_mesh_extrude(molaMesh, extrudeHeight);
    }
    public MolaMesh BehaviourB(MolaMesh molaMesh)
    {
        return MeshSubdivision.subdivide_mesh_extrude_tapered(molaMesh, extrudeHeight, 0.5f);
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
