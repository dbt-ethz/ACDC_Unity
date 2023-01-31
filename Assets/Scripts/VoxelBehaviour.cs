using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;
using System;
using Random = UnityEngine.Random;

[ExecuteInEditMode]
public class VoxelBehaviour : MonoBehaviour
{
    public Material material;

    public int nX = 100;
    public int nY = 100;
    public int nZ = 100;
    [Range(0, 50)]
    public float radius = 0;

    private HDGrid<bool> grid;
    private Mesh mesh;
    private HDMesh hdMesh;

    void Start()
    {
        Debug.Log("start is called");
        InitMesh();
    }

    private void OnValidate()
    {
        Debug.Log("Inspector causes this Update");
        InitGrid();
        SphereGrid(new Vector3(20, 20, 20), radius);
        hdMesh = VoxelMesh(Color.red);
        //hdMesh.SeparateVertices();
        hdMesh.FillUnityMesh(mesh);
    }

    private void InitGrid()
    {
        grid = new HDGrid<bool>(nX, nY, nZ);
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

    private HDGrid<bool> CustomizedGrid(int nX, int nY, int nZ)
    {
        HDGrid<bool> grid = new HDGrid<bool>(nX, nY, nZ);
        // customization
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

    private HDGrid<bool> GyroidGrid(int nX, int nY, int nZ)
    {
        throw new NotImplementedException();
    }

    public HDMesh VoxelMesh(Color color)
    {
        HDMesh myMesh = new HDMesh();
        for (int x = 0; x < nX; x++)
        {
            for (int y = 0; y < nY; y++)
            {
                for (int z = 0; z < nZ; z++)
                {
                    if (grid[x, y, z])
                    {
                        Color colorFacade = color;
                        HDMeshFactory.AddBox(myMesh, x, y, z, x + 1, y + 1f, z + 1, colorFacade);

                    }
                }
            }
        }
        return myMesh;
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
