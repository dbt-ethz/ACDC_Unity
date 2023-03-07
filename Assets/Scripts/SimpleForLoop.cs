using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleForLoop : MonoBehaviour
{
    [Range(0, 20)]
    public int x;
    [Range(0, 20)]
    public float distance;

    // this function will run everytime anything changes in inspector window. 
    private void UpdateGeometry()
    {
        // delete cubes from previous run
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // generate a cube for each iteration in the for loop
        for (int i = 0; i < x; i++)
        {
            GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            myCube.transform.SetParent(this.transform);

            float height = Random.Range(3, 20); 
            myCube.transform.localScale = new Vector3(3, height, 3); 
            myCube.transform.localPosition = new Vector3(i * distance, height * 0.5f, 0);
        }
    }

    // This part is required for unity. Don’t change it (It allows the script to run on inspector window change.)
    private void OnValidate()
    {
        UnityEditor.EditorApplication.delayCall += UpdateGeometry;
    }
}
