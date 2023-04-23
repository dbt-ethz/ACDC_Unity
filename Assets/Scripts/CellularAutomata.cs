using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System;
//using netDxf.Entities;

public class CellularAutomata : MolaMonoBehaviour
{
    readonly int nX = 64;
    readonly int nY = 32;
    readonly int nZ =128;

   
    public int cZ = 3;
    MolaGrid<bool> grid;
    // Start is called before the first frame update
    void Start()
    {
        this.InitMesh();
        //grid = new MolaGrid<bool>(nX, nY, nZ);
    }


    private void OnValidate()
    {
        grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int x = 0; x < nX; x++)
        {
            for (int y = 0; y < nY; y++)
            {
                grid.SetValue(x, y, 0, UnityEngine.Random.Range(0f, 1f) > 0.5f);
            }
        }

       
        // run process layer by layer
        for (int z = 0; z < cZ; z++)
        {
            Debug.Log("z: "+z);
            for (int x = 1; x < nX - 1; x++)
            {
                for (int y = 1; y < nY - 1; y++)
                {
                    bool cell_state = grid.GetValue(x, y, z);
                    int nbs = 0;
                    for (int cx = -1; cx < 2; cx++)
                    {
                        for (int cy = -1; cy < 2; cy++)
                        {
                            if (cx != 0 || cy != 0)
                            {
                                if (grid.GetValue(x + cx, y + cy, z))
                                {
                                    nbs ++;
                                }
                            }
                        }
                    }

                    if (cell_state && nbs< 2){
                        grid.SetValue(x, y, z + 1,false) ; // underpopulation
                    }
                    else if (cell_state && (nbs == 2 || nbs == 3))
                    {
                        grid.SetValue(x, y, z + 1, true); // stable state
                    }
                    else if (!cell_state && (nbs == 3 || nbs == 3))
                    {
                        grid.SetValue(x, y, z + 1, true); // reproduction
                    }
                    else if (!cell_state)
                    {
                        grid.SetValue(x, y, z + 1, false); // overpopulation
                    }
                }
            }
        }

        MolaMesh mesh = UtilsGrid.VoxelMesh(grid);
        mesh.FlipYZ();
        mesh.FlipFaces();
        FillUnityMesh(mesh);
       

    }

}
