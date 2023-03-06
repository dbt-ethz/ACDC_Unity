using System.Collections;
using System.Collections.Generic;
//sing log4net.Util;
using UnityEngine;

[ExecuteAlways]
public class PolarArray : MonoBehaviour
{
    public int repeat = 2;
    public int startAngle = 0;
    public int endAngle = 360;
    public float radius;
    public GameObject prefab;
    void Start()
    {
        UpdateGeometry();
    }

    public void UpdateGeometry()
    {
        if (transform.childCount != repeat)
        {
            for (var i = transform.childCount - 1; i >= 0; i--)
            {
                Object.DestroyImmediate(transform.GetChild(i).gameObject);
            }

            for (var i = 0; i < repeat; i++)
            {
                //Instantiate(prefab, new Vector3(i * 2.0f, 0, 0), Quaternion.identity);
                GameObject go = Instantiate(prefab, this.gameObject.transform);
                //go.transform.Rotate(Vector3.up, i * deltaAngle);
                //go.transform.Translate(radius, 0, 0);
            }
        }
        rotateArray2();
    }

    void rotateArray()
    {
        float dAngle = endAngle - startAngle;
        float deltaAngle = dAngle / repeat;

        for (int i = 0; i < transform.childCount; i++)
        {
            float cAngle = deltaAngle * i + startAngle;
            GameObject go = transform.GetChild(i).gameObject;
            Vector3 parentRot = transform.rotation.eulerAngles;
            Quaternion quat = Quaternion.AngleAxis(-parentRot.y - cAngle, new Vector3(0, 1, 0));
            go.transform.rotation = quat;
        }
    }

    void rotateArray2()
    {
        float dAngle = endAngle - startAngle;
        float deltaAngle = dAngle / repeat;

        for (int i = 0; i < transform.childCount; i++)
        {
            Vector3 parentRot = transform.rotation.eulerAngles;

            float cAngle = deltaAngle * i + startAngle;
            float radians = Mathf.Deg2Rad * (cAngle + parentRot.y);
            float x = radius * Mathf.Cos(radians);
            float y = radius * Mathf.Sin(radians);
            GameObject go = transform.GetChild(i).gameObject;
            //float parentAngle = 0;

            // Quaternion.
            Quaternion quat = Quaternion.AngleAxis(-parentRot.y - cAngle, new Vector3(0, 1, 0));
            //Quaternion.
            //go.transform.localPosition = new Vector3(x, 0, y);
            //go.transform.RotateAround(new Vector3(0, 0, 0), Vector3.up, cAngle);
            go.transform.rotation = quat;
            go.transform.localPosition = new Vector3(x, 0, y);
            //go.transform.LookAt(this.transform.localPosition, Vector3.up);
            //, Quaternion.AngleAxis(cAngle, Vector3.up), 
        }

    }
    void OnValidate()
    {

    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log("updated");
        UpdateGeometry();
    }
}
