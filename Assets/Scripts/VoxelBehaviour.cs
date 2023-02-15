using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class VoxelBehaviour : MonoBehaviour
{
    public int nX = 100;
    public int nY = 100;
    public int nZ = 100;
    [Range(0, 50)]
    public float radius = 0;

    private MolaGrid<bool> grid;
    private Mesh mesh;

    void Start()
    {
        Debug.Log("start is called");
        //InitMesh();
    }

    private void OnValidate()
    {
        InitMesh();
        InitGrid();
        SphereGrid(new Vector3(20, 20, 20), radius);
        MolaMesh molaMesh = UtilsGrid.VoxelMesh(grid, Color.red);
        Debug.Log($"quad count: {molaMesh.FacesCount()}");
        molaMesh.FillUnityMesh(mesh);
    }

    private void InitGrid()
    {
        grid = new MolaGrid<bool>(nX, nY, nZ);
    }

    private void RandomGrid()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = Random.value > 0.5;
        }
    }

    private void SphereGrid(Vector3 centerPt, float radius)
    {
        float radiusSquare = Mathf.Pow(radius, 2);
        for (int i = 0; i < grid.Count; i++)
        {
            float dx = Mathf.Pow(grid.getX(i) - centerPt.x, 2);
            float dy = Mathf.Pow(grid.getY(i) - centerPt.y, 2);
            float dz = Mathf.Pow(grid.getZ(i) - centerPt.z, 2);

            float dSquare = dx + dy + dz;
            if (dSquare <= radiusSquare)
            {
                grid[i] = true;
            }
        }
    }

    private MolaGrid<bool> CustomizedGrid(int nX, int nY, int nZ)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int x = 10; x < 20; x++)
        {
            for (int y = 0; y < 30; y++)
            {
                for (int z = 10; z < 25; z++)
                {
                    grid[x, y, z] = true;
                }
            }
        }
        return grid;
    }

    private MolaGrid<bool> GyroidGrid(int nX, int nY, int nZ)
    {
        throw new NotImplementedException();
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
            renderer = gameObject.AddComponent<MeshRenderer>();
        }
        renderer.material = new Material(Shader.Find("Particles/Standard Surface"));
    }
}
