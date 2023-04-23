using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;

public class SubdivideByParamListExample : MolaMonoBehaviour
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
        // MeshSubdivide.SubdivideMeshExtrude() could take: a mesh, a height, a bool, or: a mesh, a list of heights, a list of bool
        // same with some other mesh subdivide methods. 

        MolaMesh sphere = MeshFactory.CreateSphere();

        // create paramList, the count of paramList should equal face count of the mesh
        List<float> paramList = new List<float>();
        // bool list to decide if the result cap top. 
        List<bool> doCaps = new List<bool>();
        for (int i = 0; i < sphere.FacesCount(); i++)
        {
            // for each face in sphere mesh, add a value to the param list.
            // this value could be face area, face perimeter, face vertical angle, face horizontal angle...
            paramList.Add(sphere.FaceArea(i));
            doCaps.Add(true);
        }

        // map paramList to a new list with domain 0 to 1;
        paramList = Mola.Mathf.MapList(paramList, 0, 1);

        // extrude each face in the mesh with different parameter 
        sphere = MeshSubdivision.SubdivideMeshExtrude(sphere, paramList, doCaps);

        FillUnityMesh(sphere, true);
    }

}
