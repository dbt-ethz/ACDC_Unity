using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using Color = UnityEngine.Color;

// enum is a customized data type
public enum myEnum
{
    Box,
    Sphere,
    Cone,
    Torus,
    Tetrahedron,
    Icosahedron,
    Dodecahedron,
    Octahedron,
    RhombicDodecahedron
}
[ExecuteInEditMode]
public class MeshPrimitivesAndSubd : MonoBehaviour
{
    public myEnum myDropDown = new myEnum();

    [Range(0, 10)]
    public float paraA = 1;
    [Range(0, 10)]
    public float paraB = 1;
    [Range(0, 10)]
    public float paraC = 1;
    [Range(0, 1)]
    public float colorValue = 0;
    [Range(0, 5)]
    public int subdInteration = 1;
    [Range(0, 10)]
    public float subdLength = 0;

    private Mesh unityMesh;
    private MolaMesh molaMesh;

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
        // create mola mesh primitive
        molaMesh = createPrimitive();

        // apply subdivision methods
        molaMesh = ApplySubdivision(molaMesh);

        //// update unity mesh color
        //MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        //if(meshRenderer != null)
        //{
        //    meshRenderer.sharedMaterial.color = Color.HSVToRGB(colorValue, 1, 1);
        //}

        // convert mola mesh to unity mesh
        if (unityMesh != null)
        {
            HDMeshToUnity.FillUnityMesh(unityMesh, molaMesh);
        }
    }
    private MolaMesh createPrimitive()
    {
        MolaMesh mesh = new MolaMesh();
        // create mola mesh primitive from the selection of dropdown menu
        switch (myDropDown)
        {
            case myEnum.Box:
                mesh = MeshFactory.CreateBox(0, 0, 0, paraA, paraA, paraA);
                break;
            case myEnum.Cone:
                mesh = MeshFactory.CreateCone(0, paraA, paraB, paraC, 6, true, true);
                break;
            case myEnum.Sphere:
                mesh = MeshFactory.CreateSphere(paraA);
                break;
            case myEnum.Torus:
                mesh = MeshFactory.CreateTorus(paraA, paraB);
                break;
            case myEnum.Tetrahedron:
                mesh = MeshFactory.CreateTetrahedron(paraA, 0, 0, 0);
                break;
            case myEnum.Dodecahedron:
                mesh = MeshFactory.CreateDodecahedron(paraA, 0, 0, 0);
                break;
            case myEnum.Icosahedron:
                mesh = MeshFactory.CreateIcosahedron(paraA, 0, 0, 0);
                break;
            case myEnum.Octahedron:
                mesh = MeshFactory.CreateOctahedron(paraA);
                break;
            case myEnum.RhombicDodecahedron:
                mesh = MeshFactory.CreateRhombicDodecahedron(paraA);
                break; 
        }
        return mesh;
    }
    private MolaMesh ApplySubdivision(MolaMesh molaMesh)
    {
        molaMesh = MeshSubdivision.SubdivideMeshExtrude(molaMesh, subdLength);

        //for (int i = 0; i < subdInteration; i++)
        //{
        //    molaMesh = MeshSubdivision.SubdivideMeshExtrude(molaMesh, subdLength);
        //}
        molaMesh = MeshSubdivision.SubdivideMeshExtrudeToPointCenter(molaMesh, paraC);
        return molaMesh;
    }
    // This part is to prepare unity mesh. Don’t change it
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
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.sharedMaterial.color = Color.HSVToRGB(colorValue, 1, 1);
        return mesh;
    }
}
