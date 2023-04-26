using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class PlotSubdivide : MolaMonoBehaviour
{
    [Range(10, 300)]
    public float dX = 100;
    [Range(10, 300)]
    public float dY = 200;
    void Start()
    {
        InitMesh();
        UpdateGeometry();
    }

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += UpdateGeometry;
#endif
    }

    public override void UpdateGeometry()
    {
        MolaMesh mesh = MeshFactory.CreateQuad(dX, dY);

        mesh = MeshSubdivision.SubdivideMeshGrid(mesh, 4, 5);
        mesh = MeshSubdivision.SubdivideMeshSplitFrame(mesh, 2);

        molaMeshes = new List<MolaMesh> { mesh };
        FillUnitySubMesh(molaMeshes);
    }
}
