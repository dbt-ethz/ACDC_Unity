using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class VoxelCellularAutomata : MolaMonoBehaviour
{
    // public variables
    [Range(1, 256)]
    public int nX = 8;
    [Range(1, 256)]
    public int nY = 4;
    [Range(1, 256)]
    public int nZ = 1;

   

    [Range(0.0f, 1f)]
    public float density = 0.5f;
    public bool doFrame = false;

    // Start is called before the first frame update
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    // Update is called once per frame
    public override void UpdateGeometry()
    {
        // create a first row of 
        MolaGrid<bool> grid = new MolaGrid<bool>(nX, nY, nZ);
        for (int x = 0; x < nX; x++)
        {
            for (int y = 0; y < nY; y++)
            {
                    float randomValue = UnityEngine.Random.Range(0.0f, 1f);
                    grid.SetValue(x, y, 0, randomValue < density);
            }
        }

       
        // we iterate throuh all z layers.
        // each z layer determines the states of the upper z layer based on Conways Game of Life
        for (int z = 0; z < nZ-1; z++)
        {
            // now we iterate through all cells of a z layer
            // first we count the solid neighbours of each cell
            // then we assign the state to the upper cell
            for (int x = 1; x < nX - 1; x++)
            {
                for (int y = 1; y < nY - 1; y++)
                {
                    int solidNeighbours = 0;
                    bool cellValue = grid.GetValue(x, y, z);
                    // count the neighbours
                    for (int cX = x - 1; cX <= x + 1; cX++)
                    {
                        for (int cY = y - 1; cY <= y + 1; cY++)
                        {
                            // make sure you don't count yourself
                            if (cX != x || cY != y)
                            {
                                if (grid.GetValue(cX, cY, z))
                                {
                                    solidNeighbours++;
                                }
                            }

                        }
                    }

                    // assign next state based on neighbours and own state
                    if (cellValue && solidNeighbours < 2)
                    {
                        grid.SetValue(x, y, z + 1, false);
                    }
                    else if (cellValue && (solidNeighbours == 2 || solidNeighbours == 3))
                    {
                        grid.SetValue(x, y, z + 1, true);
                    }
                    else if (!cellValue && (solidNeighbours == 3 || solidNeighbours == 3))
                    {
                        grid.SetValue(x, y, z + 1, true);
                    }
                    else if (!cellValue)
                    {
                        grid.SetValue(x, y, z + 1, false);
                    }
                }
            }
        }

        // optional subdivision of the result
        MolaMesh mesh = UtilsGrid.VoxelMesh(grid);
        if (doFrame)
        {
            mesh = MeshSubdivision.SubdivideMeshExtrudeTapered(mesh, 0, 0.2f, false);
        }

        // this is required to display it in the unity orientation (Y is upward)
        mesh.FlipYZ();
        mesh.FlipFaces();

        FillUnityMesh(mesh);
    }


    public void OnValidate()
    {
        UpdateGeometry();
    }

}
