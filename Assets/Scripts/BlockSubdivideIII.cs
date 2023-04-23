using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class BlockSubdivideIII : MolaMonoBehaviour
{
    public float dimX = 274;
    public float dimY = 80;
    public float dimZ = 200;
    [Range(1, 5)]
    public int iteration = 3;
    [Range(0, 10)]
    public int setback = 3;
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
        
        for (int i = 0; i < iteration; i++)
        {
            block = MeshSubdivision.SubdivideMeshSplitRelative(block, 0, 0.4f, 0.4f, 0.6f, 0.6f);
        }

        block = MeshSubdivision.SubdivideMeshSplitFrame(block, roadWidth * 0.5f);

        bool[] indexMask = new bool[block.FacesCount()];
        for (int i = 0; i < block.FacesCount(); i++)
        {
            if ((i + 1) % 9 == 0) indexMask[i] = true; // every 9th face is plot. others are road
        }

        MolaMesh plots = block.CopySubMesh(indexMask);
        indexMask = indexMask.Select(a => !a).ToArray();
        MolaMesh road = block.CopySubMesh(indexMask);
        road.FlipFaces();

        molaMeshes = new List<MolaMesh> { plots, road };
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();

        // delete previous building prefabs
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            // destroyimmediate will excute in each iteration, as a result always delete the first child because previous first child is gone
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // instantiate new building prefabs for each plot mesh face
        for (int i = 0; i < plots.FacesCount(); i++)
        {
            float x = plots.FaceEdgeLength(i, 0) - setback; // setback from the edge of plot face
            float y = plots.FaceEdgeLength(i, 1) - setback;
            if (x >= 5 && y >= 5) // minimal prefab size
            {
                var prefabLoad = Resources.Load<GameObject>("LOD_GroupName"); // change to your prefab name. it has to be in "Resources" folder.
                GameObject LODPrefab = Instantiate(prefabLoad, transform);

                // this part could be optimized
                LODPrefab.GetComponent<MolaLOD>().LODs[2].GetComponent<LOD2_GroupName>().dimX = x;
                LODPrefab.GetComponent<MolaLOD>().LODs[2].GetComponent<LOD2_GroupName>().dimY = y;
                LODPrefab.GetComponent<MolaLOD>().LODs[2].GetComponent<LOD2_GroupName>().dimZ = Random.Range(5, 100);

                // adjust the location and rotation of building prefab to suit plot face
                Vector3 center = new Vector3(plots.FaceCenter(i).x, plots.FaceCenter(i).z, plots.FaceCenter(i).y); // swap yz here
                LODPrefab.transform.localPosition = center;

                float angle = plots.FaceAngleHorizontal(i); // the angle between face's first edge and world X axis
                //LODPrefab.transform.localRotation = UnityEngine.Quaternion.Euler(new Vector3(0, angle, 0));
                LODPrefab.transform.Rotate(0, angle, 0, Space.World);
            }
        }
    }
}
