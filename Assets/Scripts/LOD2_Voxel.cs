using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;
using Unity.XRTools.Utils;

public class LOD2_Voxel : MolaMonoBehaviour
{
    [Range(0.1f, 10)]
    public float scale = 3;

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
        // create mola mesh for current LOD level
        MolaMesh molaMesh = MeshFactory.CreateSingleQuad(dimX/2, -dimY / 2, 0, 0, dimY / 2, 0, -dimX / 4, dimY / 4, 0, -dimX / 2, -dimY / 2, 0, false);
        MolaMesh floor = startMesh ?? molaMesh;

        FillUnityMesh(floor, true);
        if (unityMesh != null)
        {
            Bounds bound = unityMesh.bounds;

            List<Vector3> polygon = new List<Vector3>();
            foreach (var v in floor.Vertices)
            {
                polygon.Add(new Vector3(v.x, 0, v.y));
            }

            dimX = (int)bound.size.x;
            dimY = (int)bound.size.z;

            MolaGrid<bool> gyroid = GyroidGrid(0, 0, 0, scale);
            //MolaGrid<bool> solid = Solid();
            MolaGrid<bool> clipping = ClippingGrid(bound, polygon);
            MolaGrid<bool> result = UtilsGrid.GridBooleanIntersection(gyroid, clipping);

            MolaMesh volume = UtilsGrid.VoxelMesh(result, 1);

            molaMeshes = new List<MolaMesh>() { volume };
            FillUnitySubMesh(molaMeshes, true);
            ColorSubMeshRandom();
            this.transform.parent.localPosition = bound.center - new Vector3(dimX/2, 0, dimY/2);
            
            UpdateLOD();
        }

    }
    private MolaGrid<bool> GyroidGrid(float x = 0, float y = 0, float z = 0, float scale = 1)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>((int)dimX, (int)dimY, (int)dimZ);
        for (int i = 0; i < grid.Count; i++)
        {
            float distValue = Mola.Mathf.Sin((grid.getX(i) - x) / scale) + Mola.Mathf.Sin((grid.getY(i) - y) / scale) + Mola.Mathf.Sin((grid.getZ(i) - z) / scale);
            grid[i] = Mola.Mathf.Abs(distValue) < 0.5;
        }
        return grid;
    }
    private MolaGrid<bool> ClippingGrid(Bounds bound,List<Vector3> polygon)
    {
        MolaGrid<bool> grid = new MolaGrid<bool>((int)dimX, (int)dimY, (int)dimZ);
        for (int i = 0; i < grid.Count; i++)
        {
            Vector3 v = new Vector3(grid.getX(i) - dimX / 2, 0, grid.getY(i) - dimY / 2) + bound.center;
            if (GeometryUtils.PointInPolygon(v, polygon))
            {
                grid[i] = true;
            }
        }
        return grid;
    }
    private MolaGrid<bool> Solid()
    {
        MolaGrid<bool> grid = new MolaGrid<bool>((int)dimX, (int)dimY, (int)dimZ);
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = true;
        }
        return grid;
    }
}
