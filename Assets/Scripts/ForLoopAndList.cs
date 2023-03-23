using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoopAndList : MonoBehaviour
{
    [Range (0, 20)]
    public int x;
    [Range(0, 20)]
    public int z;
    [Range(0, 20)]
    public float distance;
    private List<GameObject> myCubes;

    private void UpdateGeometry()
    {
        // delete cubes from previous run
        int n = transform.childCount;

        for (int i = 0; i < n; i++)
        {
            DestroyImmediate(transform.GetChild(0).gameObject);
        }

        // empty list before generating new cubes
        myCubes = new List<GameObject>();

        // nested for loop
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                //Debug.Log(i * z + j);

                // create box for each iteration
                GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myCube.transform.SetParent(this.transform);

                // assign random height to cubes, relocate them to designed position
                float height = Random.Range(3, 20); 
                myCube.transform.localScale = new Vector3(3, height, 3); 
                myCube.transform.localPosition = new Vector3(i * distance, height * 0.5f, j * distance);

                // assign color using unity built in map function
                float value = Remap(i * z + j, 0, x * z, 0, 1);

                // create temp material so the default material will not be overwirte. this part is not important for now
                Renderer renderer = myCube.GetComponent<Renderer>();
                Material tempMats = new Material(renderer.sharedMaterial);
                tempMats.color = Color.HSVToRGB(value, 1, 1);
                renderer.material = tempMats;

                // add cube to list
                myCubes.Add(myCube);
            }
        }

        // randomly reduce 30% of cubes
        for (int i = 0; i < x * z * 0.3; i++)
        {
            int index = Random.Range(0, myCubes.Count);
            DestroyImmediate(myCubes[index]);
            myCubes.RemoveAt(index);
        }
    }
    public static float Remap(float value, float input1, float input2, float output1, float output2)
    {
        return (output2 - output1) * value / (input2 - input1) + output1;
    }

    // This part is required for unity. Don’t change it (It allows the script to run on inspector window change.)
    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.delayCall += UpdateGeometry;
#endif
    }

}
