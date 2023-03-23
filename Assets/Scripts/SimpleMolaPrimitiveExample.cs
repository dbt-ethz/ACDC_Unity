using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

[ExecuteInEditMode]
public class SimpleMolaPrimitiveExample : MonoBehaviour
{
    [Range(0, 10)]
    public float length;
    [Range(0, 10)]
    public float height;
    [Range(0, 5)]
    public int iteration;
    [Range(0, 100)]
    public int gridNum;
    [Range(3, 100)]
    public int segments = 4;
    private Mesh unityMesh;
    private MolaMesh molaMesh;

    private void Start()
    {
        // create unity mesh, add mesh filter, mesh renderer
        unityMesh = InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    private void UpdateGeometry()
    {
        // create mola mesh
        //molaMesh = MeshFactory.CreateBox(0, 0, 0, length, length, length);
        molaMesh = MeshFactory.CreateCone(0, 3, 2, 1, segments, true, false);
        //molaMesh = UtilsMesh.MeshOffset(molaMesh, 0.5f);
        //molaMesh.SeparateVertices();
        //molaMesh.WeldVertices();
        //MolaMesh molaMesh2 = MeshFactory.CreateBox(3, 3, 3, length * 0.8f, length * 0.8f, length * 0.8f);
        //molaMesh.AddMesh(molaMesh2);
        //molaMesh = MeshSubdivision.SubdivideMeshSplitRoof(molaMesh, -0.5f);

        molaMesh = MeshSubdivision.SubdivideMeshExtrudeTapered(molaMesh, height, 0.7f, true);
        molaMesh.WeldVertices();
        for (int i = 0; i < 3; i++)
        {
            molaMesh.UpdateTopology();
            molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
        }
        
        //molaMesh = MeshSubdivision.SubdivideMeshGrid(molaMesh, gridNum, gridNum);
        //molaMesh = MeshSubdivision.SubdivideMeshExtrudeTapered(molaMesh, 0, 0.2f, false);
        //molaMesh = UtilsMesh.MeshOffset(molaMesh, 0.1f);
        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);

        //molaMesh = MeshSubdivision.SubdivideMeshGrid(molaMesh, gridNum, gridNum);
        //molaMesh = MeshSubdivision.SubdivideMeshExtrude(molaMesh, height);



        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);

        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
        //molaMesh = MeshSubdivision.SubdivideMeshExtrudeToPointCenter(molaMesh, height);
        //for (int i = 0; i < iteration; i++)
        //{
        //    molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
        //}
        //molaMesh = MeshSubdivision.SubdivideMeshCatmullClark(molaMesh);
        //for (int i = 0; i < iteration; i++)
        //{
        //    //molaMesh = MeshSubdivision.SubdivideMeshExtrudeTapered(molaMesh, 2, 0.5f, true);
        //    //molaMesh = MeshSubdivision.SubdivideMeshExtrude(molaMesh, 3);
        //    //molaMesh = MeshSubdivision.SubdivideMeshSplitRoof(molaMesh, -0.5f);
        //    //molaMesh = MeshSubdivision.SubdivideMeshExtrudeToPointCenter(molaMesh, height);


        //}


        //List<float> attribute = molaMesh.FaceProperties(UtilsFace.FaceCenterY);
        //List<float> attribute2 = molaMesh.FaceProperties(UtilsFace.FaceAngleVertical);

        //molaMesh.SeparateVertices();
        //UtilsFace.ColorFaceByValue(molaMesh, attribute);
        
        // convert mola mesh to unity mesh
        if (unityMesh != null)
        {
            Debug.Log("face county: " + molaMesh.FacesCount());
            HDMeshToUnity.FillUnityMesh(unityMesh, molaMesh);
        }
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
        //meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));
        meshRenderer.material = new Material(Shader.Find("Standard"));
        return mesh;
    }

}
