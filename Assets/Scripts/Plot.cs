using UnityEngine;
using Mola;
using System;
using System.Collections.Generic;

public class Plot : MolaMonoBehaviour
{
    public float dX = 100;
    public float dY = 200;

    // Start is called before the first frame update
    void Start()
    {
        this.InitMesh();
    }

    private void OnValidate()
    {
        // students need to collect here their different LODS

        MolaMesh mesh = MeshFactory.CreateQuad(dX, dY);

        mesh = MeshSubdivision.SubdivideMeshGrid(mesh, 4, 5);
        mesh = MeshSubdivision.SubdivideMeshSplitFrame(mesh, 2);
        List<int> blocks = new List<int>();
        for (int i = 0; i < mesh.Faces.Count; i++)
        {
            if (mesh.FaceNormal(i).z > 0)
            {
                blocks.Add(i);
            }
        }
        MolaMesh meshPlots = mesh.CopySubMesh(blocks);

        // adding first LOD
        AddLODMesh((meshPlots));

        MolaMesh meshStreets = mesh.CopySubMesh(blocks, true);
        meshStreets.FlipFaces();
        List<float> rndHeights = new List<float>();
        List<bool> rndCaps = new List<bool>();
        for (int i = 0; i < meshPlots.FacesCount(); i++)
        {
            rndHeights.Add(UnityEngine.Random.Range(1f, 100f));
            rndCaps.Add(true);
        }
        meshPlots = MeshSubdivision.SubdivideMeshExtrude(meshPlots, rndHeights, rndCaps);

        // adding second LOD
        AddLODMesh(meshPlots);

        // adding all LODs to the scene
        AddLODsToObject();

    }


    // the below methods could be added to the MolaMonoBehaviour

    List<GameObject> lodMeshes = new List<GameObject>();

    //now this method should have all input parameters as FillUnityMesh

    public void AddLODMesh(MolaMesh molaMesh)
    {
        lodMeshes.Add(FabricateMeshObject( molaMesh));
    }
    public void AddLODsToObject()
    {
        // if no LODGroup available, add one.
        LODGroup group;
        if (gameObject.GetComponent<LODGroup>() == null)
        {
            group = gameObject.AddComponent<LODGroup>();
        }
        else
        {
            group = gameObject.GetComponent<LODGroup>();
        }
        // clear all children and add Gameobjects as LOD.
        UnityEditor.EditorApplication.delayCall += () =>
        {
            ClearChildrenImmediate();
            // Add 4 LOD levels
            LOD[] lods = new LOD[lodMeshes.Count];
            for (int i = lodMeshes.Count - 1; i >= 0; i--)
            {
                GameObject go = lodMeshes[i];
                go.transform.parent = gameObject.transform;
                Renderer[] renderers = new Renderer[1];
                renderers[0] = go.GetComponent<Renderer>();
                int index = lodMeshes.Count - i - 1;
                lods[index] = new LOD(1.0F / (index + 1), renderers);
            }
            group.SetLODs(lods);
            group.RecalculateBounds();
            lodMeshes.Clear();
        };

    }

    // this method should have all input parameters as FillUnityMesh
    public GameObject FabricateMeshObject(MolaMesh molaMesh)
    {
        GameObject gObject = new GameObject();
        unityMesh = new Mesh();
        unityMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        unityMesh.MarkDynamic();
        MeshFilter meshFilter = gObject.GetComponent<MeshFilter>();
        if (meshFilter == null)
        {
            meshFilter = gObject.AddComponent<MeshFilter>();
        }
        UnityEditor.EditorApplication.delayCall += () =>
        {
            meshFilter.mesh = unityMesh;
        };
        

        MeshRenderer meshRenderer = gObject.GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gObject.AddComponent<MeshRenderer>();
        }
        meshRenderer.material = new Material(Shader.Find("Particles/Standard Surface"));
        HDMeshToUnity.FillUnityMesh(unityMesh, molaMesh, true);
        return gObject;
    }

    new public void ClearChildrenImmediate()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            // destroyimmediate will excute in each iteration, as a result always delete the first child because previous first child is gone
            DestroyImmediate(transform.GetChild(0).gameObject);
        }
    }

}