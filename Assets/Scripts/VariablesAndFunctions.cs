using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesAndFunctions : MonoBehaviour
{
    // 02
    int myInt = 5;
    bool myBool = true;
    string myString = "hello world!";

    //06 public int myInt;

    void Start()
    {
        // 01 start and update
        Debug.Log("start");
        
        // 02 variables 
        int result;
        result = myInt * 2;
        Debug.Log(result);

        // 05 change variables
        myInt = 10;

        // 03 functions
        result = MultiplyByTwo(myInt);
        Debug.Log(result);

        // 04
        MultiplyByThree(myInt);
    }

    // 01 start and update
    void Update()
    {
        Debug.Log("update");
    }

    // 03 functions
    private int MultiplyByTwo(int number)
    {
        int multiplyResult;
        multiplyResult = number * 2;
        return multiplyResult;

        // or
        // return number * 2;

    }

    // 04
    private void MultiplyByThree(int number)
    {
        Debug.Log(number * 3);
    }
}
