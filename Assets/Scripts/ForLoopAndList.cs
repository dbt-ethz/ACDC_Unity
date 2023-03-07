using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ForLoopAndList : MonoBehaviour
{
    // 02
    [Range (0, 20)]
    public int x;
    [Range(0, 20)]
    public int z;
    [Range(0, 20)]
    public float distance;

    // 06 list
    private List<GameObject> myCubes;
    private void OnValidate() 
    { 
        UnityEditor.EditorApplication.delayCall += UpdateGeometry; 
    }
 
    private void UpdateGeometry()
    {
        int n = transform.childCount;
        for (int i = 0; i < n; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        myCubes = new List<GameObject>();

        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                //02 nested for loop
                //Debug.Log(i * z + j);

                // 03 create box for each iteration
                GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myCube.transform.SetParent(this.transform);
                // 03.5 extension: vector and vector math
                
                // 04 assign random height to cubes
                float height = Random.Range(3, 20); // (3, 10)
                myCube.transform.localScale = new Vector3(3, height, 3); // (1, height, 1)
                myCube.transform.localPosition = new Vector3(i * distance, height * 0.5f, j * distance);


                // 05 assign color
                float value = UtilsMath.Remap(i * z + j, 0, x * z, 0, 1);

                Renderer renderer = myCube.GetComponent<Renderer>();
                Material tempMats = new Material(renderer.sharedMaterial);
                tempMats.color = Color.HSVToRGB(value, 1, 1);
                renderer.material = tempMats;

                // 06
                myCubes.Add(myCube);
            }
        }

        for (int i = 0; i < (int)(x * z * 0.3); i++)
        {
            int index = Random.Range(0, myCubes.Count);
            DestroyImmediate(myCubes[index]);
            myCubes.RemoveAt(index);
        }
    }


}
