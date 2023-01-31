using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;

public class MeshSubdivisionExample : MonoBehaviour
{
    public int iteration;

    private Mesh mesh;
    private HDMesh initHDmesh;
    void Start()
    {
        InitMesh();

        initHDmesh = new HDMesh();
        Color color = Color.white;
        Color colorFacade = Color.red;

        //HDMeshFactory.AddBox(initHDmesh, 0f, 0f, 0f, 3f, 2f, 1f, colorFacade);
        HDMeshFactory.AddQuad(initHDmesh, 0, 0, 0, 3, 0, 0, 3, 2, 0, 0, 2, 0, colorFacade);

    }
    void Update()
    {
        var HDmesh = initHDmesh;
        if (Input.GetKeyDown("f"))
        {
            Debug.Log("start time: " + System.DateTime.Now.ToLongTimeString());
            StartCoroutine(subdivideWaitForSeconds(1f));
            //for (int i = 0; i < iteration; i++)
            //{
            //    HDmesh = subdivide(HDmesh);
            //    //displayHDMesh(HDmesh);
            //    //Debug.Log(string.Format("face count: {0}", HDmesh.FacesCount()));
            //}
            //Debug.Log("end time: " + System.DateTime.Now.ToLongTimeString());
            ////displayHDMesh(HDmesh);
            //Debug.Log(string.Format("face count: {0}", HDmesh.FacesCount()));
            ////displayHDMesh(HDmesh);
        }

    }

    private void InitMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;
    }

    IEnumerator subdivideWaitForSeconds(float sec)
    {
        var HDmesh = this.initHDmesh;
        for (int i = 0; i < iteration; i++)
        {
            HDmesh = subdivide(HDmesh);
            displayHDMesh(HDmesh);
            Debug.Log(string.Format("face count: {0}", HDmesh.FacesCount()));
            yield return new WaitForSeconds(sec);
        }


    }
    private HDMesh subdivide(HDMesh hdmesh)
    {
        HDMesh newMesh = new HDMesh();
        for (int i = 0; i < hdmesh.Faces.Count; i++)
        {
            Vector3[] face_vertices = HDUtilsVertex.face_vertices(hdmesh, hdmesh.Faces[i]);
            List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_extrude(face_vertices, 1f);
            //List<Vector3[]> new_faces_vertices = HDMeshSubdivision.subdivide_face_split_grid(face_vertices, 2, 1);

            foreach (var new_face_vertices in new_faces_vertices)
            {
                if (new_face_vertices.Length == 4)
                {
                    newMesh.AddQuad(new_face_vertices[0], new_face_vertices[1], new_face_vertices[2], new_face_vertices[3], Color.white);
                }
                
            }
        }
        return newMesh;
    }

    private void displayHDMesh(HDMesh hdmesh)
    {
        HDMesh displayMesh = hdmesh.Copy();

        displayMesh.TriangulateQuads();
        displayMesh.SeparateVertices();
        mesh.Clear();
        mesh.vertices = displayMesh.VertexArray();
        mesh.triangles = displayMesh.FlattenedTriangles();
        mesh.SetColors(displayMesh.Colors);
        mesh.RecalculateNormals();
    }
}
