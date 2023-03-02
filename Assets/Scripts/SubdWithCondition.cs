using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class SubdWithCondition : MonoBehaviour
{
    private Mesh mesh;
    private void OnValidate()
    {
        InitMesh();

        MolaMesh molaMesh = new MolaMesh(); 
        // create egg 
        molaMesh = MeshFactory.CreateSphere(5, 0, 0, 0, 16, 16);
        for (int i = 0; i < molaMesh.VertexCount(); i++)
        {
            if(molaMesh.Vertices[i].y > 0)
            {
                molaMesh.Vertices[i] += new Vector3(0, molaMesh.Vertices[i].y * 0.8f, 0);
            }
        }

        // calculate attribute 1 for
        List<float> attribute1 = molaMesh.FaceProperties(UtilsFace.FaceAngleVertical);
        for (int i = 0; i < attribute1.Count; i++)
        {
            attribute1[i] = Mathf.Abs(Mathf.PI - Mathf.Abs(attribute1[i]));
        }
        attribute1 = UtilsMath.MapList(attribute1, 0.3f, 1);

        // calculate attribute 2 for
        List<float> attribute2 = molaMesh.FaceProperties(UtilsFace.FaceCenterY);
        attribute2 = UtilsMath.MapList(attribute2, 0.9f, 0.1f);

        List<bool> attribute3 = new List<bool>(new bool[molaMesh.FacesCount()]);
        for (int i = 0; i < attribute3.Count; i++)
        {
            if (attribute2[i] > 0.2) attribute3[i] = false;
            else attribute3[i] = true;
        }

        molaMesh = MeshSubdivision.SubdivideMeshExtrudeTapered(molaMesh, attribute1, attribute2, attribute3);
        molaMesh = UtilsMesh.MeshOffset(molaMesh, 0.1f, true);

        List<float> attribute4 = molaMesh.FaceProperties(UtilsFace.FaceCenterY);

        UtilsFace.ColorFaceByValue(molaMesh, attribute4);

        molaMesh.FillUnityMesh(mesh);

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
