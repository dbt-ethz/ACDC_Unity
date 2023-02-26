using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfStatementAndInput : MonoBehaviour
{
    void Start()
    {
        // if statement
        int a = 5;
        int b = 3;

        if (a > b)
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

    // if statement and input
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)){
            GetComponent<Renderer>().material.color = Color.red;
        }  
        else if (Input.GetKeyDown(KeyCode.G)){
            GetComponent<Renderer>().material.color = Color.green;
        }
        else if (Input.GetKeyDown(KeyCode.B)){
            GetComponent<Renderer>().material.color = Color.blue;
        }

    }
}
