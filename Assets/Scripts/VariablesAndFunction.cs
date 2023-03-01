using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VariablesAndFunction : MonoBehaviour
{
    // Start is called before the first frame update
    public int myInt;
    public bool myBool = true;
    string myString = "hello world";

    void Start()
    {
        //Debug.Log("this is start");
        int result;
        //result = myInt * 2;
        //Debug.Log(result);
        myInt = 10;

        myInt = MultiplyByTwo(myInt);
        Debug.Log(myInt);

        MultiplyByThree(myInt);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("this is update");
    }
    private int MultiplyByTwo(int number)
    {
        return number * 2;
    }
    private void MultiplyByThree(int number)
    {
        Debug.Log(number * 3);
    }
}
