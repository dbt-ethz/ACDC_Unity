using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mola;
using UnityEditor;

public class BlockFromMesh : MolaMonoBehaviour
{
    void Start()
    {
        InitMesh();
        UpdateGeometry();
        DestroyImmediate(this);
    }

    public override void UpdateGeometry()
    {
        if (startMesh == null) return;
        FillUnityMesh(startMesh);
        ColorMeshRandom();

        ClearChildrenImmediate();

        GameObject blockGo = new GameObject();
        blockGo.transform.parent = transform;
        blockGo.name = gameObject.name;

        var savePath = "Assets/" + blockGo.name + ".asset";
        AssetDatabase.CreateAsset(unityMesh, savePath);
    }
}
