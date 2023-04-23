using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;

public class LOD0_GroupName : MolaMonoBehaviour
{
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
        #region REPLACE THIS PART WITH YOUR OWN DESIGN
        // 01 get mola meshes from LOD1
        molaMeshes = GetMeshFromLOD();
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();

        if (molaMeshes.Count != 0)
        {
            wall = molaMeshes[0];
            roof = molaMeshes[1];
        }

        // 02 operate in the current level 
        MolaMesh window = new MolaMesh();
        wall = MeshSubdivision.SubdivideMeshExtrudeTapered(wall, 1, 0.2f);

        // seperate mesh into wall and window by index. every 5th face is window
        bool[] indexMusk = new bool[wall.FacesCount()];
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            indexMusk[i] = (i + 1) % 5 == 0; // get every 5th item
        }
        window = wall.CopySubMesh(indexMusk);

        indexMusk = indexMusk.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(indexMusk);

        molaMeshes = new List<MolaMesh>() { wall, roof, window };
        #endregion

        // visualize current 
        FillUnitySubMesh(molaMeshes, true);
        ColorSubMeshRandom();
    }
}
