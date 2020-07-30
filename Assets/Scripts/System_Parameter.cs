using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class System_Parameter : MonoBehaviour
{
    //for Robot_Motion
    public static GameObject R700;
    public static GameObject RJ2;
    public static GameObject RJ3;
    public static GameObject RJ4;
    public static GameObject RJ5;
    public static GameObject RJ6;
    public static GameObject TCP;

    //for operator
    public static GameObject Operator;

    void Start()
    {
        //指派Gameobject實體模型
        RJ2 = GameObject.Find("R700_J2");
        RJ3 = GameObject.Find("R700_J3");
        RJ4 = GameObject.Find("R700_J4");
        RJ5 = GameObject.Find("R700_J5");
        RJ6 = GameObject.Find("R700_J6");
        TCP = GameObject.Find("TCP");
        Operator = GameObject.Find("Operator");
        R700 = GameObject.Find("R700");
    }
}
