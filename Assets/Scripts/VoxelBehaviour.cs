using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System;
using Random = UnityEngine.Random;
using Mathf = Mola.Mathf;
using Color = Mola.Color;

public class VoxelBehaviour : MolaMonoBehaviour
{
    public int nX = 30;
    public int nY = 30;
    public int nZ = 30;
    [Range(0, 50)]
    public float radius = 0;
    [Range(0, 10)]
    public float scale = 1;
    [Range(0, 5)]
    public float thickness = 0.5f;
    [Range(0, 1)]
    public float factor = 0.001f;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    public override void UpdateGeometry()
    {
        //// create sphere
        //Vec3 centerPt = new Vec3(20, 20, 20);
        //MolaGrid<bool> sphere = SphereGrid(centerPt, radius);

        //// create sphere2
        //centerPt = new Vec3(30, 30, 30);
        //MolaGrid<bool> sphere2 = SphereGrid(centerPt, radius*0.8f);

        //// create cube
        //MolaGrid<bool> cube = BoxGrid(10, 10, 10, 20, 30, 50);

        //MolaGrid<bool>  result = UtilsGrid.GridBooleanUnionList(new List<MolaGrid<bool>> { sphere, sphere2, cube });

        MolaGrid<float> gyroidDist = GyroidDistance(0, 0, 0, scale);
        Vec3 centerPt = new Vec3(0, 0, 0);
        MolaGrid<float> sphereDist = SphereDistanceSquare(centerPt, radius);

        MolaGrid<bool> result = Overlay(gyroidDist, sphereDist, factor, thickness);

        MolaMesh molaMesh = UtilsGrid.VoxelMesh(result, Color.red);
        FillUnityMesh(molaMesh, true);
    }

    public MolaGrid<bool> Overlay(MolaGrid<float> gridA, MolaGrid<float> gridB, float factor = 0.1f, float thickness = 0.5f)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        MolaGrid<float> gridDistance = new MolaGrid<float>(nX, nY, nZ);

        for (int i = 0; i < gridDistance.Count; i++)
        {
            gridDistance[i] = gridA[i] + gridB[i] * factor;
            grid[i] = gridDistance[i] > thickness;
        }

        return grid;
    }
    private MolaGrid<bool> SphereGrid(Vec3 centerPt, float radius)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
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
        return grid;
    }
    private MolaGrid<bool> RandomGrid()
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = Random.value > 0.5;
        }
        return grid;
    }
    private MolaGrid<bool> BoxGrid(int x1, int y1, int z1, int x2, int y2, int z2)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int x = x1; x < x2; x++)
        {
            for (int y = y1; y < y2; y++)
            {
                for (int z = z1; z < z2; z++)
                {
                    grid[x, y, z] = true;
                }
            }
        }
        return grid;
    }
    private MolaGrid<bool> GyroidGrid(float x = 0, float y = 0, float z = 0, float scale = 1, float thickness = 0.5f)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            float distValue = Mola.Mathf.Sin((grid.getX(i) - x) / scale) + Mola.Mathf.Sin((grid.getY(i) - y) / scale) + Mola.Mathf.Sin((grid.getZ(i) - z) / scale);
            grid[i] = Mola.Mathf.Abs(distValue) < thickness;
        }
        return grid;
    }
    private MolaGrid<float> GyroidDistance(float x = 0, float y = 0, float z = 0, float scale = 1) 
    {
        MolaGrid<float> grid = new MolaGrid<float>(nX, nY, nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            float newScale = (grid.getX(i) + 1) / 15;
            float distValue = Mola.Mathf.Sin((grid.getX(i) - x) / scale) + Mola.Mathf.Sin((grid.getY(i) - y) / scale) + Mola.Mathf.Sin((grid.getZ(i) - z) / scale);
            //float distValue = Mola.Mathf.Sin((grid.getX(i) - x) / newScale) + Mola.Mathf.Sin((grid.getY(i) - y) / newScale) + Mola.Mathf.Sin((grid.getZ(i) - z) / newScale);
            grid[i] = distValue;
        }
        return grid;
    }
    private MolaGrid<float> SphereDistanceSquare(Vec3 centerPt, float radius)
    {
        MolaGrid<float> grid = new MolaGrid<float>(nX, nY, nZ);
        float radiusSquare = Mathf.Pow(radius, 2);
        for (int i = 0; i < grid.Count; i++)
        {
            float dx = Mathf.Pow(grid.getX(i) - centerPt.x, 2);
            float dy = Mathf.Pow(grid.getY(i) - centerPt.y, 2);
            float dz = Mathf.Pow(grid.getZ(i) - centerPt.z, 2);

            float dSquare = dx + dy + dz;

            grid[i] = dSquare - radiusSquare;
        }
        return grid;
    }

}
