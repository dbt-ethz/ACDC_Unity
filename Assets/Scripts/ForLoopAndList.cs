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
    private List<GameObject> myCubes = new List<GameObject>();
    private void Start()
    {
        // 01 for loop
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(i);
        }

        // 02 
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                //02 nested for loop
                Debug.Log(i * x + j);

                // 03 create box for each iteration
                GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myCube.transform.SetParent(this.transform);
                // 03.5 extension: vector and vector math
                myCube.transform.localPosition = new Vector3(i * distance, 0, j * distance);

                // 04 assign random height to cubes
                float height = Random.Range(3, 20); // (3, 10)
                myCube.transform.localScale = new Vector3(3, height, 3); // (1, height, 1)
                myCube.transform.Translate(new Vector3(0, height * 0.5f, 0));

                // 05 assign color
                float value = Map(i * x + j, 0, x * z, 0, 1);
                myCube.GetComponent<Renderer>().material.color = Color.HSVToRGB(value, 1, 1);

                // home work: how to make cubes bigger, like real buildings?

                // 06
                myCubes.Add(myCube);
            }
        }

        // 06
        Debug.Log(myCubes.Count);
        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, myCubes.Count);
            Destroy(myCubes[index]);
            myCubes.RemoveAt(index);
        }
    }

    // 05 assign color
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float delta = fromMax - fromMin;
        return toMin + ((toMax - toMin) / delta) * (value - fromMin);
    }

}
