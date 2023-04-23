using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class BlockSubdivideII : MolaMonoBehaviour
{
    public float dimX = 274;
    public float dimY = 80;
    public float dimZ = 200;
    [Range(1, 5)]
    public int iteration = 3;
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
        // create plots and roads
        MolaMesh block = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);

        block = MeshSubdivision.SubdivideMeshExtrudeToPointCenter(block, 0);
        block = MeshSubdivision.SubdivideMeshGrid(block, 2, 2);

        bool[] randomMask = new bool[block.FacesCount()];
        randomMask = randomMask.Select(a => Random.value > 0.3).ToArray();

        MolaMesh plots = block.CopySubMesh(randomMask);
        randomMask = randomMask.Select(a => !a).ToArray();
        MolaMesh garden = block.CopySubMesh(randomMask);

        plots = MeshSubdivision.SubdivideMeshSplitFrame(plots, roadWidth * 0.5f);

        bool[] indexMask = new bool[plots.FacesCount()];
        for (int i = 0; i < plots.FacesCount(); i++)
        {
            if (plots.FaceAngleVertical(i)>=0) indexMask[i] = true; 
        }

        MolaMesh buildingBase = plots.CopySubMesh(indexMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        MolaMesh road = plots.CopySubMesh(indexMask);
        road.FlipFaces();

        molaMeshes = new List<MolaMesh> { buildingBase, garden, road };
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        InstantiatePrefabs(buildingBase);

    }
    public void InstantiatePrefabs(MolaMesh mesh)
    {
        // delete previous building prefabs
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            // destroyimmediate will excute in each iteration, as a result always delete the first child because previous first child is gone
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // instantiate new building prefabs for each plot mesh face
        for (int i = 0; i < mesh.FacesCount(); i++)
        {
            float x = mesh.FaceEdgeLength(i, 0);
            float y = mesh.FaceEdgeLength(i, 1);
            if (x >= 5 && y >= 5 && Random.value>0.1) // minimal prefab size
            {
                var prefabLoad = Resources.Load<GameObject>("LOD_Voxel"); // change to your prefab name. it has to be in "Resources" folder.
                GameObject LODPrefab = Instantiate(prefabLoad, transform);

                LODPrefab.GetComponent<MolaLOD>().LODs[2].GetComponent<LOD2_Voxel>().startMesh = mesh.CopySubMesh(i, false);
                LODPrefab.GetComponent<MolaLOD>().LODs[2].GetComponent<LOD2_Voxel>().dimZ = Random.Range(5, 80);
            }
        }
    }
}
