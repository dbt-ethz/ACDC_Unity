using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class CitySubdivide : MolaMonoBehaviour
{
    public float cityDimX = 1000;
    public float cityDimY = 600;
    public float cityDimZ = 200;
    [Range(2, 50)]
    public int roadWidth = 20;
    [Range(1, 5)]
    public int iteration = 4;
    [Range(0, 10)]
    public int seed = 0;
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    public override void UpdateGeometry()
    {
        MolaMesh city = MeshFactory.CreateSingleQuad(cityDimX / 2, -cityDimY / 2, 0, cityDimX / 2, cityDimY / 2, 0, -cityDimX / 2, cityDimY / 2, 0, -cityDimX / 2, -cityDimY / 2, 0, false);

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

        molaMeshes = new List<MolaMesh> { road };
        FillUnitySubMesh(molaMeshes, true);
        UnityEngine.Color color = new UnityEngine.Color(0, 0, 0);
        ColorMesh(color);

        InstantiateBlocks(blocks);
    }
    private void InstantiateBlocks(MolaMesh blocks)
    {
        ClearChildrenImmediate();

        string groupName = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        List<int> parks = new List<int> { 0};
        int groupIndex = 0;
        for (int i = 0; i < blocks.FacesCount(); i++)
        {
            int n = i / groupName.Length;
            GameObject blockGO = new GameObject();
            if (parks.Contains(i))
            {
                blockGO.name = "Park" + i + n;
            }
            else
            {
                if(n == 0) blockGO.name = "Block" + groupName[groupIndex];
                else blockGO.name = "Block" + groupName[groupIndex] + n;

                groupIndex++;
            }

            blockGO.transform.parent = transform;
            MolaMesh blockMesh = blocks.CopySubMesh(i);
            InstantiateBlock(blockGO, blockMesh);
        }
        Debug.Log("saved mesh");
    }
    private void InstantiateBlock(GameObject blockGO, MolaMesh molaMesh)
    {
        // pass on molaMesh
        BlockFromMesh blockFromMesh = GetComponent<BlockFromMesh>();
        if(blockFromMesh == null)
        {
            blockFromMesh = blockGO.AddComponent<BlockFromMesh>();
        }
        blockFromMesh.startMesh = molaMesh;
    }
}
