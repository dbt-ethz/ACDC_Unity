using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class CatmullClark : MonoBehaviour
{
    [Range(0, 10)]
    public int paraA;
    private Mesh mesh;
    private void OnValidate()
    {
        Debug.Log("onValidate");

        InitMesh();
        MolaMesh molaMesh = MeshFactory.CreateBox(0, 0, 0, 1, 1, 1);
        //molaMesh.SeparateVertices();
        //molaMesh.UpdateTopology();
        //molaMesh.WeldVertices();
        //molaMesh = UtilsMesh.MeshOffset(molaMesh, 0.5f, true);
        //Debug.Log($"face count: {molaMesh.FacesCount()}");
        //Debug.Log($"vertice count: {molaMesh.VertexCount()}");
        //molaMesh.SeparateVertices();
        //Debug.Log($"face count: {molaMesh.FacesCount()}");
        //Debug.Log($"vertice count: {molaMesh.VertexCount()}");
        //molaMesh.WeldVertices();
        //molaMesh.UpdateTopology();
        //Debug.Log($"face count: {molaMesh.FacesCount()}");
        //Debug.Log($"vertice count: {molaMesh.VertexCount()}");
        //MolaMesh molaMesh = new MolaMesh();
        //MeshFactory.AddQuad(molaMesh, 0, 0, 0, 1, 0, 0, 1, 1, 0, 0, 1, 0, Color.white);
        //molaMesh = MeshFactory.CreateSphere(1);
        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);

        for (int i = 0; i < 2; i++)
        {
            molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
            //molaMesh.WeldVertices();
        }

        HDMeshToUnity.FillUnityMesh(mesh, molaMesh);
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
        renderer.material = new Material(Shader.Find("Standard"));
    }
}
