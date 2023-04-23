using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class BlockSubdivideF : MolaMonoBehaviour
{
    [Range(1, 5)]
    public int iteration = 4;
    [Range(2, 50)]
    public int roadWidth = 7;
    [Range(0, 10)]
    public int seed = 0;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        // this is a by pass to allow deleting objects in OnValidate. can be hide in MolaMonobehaviour
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += UpdateGeometry;
#endif 
    }
    public override void UpdateGeometry()
    {
        // create mola mesh from unity mesh
        Mesh refMesh = transform.parent.GetComponent<MeshFilter>().sharedMesh;
        if (refMesh == null) return;

        List<Vec3> vertices = new List<Vec3>();
        foreach (var v in refMesh.vertices)
        {
            vertices.Add(new Vec3(v.x, v.z, 0));
        }

        MolaMesh block = MeshFactory.CreateSingleFace(vertices.ToList());
        
        //// create block example 01
        //for (int i = 0; i < iteration; i++)
        //{
        //    block = MeshSubdivision.SubdivideMeshSplitRelative(block, 0, 0.4f, 0.4f, 0.6f, 0.6f);
        //}

        // create block example 02
        block = MeshSubdivision.SubdivideMeshExtrudeToPointCenter(block, 0);
        block = MeshSubdivision.SubdivideMeshGrid(block, 2, 2);

        bool[] randomMask = new bool[block.FacesCount()];
        randomMask = randomMask.Select(a => Random.value > 0.3).ToArray();

        MolaMesh plots = block.CopySubMesh(randomMask);
        randomMask = randomMask.Select(a => !a).ToArray();
        MolaMesh garden = block.CopySubMesh(randomMask);

        // create road
        plots = MeshSubdivision.SubdivideMeshSplitFrame(plots, roadWidth * 0.5f);
        bool[] normMask = new bool[plots.FacesCount()];
        for (int i = 0; i < plots.FacesCount(); i++)
        {
            if (plots.FaceAngleVertical(i) >= 0) normMask[i] = true;
        }

        MolaMesh sites = plots.CopySubMesh(normMask);
        normMask = normMask.Select(a => !a).ToArray();
        MolaMesh road = plots.CopySubMesh(normMask);
        road.FlipFaces();

        molaMeshes = new List<MolaMesh> { sites, road, garden };
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        InstantiatePrefab(sites);

    }
    private void InstantiatePrefab(MolaMesh mesh)
    {
        // delete previous building prefabs
        ClearChildrenImmediate();

        // instantiate new building prefabs for each plot mesh face
        for (int i = 0; i < mesh.FacesCount(); i++)
        {
            float x = mesh.FaceEdgeLength(i, 0);
            float y = mesh.FaceEdgeLength(i, 1);
            if (x >= 5 && y >= 5) // minimal prefab size
            {
                var prefabLoad = Resources.Load<GameObject>("LOD_W"); // change to your prefab name. it has to be in "Resources" folder.
                GameObject LODPrefab = Instantiate(prefabLoad, transform);

                LODPrefab.GetComponent<MolaLOD>().startMesh = mesh.CopySubMesh(i);
                LODPrefab.GetComponent<MolaLOD>().DimZ = Random.Range(5, 80);
            }
        }
    }
}
