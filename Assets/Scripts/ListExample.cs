using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class ListExample : MonoBehaviour
{
    [Range (0, 20)]
    public int x;
    [Range(0, 20)]
    public int z;
    [Range(0, 5)]
    public float distance;

    // 06 list
    private List<GameObject> myCubes;
    private void Start()
    {
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < z; j++)
            {
                GameObject myCube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                myCube.transform.SetParent(this.transform);
                myCube.transform.localPosition = new Vector3(i * distance, 0, j * distance);

                float height = Random.Range(3, 10);
                myCube.transform.localScale = new Vector3(1, height, 1);
                myCube.transform.Translate(new Vector3(0, height * 0.5f, 0));

                float value = Map(i * x + j, 0, x * z, 0, 1);
                myCube.GetComponent<Renderer>().material.color = Color.HSVToRGB(value, 1, 1);

                // 06 add item to list
                myCubes.Add(myCube);

            }
        }

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
