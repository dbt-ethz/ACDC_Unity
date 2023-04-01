using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class Building2_LOD2 : MolaMonoBehaviour
{
    public int nX = 30;
    public int nY = 30;
    public int nZ = 30;
    [Range(0, 10)]
    public int seed = 0;
    [Range(1, 10)]
    public float scale = 3;
    [Range(0, 100)]
    public float x = 0;
    [Range(0, 100)]
    public float y = 0;
    [Range(0, 100)]
    public float z = 0;

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
        //List<MolaGrid<bool>> spheres = new List<MolaGrid<bool>>();
        //for (int i = 0; i < 7; i++)
        //{
        //    Vec3 centerPt = new Vec3(Random.Range(0, nX), Random.Range(0, nY), Random.Range(0, nZ));
        //    int radius = Random.Range(2, 10);
        //    spheres.Add(SphereGrid(centerPt, radius));
        //}
        //MolaGrid<bool> result = UtilsGrid.GridBooleanUnionList(spheres);

        // make a gyroid grid
        MolaGrid<bool> gyroid = GyroidGrid(x, y, z, scale);

        // voxel grid to mola mesh
        MolaMesh volume = UtilsGrid.VoxelMesh(gyroid, 3);
        molaMeshes = new List<MolaMesh>() { volume };

        // create color 
        UnityEngine.Color [] colors = new UnityEngine.Color[molaMeshes.Count];
        colors[0] = new UnityEngine.Color((float)100 / 255, (float)100 / 255, (float)100 / 255);

        // convert to unitymesh, assign color
        FillUnitySubMesh(molaMeshes);
        ColorSubMesh(colors);

        UpdateLOD();

    }
    private MolaGrid<bool> SphereGrid(Vec3 centerPt, float radius)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        float radiusSquare = Mola.Mathf.Pow(radius, 2);
        for (int i = 0; i < grid.Count; i++)
        {
            float dx = Mola.Mathf.Pow(grid.getX(i) - centerPt.x, 2);
            float dy = Mola.Mathf.Pow(grid.getY(i) - centerPt.y, 2);
            float dz = Mola.Mathf.Pow(grid.getZ(i) - centerPt.z, 2);

            float dSquare = dx + dy + dz;
            if (dSquare <= radiusSquare)
            {
                grid[i] = true;
            }
        }
        return grid;
    }
    private MolaGrid<bool> GyroidGrid(float x=0, float y=0, float z=0, float scale=1)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            float distValue = Mola.Mathf.Sin((grid.getX(i) - x) / scale) + Mola.Mathf.Sin((grid.getY(i) - y) / scale) + Mola.Mathf.Sin((grid.getZ(i) - z) / scale);
            grid[i] = Mola.Mathf.Abs(distValue) < 0.5;
        }
        return grid;
    }
}
