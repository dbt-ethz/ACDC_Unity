using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class CitySubdivide : MolaMonoBehaviour
{
    public float dimX = 1000;
    public float dimY = 500;
    public float dimZ = 200;
    [Range(2, 50)]
    public int roadWidth = 20;
    [Range(1, 5)]
    public int iteration = 5;
    [Range(0, 10)]
    public int seed = 0;
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    private void OnValidate()
    {
        // this is a by pass to allow deleting objects in OnValidate. Don't change it
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += UpdateGeometry;
#endif 
    }
    public override void UpdateGeometry()
    {
        MolaMesh city = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);

        for (int i = 0; i < iteration; i++)
        {
            city = MeshSubdivision.SubdivideMeshSplitRelative(city, 0, 0.4f, 0.4f, 0.6f, 0.6f);
        }

        city = MeshSubdivision.SubdivideMeshSplitFrame(city, roadWidth * 0.5f);

        bool[] filterMask = new bool[city.FacesCount()];
        for (int i = 0; i < city.FacesCount(); i++)
        {
            if (city.FaceAngleVertical(i) >= 0) filterMask[i] = true;
        }

        MolaMesh blocks = city.CopySubMesh(filterMask);
        filterMask = filterMask.Select(a => !a).ToArray();
        MolaMesh road = city.CopySubMesh(filterMask);
        road.FlipFaces();
        Debug.Log(blocks.FacesCount());

        molaMeshes = new List<MolaMesh> { road };
        FillUnitySubMesh(molaMeshes, true);
        UnityEngine.Color color = new UnityEngine.Color(0, 0, 0);
        ColorMesh(color);

        InstantiateBlocks(blocks);
    }
    private void InstantiateBlocks(MolaMesh blocks)
    {
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            // destroyimmediate will excute in each iteration, as a result always delete the first child because previous first child is gone
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
        string groupName = "PABCDEFGHIJKLMNO";

        for (int i = 0; i < blocks.FacesCount(); i++)
        {
            GameObject blockGO = new GameObject();
            blockGO.name = "Block" + groupName[i];
            blockGO.transform.parent = transform;
            MolaMesh blockMesh = blocks.CopySubMesh(i);
            InstantiateBlock(blockGO, blockMesh);
        }
    }
    private void InstantiateBlock(GameObject blockGO, MolaMesh molaMesh)
    {
        Mesh blockUnityMesh = new Mesh();
        blockUnityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        blockUnityMesh.MarkDynamic();
        MeshFilter meshFilter = blockGO.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = blockGO.AddComponent<MeshFilter>();
        }
        
        MeshRenderer meshRenderer = blockGO.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = blockGO.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = new Material(Shader.Find("Standard"));
        meshRenderer.sharedMaterial.color = RandomColor();

        meshFilter.mesh = blockUnityMesh;
        HDMeshToUnity.FillUnityMesh(blockUnityMesh, molaMesh, true);
    }
}
