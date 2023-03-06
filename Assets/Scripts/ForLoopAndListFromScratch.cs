using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForLoopAndListFromScratch : MonoBehaviour
{
    [Range(0, 10)]
    public int x;
    [Range(0, 10)]
    public int z;
    public float distance;

    private List<GameObject> myCubes = new List<GameObject>();

    private void Start()
    {
        foreach (Transform trans in transform)
        {
            DestroyImmediate(trans.gameObject);
        }
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                Debug.Log(i * z + j);
                // create a cube for each iteration
                GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myCube.transform.SetParent(this.transform);

                // move the cube to the spot

                float height = Random.Range(3, 20);
                //height = i;

                //myCube.transform.localScale = new Vector3(1, height, 1);
                Vector3 myVector = new Vector3(3, height, j);
                myCube.transform.localScale = myVector;

                myCube.transform.localPosition = new Vector3(i * distance, height * 0.5f, j * distance);

                float value = Map(height, 0, x, 0, 1);
                //myCube.GetComponent<Renderer>().material.color = Color.HSVToRGB(value, 1, 1);

                myCubes.Add(myCube);
            }
        }
        Debug.Log(myCubes.Count);

        for (int i = 0; i < 10; i++)
        {
            int index = Random.Range(0, myCubes.Count);
            DestroyImmediate(myCubes[index]);
            myCubes.RemoveAt(index);
        }
    }
    private float Map(float value, float fromMin, float fromMax, float toMin, float toMax)
    {
        float delta = fromMax - fromMin;
        return toMin + ((toMax - toMin) / delta) * (value - fromMin);
    }
}
