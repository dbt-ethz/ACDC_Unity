using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using System.Linq;
using System;

public class BuildingLOD0 : MolaMonoBehaviour
{
    [Range(0, 10)]
    public int seed = 5;
    public List<MolaMesh> molaMeshes;
    public BuildingLOD1 LOD1;

    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }
    private void OnValidate()
    {
        UpdateGeometry();
    }
    public void UpdateGeometry()
    {
        // inherit from previous level
        MolaMesh wall = new MolaMesh();
        MolaMesh roof = new MolaMesh();
        if (LOD1 != null)
        {
            molaMeshes = LOD1.molaMeshes;
            if (molaMeshes != null)
            {
                //wall = molaMeshes.Find(item => item.Name == "wall");
                //roof = molaMeshes.Find(item => item.Name == "roof");
                wall = molaMeshes[0];
                roof = molaMeshes[1];
            }
        }

        // operation in current level
        MolaMesh window = new MolaMesh();
        wall = MeshSubdivision.SubdivideMeshExtrudeTapered(wall, 1, 0.2f);

        bool[] indexMusk = new bool[wall.FacesCount()];
        for (int i = 0; i < wall.FacesCount(); i++)
        {
            indexMusk[i] = i % 4 == 0;
        }
        window = wall.CopySubMesh(indexMusk);

        indexMusk = indexMusk.Select(a => !a).ToArray();
        wall = wall.CopySubMesh(indexMusk);

        molaMeshes = new List<MolaMesh>() { wall, roof, window };

        FillUnitySubMesh(molaMeshes);
        ColorSubMeshRandom();
    }
}
