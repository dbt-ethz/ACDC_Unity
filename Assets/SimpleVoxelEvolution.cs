using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HD;
public class SimpleVoxelEvolution : MonoBehaviour
{
    int nX=16;
    int nY=7;
    int nZ=16;
    Mesh mesh;
    HDGrid<bool> grid;
    HDGrid<bool> backupGrid;
    float fitness;
    int[][] nbs;
    float[] distances;
    HDDirectedGraph graph;
    // Start is called before the first frame update
    void Start()
    {
        grid=InitGrid(nX,nY,nZ);
        nbs=this.InitNbs();
        backupGrid= InitGrid(nX, nY, nZ);
        InitMesh();
        ResetGrid();
        InitGraph();
        
        fitness = CalculateFitness();
    }

    public void InitGraph()
    {
        graph = new HDDirectedGraph(grid.Count);
        for (int i = 0; i < nbs.Length; i++)
        {
            int[] nodeNbs = nbs[i];
            for (int j = 0; j < nodeNbs.Length; j++)
            {
                if (nodeNbs[j] >= 0)
                {
                    graph.AddDoubleEdge(i, nodeNbs[j]);
                }
                
            }
          
        }
    }
    public void InitMesh()
    {
        MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        meshFilter.mesh = mesh;
    }

    public void MutateForBetter()
    {
        CopyStates(grid, backupGrid);
        Mutate();
        float newFitness = CalculateFitness();
        if (newFitness < fitness)
        {
            CopyStates(backupGrid,grid);
        }
        else
        {
            fitness = newFitness;
           
        }
    }
    float CalculateFitness()
    {
        float fitness = 0;
        for (int i = 0; i < grid.Count; i++)
        {
            if (grid[i])
            {
                int warmNbs = 0;
                int[] cellNbs = nbs[i];
                for (int j = 0; j < cellNbs.Length; j++)
                {
                    int nb = cellNbs[j];
                    if (nb >= 0 && grid[nb])
                    {
                        warmNbs++;
                    }
                }

                fitness -= Mathf.Abs(warmNbs - 4);
            }
        }
        Dijkstra dijkstra=new Dijkstra();
        distances = dijkstra.DistanceToAll(graph,0);
        return fitness;
    }

    void Mutate()
    {
        int i = Random.Range(0, grid.Count);
        int i2 = Random.Range(0, grid.Count);
        bool newValue = grid[i];
        grid[i] = grid[i2];
        grid[i2] = newValue;
    }

    void CopyStates(HDGrid<bool> grid, HDGrid<bool> copyTarget)
    {
        grid.CopyTo((System.Array)copyTarget.Values,0);
    }

    HDGrid<bool> InitGrid(int nX, int nY, int nZ)
    {
        HDGrid<bool> grid = new HDGrid<bool>(nX,nY,nZ);
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = Random.value > 0.5;
        }
        return grid;
    }



    void ResetGrid()
    {
        for (int i = 0; i < grid.Count; i++)
        {
            grid[i] = Random.value > 0.5;
        }
    }

    public int[][] InitNbs()
    {
        return grid.GetXYZNbs6();

    }

    public float GetMin(IEnumerable<float> values)
    {
        float min = float.MaxValue;
        foreach (float value in values)
        {
            min = Mathf.Min(min, value);
        }
        return min;
    }
    public float GetMax(IEnumerable<float> values)
    {
        float max = float.MinValue;
        foreach (float value in values)
        {
            max = Mathf.Max(max, value);
        }
        return max;
    }
    public void mapFloatList(float[] values,float target1, float target2)
    {
        float min = GetMin(values);
        float max = GetMax(values);
        float deltaSoure = max - min;
        float deltaTarget = target2 - target1;
        for (int i = 0; i < values.Length; i++)
        {
            values[i] = ((values[i] - min) / deltaSoure) * deltaTarget + target1;
        }
    }
    // c#
    float map(float s, float source1, float source2, float target1, float target2)
    {
        return target1 + (s - source1) * (target2 - target1) / (source2 - source1);
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < 100; i++)
        {
            MutateForBetter();
        }
       
        HDMesh myMesh = new HDMesh();
        Color color = Color.white;
        Color colorFacade = Color.white;
        colorFacade.a = 0.5f;

        float minDist = GetMin(distances);
        float maxDist = GetMax(distances);
      
        float deltaSource = maxDist - minDist;
        float deltaTarget = 1 - 0;

        bool flagFacade = true;
        for (int x = 0; x < nX; x++)
        {
            for (int y = 0; y < nY; y++)
            {
                for (int z = 0; z < nZ; z++)
                {
                    if (grid[x,y,z])
                    {

                        int index = grid.GetIndex(x, y, z);
                        float d = distances[index];
                        float colorValue = ((d - minDist) / deltaSource) * deltaTarget ;
                        Color cColor=Color.HSVToRGB(colorValue, 1, 1);
                        //cColor = Color.white;
                        float o = 0.01f;

                        if (flagFacade) { 
                        if (x == 0|| !grid[x - 1, y, z])
                        {
                                //HDMeshFactory.AddQuadX0(myMesh, x, y, z, colorFacade);
                                HDMeshFactory.AddBox(myMesh, x, y+0.1f, z+0.1f, x+0.1f, y+1, z+1-0.1f, colorFacade);
                        }
                       
                        if (x==nX-1||!grid[x + 1, y, z])
                        {
                                //HDMeshFactory.AddQuadX1(myMesh, x, y, z, colorFacade);
                                HDMeshFactory.AddBox(myMesh, x+1-0.1f, y + 0.1f, z + 0.1f, x + 1, y + 1, z + 1 - 0.1f, colorFacade);

                            }
                            if (z == 0 || !grid[x, y, z - 1])
                        {
                                //HDMeshFactory.AddQuadZ0(myMesh, x, y, z, colorFacade);
                                HDMeshFactory.AddBox(myMesh, x + 0.1f, y + 0.1f, z , x + 1 -0.1f, y + 1, z + 0.1f, colorFacade);

                            }
                            if (z == nZ - 1 || !grid[x, y, z + 1])
                        {
                                //HDMeshFactory.AddQuadZ1(myMesh, x, y, z, colorFacade);
                                HDMeshFactory.AddBox(myMesh, x + 0.1f, y + 0.1f, z+1-0.1f, x + 1 - 0.1f, y + 1, z + 1, colorFacade);

                            }
                        }
                        if (y==nY-1||!grid[x, y+1, z])
                        {
                            //HDGridToMesh.AddQuadY1(myMesh, x, y, z, Color.green);
                        }

                        /*if (y==0||!grid[x, y - 1, z])
                        //{
                            
                        //}*/
                        HDMeshFactory.AddBox(myMesh, x+o, y, z+o, x + 1-o, y + 0.2f, z + 1-o, cColor);
                        //HDGridToMesh.AddQuadY0(myMesh, x, y, z, cColor,true);

                        
                    }
                }
            }
        }
        myMesh.TriangulateQuads();
        myMesh.SeparateVertices();
        mesh.Clear();
        mesh.vertices = myMesh.VertexArray();
        mesh.triangles = myMesh.FlattenedTriangles();
        mesh.SetColors(myMesh.Colors);
        mesh.RecalculateNormals();

        Debug.Log("time: " + System.DateTime.Now.ToLongTimeString());
    }
}