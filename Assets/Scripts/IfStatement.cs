using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfStatement : MonoBehaviour
{

    void Start()
    {
        int a = 5;
        int b = 3;

        if(a > b)
        {
            Debug.Log("a is larger");
        } 
        else if (a < b)
        {
            Debug.Log("a is smaller");
        }
        else
        {
            Debug.Log("a is equal to b");
        }

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            GetComponent<Renderer>().material.color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            GetComponent<Renderer>().material.color = Color.blue;
        }

    }
}
