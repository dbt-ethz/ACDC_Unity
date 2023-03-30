using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System;
using Random = UnityEngine.Random;
using Mathf = Mola.Mathf;
using Color = Mola.Color;
using System.Linq;

public class VoxelBehaviour : MolaMonoBehaviour
{
    public int nX = 100;
    public int nY = 100;
    public int nZ = 100;
    [Range(0, 50)]
    public float radius = 0;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    private void UpdateGeometry()
    {
        // create sphere
        Vec3 centerPt = new Vec3(20, 20, 20);
        MolaGrid<bool> sphere = SphereGrid(centerPt, radius);

        // create sphere2
        centerPt = new Vec3(30, 30, 30);
        MolaGrid<bool> sphere2 = SphereGrid(centerPt, radius*0.8f);

        // create cube
        MolaGrid<bool> cube = CubeGrid(10, 10, 10, 20, 30, 50);

        MolaGrid<bool>  result = GridBooleanUnionList(new List<MolaGrid<bool>> { sphere, sphere2, cube });
        MolaMesh molaMesh = UtilsGrid.VoxelMesh(result, Color.red);
        FillUnityMesh(molaMesh);
    }

    private MolaGrid<bool> GridBooleanUnion(MolaGrid<bool> grid1, MolaGrid<bool> grid2)
    {
        MolaGrid<bool> result = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] = grid1[i] || grid2[i];
        }
        return result;
    }
    private MolaGrid<bool> GridBooleanUnionList(List<MolaGrid<bool>> grids)
    {
        MolaGrid<bool> result = new MolaGrid<bool>(nX, nY, nZ);

        var listsOfLists = new List<IEnumerable<bool>>();
        foreach (MolaGrid<bool> grid in grids)
        {
            listsOfLists.Add(grid.Values);
        }
        var combinedResults = listsOfLists.Aggregate((a, b) => a.Zip(b, (aElement, bElement) => aElement || bElement));
        result.Values = combinedResults.ToList();

        return result;
    }
    private MolaGrid<bool> GridBooleanIntersection(MolaGrid<bool> grid1, MolaGrid<bool> grid2)
    {
        MolaGrid<bool> result = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] = grid1[i] && grid2[i];
        }
        return result;
    }
    private MolaGrid<bool> GridBooleanDifference(MolaGrid<bool> grid1, MolaGrid<bool> grid2)
    {
        MolaGrid<bool> result = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < result.Count; i++)
        {
            result[i] = grid1[i] && ! grid2[i];
        }
        return result;
    }

    private MolaGrid<bool> SphereGrid( Vec3 centerPt, float radius)
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
    private MolaGrid<bool> CubeGrid(int x1, int y1, int z1, int x2, int y2, int z2)
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
    private MolaGrid<bool> GyroidGrid(int nX, int nY, int nZ)
    {
        throw new NotImplementedException();
    }
}
