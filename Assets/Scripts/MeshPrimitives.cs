using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Reflection;

public enum myEnum
{
    Box,
    Sphere,
    Cone,
    Torus,
    Tetrahedron,
    Icosahedron,
    Dodecahedron
}
public class MeshPrimitives : MonoBehaviour
{
    public myEnum myDropDown = new myEnum();

    [Range(0, 10)]
    public float size = 1;
    [Range(0, 10)]
    public float height;

    private Mesh mesh;

    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        InitMesh();

        //Debug.Log($"vertices: {molaMesh.VertexCount()}, faces: {molaMesh.FacesCount()}");
        //foreach (Vector3 v in molaMesh.Vertices)
        //{
        //    Debug.Log(v.x + ",  " + v.y + ",  " + v.z);
        //}
        //molaMesh = MeshSubdivision.subdivide_mesh_extrude_tapered(molaMesh, 0, 0.5f, false);
        //molaMesh = UtilsMesh.meshOffset(molaMesh, height, true);

        MolaMesh molaMesh = new MolaMesh();

        switch (myDropDown)
        {
            case myEnum.Box:
                molaMesh = MeshFactory.createBox(0, 0, 0, size, size, size);
                break;            
            case myEnum.Cone:
                molaMesh = MeshFactory.createCone(new Vector3(0, 0, 0), new Vector3(0, 0, 1), 6, 0.2f, 1.5f);
                break;
            case myEnum.Sphere:
                molaMesh = MeshFactory.createSphere(size);
                break;            
            case myEnum.Torus:
                molaMesh = MeshFactory.createTorus(size, height);
                break;            
            case myEnum.Tetrahedron:
                molaMesh = MeshFactory.createTetrahedron(size, transform.position.x, transform.position.y, transform.position.z);
                break;
            case myEnum.Dodecahedron:
                molaMesh = MeshFactory.createDodecahedron(size, transform.position.x, transform.position.y, transform.position.z);
                break;
            case myEnum.Icosahedron:
                molaMesh = MeshFactory.createIcosahedron(size, transform.position.x, transform.position.y, transform.position.z);
                break;
        }

        // molaMesh.SeparateVertices();
        molaMesh.FillUnityMesh(mesh);
        //Debug.Log($"unity mesh vertices: {mesh.vertices.Length}, faces: {mesh.triangles.Length}");

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
        //renderer.material = new Material(Shader.Find("Particles/Standard Surface"));
        renderer.material = new Material(Shader.Find("Standard"));
        renderer.sharedMaterial.color = Color.white;
    }
}
