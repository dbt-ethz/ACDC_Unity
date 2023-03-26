using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using Mathf = Mola.Mathf;

[ExecuteInEditMode]
public class SubdWithConditionII : MonoBehaviour
{
    [Range(1, 10)]
    public float size = 5;
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
        molaMesh = MeshFactory.CreateSphere(size, 0, 0, 0, 16, 16);

        MolaMesh newMesh = new MolaMesh();
        foreach (var face in molaMesh.Faces)
        {
            Vec3[] face_vertices = UtilsVertex.face_vertices(molaMesh, face);

            float extrudingHeight = UtilsFace.FaceAngleVertical(face_vertices);
            extrudingHeight = Mathf.Abs(Mathf.PI - Mathf.Abs(extrudingHeight));
            extrudingHeight = Mathf.Remap(extrudingHeight, -Mathf.PI / 2, Mathf.PI / 2, extrudeHeightMin, extrudeHeightMax);

            float extrudingFraction = UtilsFace.FaceCenterY(face_vertices);
            extrudingFraction = Mathf.Remap(extrudingFraction, -size / 2, size / 2, fractiontMin, fractionMax);

            bool capTop = false;
            if (UtilsFace.FaceCenterY(face_vertices) < 0)
            {
                capTop = true;
            }

            List<Vec3[]> new_faces_vertices = MeshSubdivision.SubdivideFaceExtrudeTapered(
                molaMesh, face, extrudingHeight, extrudingFraction, capTop);

            newMesh.AddFaces(new_faces_vertices);
        }
        molaMesh = newMesh;

        // get attribute 4 for color
        List<float> attribute = molaMesh.FaceProperties(UtilsFace.FaceCenterY);

        UtilsFace.ColorFaceByValue(molaMesh, attribute);

        // convert mola mesh to unity mesh
        if (unityMesh != null)
        {
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
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));

        return mesh;
    }
}
