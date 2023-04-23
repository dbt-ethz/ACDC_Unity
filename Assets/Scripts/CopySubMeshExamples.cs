using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class CopySubMeshExamples : MolaMonoBehaviour
{
    [Range(5, 100)]
    public float dimX = 10;
    [Range(5, 100)]
    public float dimY = 10;
    [Range(5, 100)]
    public float dimZ = 10;
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
        // this example shows different ways to split a mola mesh into submeshes for further operation

        MolaMesh floor = MeshFactory.CreateSingleQuad(dimX / 2, -dimY / 2, 0, dimX / 2, dimY / 2, 0, -dimX / 2, dimY / 2, 0, -dimX / 2, -dimY / 2, 0, false);

        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        floor = MeshSubdivision.SubdivideMeshExtrude(floor, 6);

        // copy submesh by single index
        roof = floor.CopySubMesh(4, false); 
        // copy submesh by a list of indices
        wall = floor.CopySubMesh(new List<int>() { 0, 1, 2, 3 }); 

        wall = MeshSubdivision.SubdivideMeshGrid(wall, 3, 3);



        // copy submesh by random bool mask
        MolaMesh newWall = new MolaMesh();
        bool[] randomMask = new bool[wall.FacesCount()]; // create a bool array with a length same as the wall mesh face count
        for (int i = 0; i < randomMask.Length; i++)
        {
            if (UnityEngine.Random.value > 0.5) randomMask[i] = true; // randomly assign 50% of the bool values to true
        }
        newWall = wall.CopySubMesh(randomMask); // copy thoes 50% faces to newWall

        // flip the bool mask. for each value in the array, true = false and false = true
        randomMask = randomMask.Select(a => !a).ToArray();
        // copy the other 50% to wall mesh(the result wall is already a new mesh)
        wall = wall.CopySubMesh(randomMask);

        //subdivide newWall
        newWall = MeshSubdivision.SubdivideMeshExtrude(newWall, 3);



        // copy submesh by orientation bool mask
        MolaMesh newRoof = new MolaMesh();
        bool[] orientationMask = new bool[newWall.FacesCount()];
        for (int i = 0; i < orientationMask.Length; i++)
        {
            // if face is more horizontal than vertical, assign the value to true.
            if (Mola.Mathf.Abs(UtilsFace.FaceAngleVertical(newWall.FaceVertices(i))) > 1) //face angle vertical: return 0 if it is vertical, -PI/2 downwards, PI/2 updwards
            {
                orientationMask[i] = true;
            }
        }
        newRoof = newWall.CopySubMesh(orientationMask);
        // flip mask
        orientationMask = orientationMask.Select(a => !a).ToArray();
        newWall = newWall.CopySubMesh(orientationMask);

        wall.AddMesh(newWall); // merge newWall back to wall
        roof.AddMesh(newRoof); // merge newRoof fack to roof

        // subdivide wall to make windows
        MolaMesh window = new MolaMesh();
        wall = MeshSubdivision.SubdivideMeshExtrudeTapered(wall, 0, 0.2f);

        // copy submesh by index bool mask. every 5th face is window
        bool[] indexMusk = new bool[wall.FacesCount()];
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            indexMusk[i] = (i + 1) % 5 == 0; // get every 5th item: 4, 9, 14, 19... 
        }
        window = wall.CopySubMesh(indexMusk);

        // flip the bool mask
        indexMusk = indexMusk.Select(a => !a).ToArray();
        // copy the remaining faces to a wall mesh
        wall = wall.CopySubMesh(indexMusk);


        molaMeshes = new List<MolaMesh>() { wall, roof, window };
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();
    }

}
