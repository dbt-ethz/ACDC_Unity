using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

[ExecuteInEditMode]
public class SubdWithCondition : MonoBehaviour
{
    [Range(0, 5)]
    public float extrudeHeightMin = 0;
    [Range(0, 5)]
    public float extrudeHeightMax = 1;
    [Range(0, 2)]
    public float fractiontMin = 0.1f;
    [Range(0, 2)]
    public float fractionMax = 0.9f;
    [Range(0, 1)]
    public float offsetDepth = 0.1f;
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
        MolaMesh molaMesh = new MolaMesh(); 
        // create egg 
        molaMesh = MeshFactory.CreateSphere(5, 0, 0, 0, 16, 16);

        // get attribute 4 for color
        List<float> attribute = molaMesh.FaceProperties(UtilsFace.FaceCenterY);

        UtilsFace.ColorFaceByValue(molaMesh, attribute);

        // convert mola mesh to unity mesh
        if (unityMesh != null)
        {
            HDMeshToUnity.FillUnityMesh(unityMesh, molaMesh);
        }

        //for (int i = 0; i < molaMesh.VertexCount(); i++)
        //{
        //    if(molaMesh.Vertices[i].y > 0)
        //    {
        //        molaMesh.Vertices[i] += new Vector3(0, molaMesh.Vertices[i].y * 0.8f, 0);
        //    }
        //}



        //// get attribute 1 for extruding height
        //List<float> attribute1 = molaMesh.FaceProperties(UtilsFace.FaceAngleVertical);
        //for (int i = 0; i < attribute1.Count; i++)
        //{
        //    attribute1[i] = Mathf.Abs(Mathf.PI - Mathf.Abs(attribute1[i]));
        //}
        //attribute1 = UtilsMath.MapList(attribute1, extrudeHeightMin, extrudeHeightMax);

        //// get attribute 2 for fraction
        //List<float> attribute2 = molaMesh.FaceProperties(UtilsFace.FaceCenterY);
        //attribute2 = UtilsMath.MapList(attribute2, fractionMax, fractiontMin);

        //// get attribute 3 for capTop
        //List<bool> attribute3 = new List<bool>(new bool[molaMesh.FacesCount()]);
        //for (int i = 0; i < attribute3.Count; i++)
        //{
        //    if (attribute2[i] > 0.2) attribute3[i] = false;
        //    else attribute3[i] = true;
        //}

        //molaMesh = MeshSubdivision.SubdivideMeshExtrudeTapered(molaMesh, attribute1, attribute2, attribute3);
        molaMesh = UtilsMesh.MeshOffset(molaMesh, offsetDepth, true);

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
